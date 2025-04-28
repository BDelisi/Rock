using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject rock;
    public int rocks = 5;

    void Start()
    {
        for (int i = 0; i < rocks; i++)
        {
            Instantiate(rock, transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
