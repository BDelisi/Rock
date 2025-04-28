using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class RockPickup : MonoBehaviour
{
    public List<GameObject> pickups = new List<GameObject>();
    public GameObject selected = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        GameObject farthest = null;
        float distance= 100;
        foreach (var item in pickups)
        {
            if (Vector3.Distance(transform.position, item.transform.position) < distance && !item.GetComponent<Rock>().isHeld)
            {
                distance = Vector3.Distance(transform.position, item.transform.position);
                farthest = item;
            }
        }
        if (selected != null)
        {
            selected.GetComponent<Outline>().enabled = false;
        }
        if (farthest != null)
        {
            farthest.GetComponent<Outline>().enabled = true;
        }
        selected = farthest;
    }

    private void OnTriggerEnter(Collider other)
    {
        pickups.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        pickups.Remove(other.gameObject);
    }
}
