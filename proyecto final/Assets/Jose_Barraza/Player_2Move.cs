using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_2Move : MonoBehaviour
{
    public float RunSpeed = 12f;
    public float RotationSpeed = 250;


    public Animator animator;
    private float x, y;

    public Rigidbody rb;
    public float JumpHeight = 3;

    public Transform GroundCheck;
    public float GroundDistance = 0.3f;
    public LayerMask GroundMask;

    bool isGrounded;



    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        transform.Rotate(0, x * Time.deltaTime * RotationSpeed, 0);
        transform.Translate(0, 0, y * Time.deltaTime * RunSpeed);

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        // Verifica si está en el suelo
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        // Salto solo si está en el suelo
        if (Input.GetKey("space") && isGrounded)
        {
            animator.Play("jump");
            Invoke("jump",1f); // Llama directamente sin delay
        }
    }

    public void Jump()
    {
        rb.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;

        }

    }

}
