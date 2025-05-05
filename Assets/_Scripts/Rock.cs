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
    public float timeWeight = 1.0f;
    public float maxAirTime = 2.0f;
    public AudioClip hitSound;
    public AudioClip bonk;

    private Rigidbody rb;
    private Collider coll;
    private AudioManager audioManager;
    private Vector3 velo;
    private float timeInAir;
    private bool inAir = false;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
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
        velo = rb.velocity;
        timeInAir = Time.deltaTime + .2f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (inAir)
        {
            if (collision.gameObject.layer == 3 || collision.gameObject.layer == 6)
            {
                inAir = false;
                audioManager.playSound(transform.position, hitSound, 1 - size/46, 1f);
                //Debug.Log(momentum.magnitude * timeInAir * damageMultiplier);
            }
            else if (collision.gameObject.CompareTag("Player"))
            {
                //Debug.Log(size + " " + velo);

                if (timeInAir > .4f)
                {
                    collision.gameObject.GetComponent<PlayerController>().TakeDamage(math.pow((size/8) + 1, massWeight) * math.pow(timeInAir, timeWeight) * damageMultiplier);
                    audioManager.playSound(transform.position, bonk, 1f , 1f);
                }
                //collision.rigidbody.AddForce(transform.rotation * Quaternion.FromToRotation(transform.position, collision.transform.position) * Vector3.forward * velo * math.pow(size/8, massWeight));
                //collision.rigidbody.AddForceAtPosition(velo * math.pow(size / 8, massWeight), collision.contacts[0].point);
                //Debug.Log(collision.impulse);
                //collision.rigidbody.AddForce(new Vector3(collision.impulse.x * -500,collision.impulse.y * -5, collision.impulse.z * -500));
                //Debug.Log(collision.impulse);
                inAir = false;
            }
        }
    }
}
