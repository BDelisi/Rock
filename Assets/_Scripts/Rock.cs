using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public bool isHeld = false;
    public float size;
    public Vector2 sizeRange;
    public float damageMultiplier = .75f;

    private Rigidbody rb;
    private Collider coll;
    private Vector3 momentum;
    private float timeInAir;
    private bool inAir = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        size = UnityEngine.Random.Range(sizeRange.x, sizeRange.y);
        transform.localScale = new Vector3(size, size, size);
        rb.mass = size / 5;
    }

    private void Update()
    {
        momentum = rb.velocity * rb.mass;
        if (inAir && timeInAir != 1.5f)
        {
            timeInAir += Time.deltaTime;
            if (timeInAir > 1.5f)
            {
                timeInAir = 1.5f;
            }
        }
    }

    public void PickedUp()
    {
        isHeld = true;
        rb.isKinematic = true;
        coll.isTrigger = true;
        gameObject.layer = 0;
    }

    public void Throw(Vector3 pos, Quaternion direction)
    {
        isHeld=false;
        rb.isKinematic = false;
        coll.isTrigger = false;
        transform.position = pos;
        transform.rotation = direction;
        gameObject.layer = 6;
        rb.AddForce(transform.forward * 2000);
        inAir = true;
        timeInAir = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (inAir)
        {
            if (collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
            {
                inAir = false;
                //Debug.Log(momentum.magnitude * timeInAir * damageMultiplier);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage(momentum.magnitude * timeInAir * damageMultiplier);
                inAir = false;
            }
        }
    }
}
