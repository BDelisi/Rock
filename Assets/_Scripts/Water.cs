using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private GameManager gameManager;


    private void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().Death();
            Debug.Log("Touched water");
        } else  if (other.gameObject.CompareTag("Rock"))
        {
            Destroy(other.gameObject);
            gameManager.SpawnRock();
        }
    }
}
