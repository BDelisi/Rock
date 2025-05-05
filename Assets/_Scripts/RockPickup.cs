using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPickup : MonoBehaviour
{
    public List<GameObject> pickups = new List<GameObject>();
    public GameObject selected = null;
    public Color color;

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
            if (item == null)
            {
                pickups.Remove(item);
            }
            if (Vector3.Distance(transform.position, item.transform.position) < distance && !item.GetComponent<Rock>().isHeld)
            {
                distance = Vector3.Distance(transform.position, item.transform.position);
                farthest = item;
            }
        }
        if (selected != null)
        {
            Outline theOutLine = selected.GetComponent<Outline>();
            //theOutLine.OutlineWidth = 0;
            theOutLine.enabled = false;
        }
        if (farthest != null)
        {
            Outline theOutLine = farthest.GetComponent<Outline>();
            //theOutLine.OutlineColor = color;
            //theOutLine.OutlineWidth = 10;
            theOutLine.enabled = true;

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
