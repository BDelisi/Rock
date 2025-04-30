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
    public float speedMultiplier = 1.0f;
    public float massWeight = 1.0f;
    public float maxAirTime = 2.0f;

    private Rigidbody rb;
    private Collider coll;
    private float velo;
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
        if (inAir && timeInAir != maxAirTime)
        {
            timeInAir += Time.deltaTime;
            if (timeInAir > maxAirTime)
            {
                timeInAir = maxAirTime;
            }
        }
    }

    private void FixedUpdate()
    {
        velo = rb.velocity.magnitude * rb.mass;
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
        isHeld = false;
        rb.isKinematic = false;
        coll.isTrigger = false;
        transform.position = pos;
        transform.rotation = direction;
        gameObject.layer = 6;
        inAir = true;
        rb.AddForce(transform.forward * 2000 * speedMultiplier);
        velo = rb.velocity.magnitude;
        timeInAir = Time.deltaTime + .2f;
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
                //Debug.Log(size + " " + velo);

                if (timeInAir > .35f)
                {
                    collision.gameObject.GetComponent<PlayerController>().TakeDamage(velo * math.pow(size/8, massWeight) * timeInAir * damageMultiplier);
                }
                //collision.rigidbody.velocity += new Vector3(collision.impulse.y, 0, collision.impulse.x) * -10;
                //Debug.Log(collision.impulse);
                inAir = false;
            }
        }
    }
}
