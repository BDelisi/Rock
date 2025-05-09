using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float health = 100f;
    public float moveSpeed = 1;
    public float xSens = .1f;
    public float ySens = .1f;
    public float sprintMultiplier = 1.5f;
    public bool isGrounded;
    public GameObject rightHand;
    public TextMeshProUGUI healthTMP;
    public Transform orientation;
    public Vector2 moveDirection = Vector2.zero;
    public AudioClip hurt;

    private Camera myCam;
    private Rigidbody rb;
    private Animator animator;
    private GameObject heldRock = null;
    private CapsuleCollider capsuleCollider;
    private PlayerInput playerInput;
    private AudioManager audioManager;
    private LayerMask layerMask;
    private float xRot = 0;
    private float yRot = 0;
    private float inputX;
    private float inputY;
    private float jumpCD = 0f;
    private float grabCD = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myCam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        yRot = 1 - math.radians(transform.rotation.eulerAngles.y);
        audioManager = FindFirstObjectByType<AudioManager>();
        layerMask = LayerMask.GetMask("Ground", "Rock");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveDirection.magnitude != 0)
        {
            if (playerInput.actions["Sprint"].IsPressed())
            {
                animator.SetBool("Running", true);
                rb.AddForce(transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime * sprintMultiplier);
            }
            else
            {
                animator.SetBool("Running", false);
                rb.AddForce(transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime);
            }
        }
        yRot += inputX;
        xRot -= inputY;
        xRot = math.clamp(xRot, -.95f, .95f);
        transform.rotation = quaternion.Euler(0, yRot, 0);
        orientation.rotation = quaternion.Euler(xRot, yRot, 0);
        if(jumpCD > 0 )
        {
            jumpCD -= Time.deltaTime;
        }
        if (grabCD > 0)
        {
            grabCD -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.CheckSphere(transform.position + Vector3.up * (capsuleCollider.radius * .8f), capsuleCollider.radius * 1f, layerMask);
        animator.SetBool("Grounded", isGrounded);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        animator.SetFloat("LRMovement", moveDirection.x);
        animator.SetFloat("FBMovement", moveDirection.y);
        //Debug.Log(moveDirection);
    }

    public void Look(InputAction.CallbackContext context)
    {
        Vector2 lookDirection = context.ReadValue<Vector2>();
        //Debug.Log(lookDirection);

        inputX = lookDirection.x * xSens * Time.deltaTime;
        inputY = lookDirection.y * ySens * Time.deltaTime;
        //Debug.Log(xRot);
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (heldRock != null)
        {
            heldRock.GetComponent<Rock>().Throw(myCam.transform.position + myCam.transform.rotation * new Vector3(0, 0, .6f)  + myCam.transform.rotation * new Vector3(0, 0, 1f) * (heldRock.GetComponent<Rock>().size/20), myCam.transform.rotation);
            heldRock.transform.parent = null;
            heldRock = null;
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {

    }

    public void Interact(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.started && grabCD <= 0f)
        {
            GameObject rockToPick = myCam.GetComponent<RockPickup>().selected;
            if (rockToPick != null && heldRock == null)
            {
                rockToPick.transform.position = rightHand.transform.position;
                rockToPick.transform.parent = rightHand.transform;
                rockToPick.GetComponent<Rock>().PickedUp();
                heldRock = rockToPick;
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log(context);
        if (context.started && isGrounded && jumpCD <= 0)
        {
            rb.AddForce(new Vector3(0, 3500, 0));
            jumpCD = .2f;
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        
    }

    public void TakeDamage(float amount)
    {
        Debug.Log(amount);
        health -= amount;
        healthTMP.SetText(Math.Round(health).ToString());
        if (health < 0)
        {
            Death();
        }
        audioManager.playSound(0f, hurt, 1f, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Debug.Log("Hit water");
        }
    }

    public void Death()
    {
        animator.Play("Death");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
