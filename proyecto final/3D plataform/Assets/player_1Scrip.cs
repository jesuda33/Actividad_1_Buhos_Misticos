using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_1Scrip : MonoBehaviour
{
    private float Speed = 12f;
    public float MinSpeed = 8f;
    public float MaxSpeed = 18f;

    public float Jumpforce = 8f;

    public bool IsGrounder;

    private Rigidbody Rb;
    // Start is called before the first frame update
    void Start()
    {
        Rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //MOVIMIENTO

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = MaxSpeed;

        }
        else
        {
            Speed = MinSpeed;

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();

        }

        transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * Speed);
        
    }

    public void Jump()
    {
        if (IsGrounder)
        {
            Rb.AddForce(new Vector3(0, Jumpforce, 0), ForceMode.Impulse);
        }
        

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounder = true;

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounder = false;

        }

    }
}
