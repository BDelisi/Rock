using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float xSens = .1f;
    public float ySens = .1f;
    public float sprintMultiplier = 1.5f;
    public bool isGrounded;
    public Transform orientation;

    private Camera myCam;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private float xRot = 0;
    private float yRot = 0;
    private Vector2 moveDirection = Vector2.zero;
    private float inputX;
    private float inputY;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myCam = GetComponentInChildren<Camera>();
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.actions["Sprint"].IsPressed())
        {
            rb.AddForce(transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime * sprintMultiplier);
        }
        else
        {
            rb.AddForce(transform.rotation * new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime);
        }
        yRot += inputX;
        xRot -= inputY;
        xRot = math.clamp(xRot, -.9f, .9f);
        transform.rotation = quaternion.Euler(0, yRot, 0);
        orientation.rotation = quaternion.Euler(xRot, yRot, 0);
        isGrounded = Physics.CheckSphere(transform.position, 1f , 3);
        Debug.Log(isGrounded);
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
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

    }

    public void Aim(InputAction.CallbackContext context)
    {

    }

    public void Interact(InputAction.CallbackContext context)
    {

    }

    public void Jump(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        if (context.performed)
        {
            rb.AddForce(new Vector3(0, 3500, 0));
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        
    }
}
