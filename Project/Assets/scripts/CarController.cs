using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float turnSpeed = 100f;

    private Rigidbody rb;
    private float moveInput, turnInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetInputs(float move, float turn)
    {
        moveInput = move;
        turnInput = turn;
    }

    void FixedUpdate()
    {
        // Input for forward movement
        //float moveInput = Input.GetAxis("Vertical");
        // Input for turning
        //float turnInput = Input.GetAxis("Horizontal");

        // Move the car forward
        Vector3 moveDirection = transform.forward * (moveInput * Time.fixedDeltaTime);
        rb.AddForce(moveDirection * moveSpeed, ForceMode.Impulse);

        // Rotate the car for turning
        float turn = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);
    }

    public void Stop()
    {
        rb.velocity = Vector3.zero;
    }
}
