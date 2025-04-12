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
    private bool IsWin;

    //zona de reaparicion

    public Transform SpawnPoint;

    private Rigidbody Rb;
    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody>();
        
    }

    //Componentes de Audio
    public AudioSource Source;
    public AudioClip JumpSound;
    public AudioClip StepSound; // << Agregado para pasos
    private float stepTimer = 0f; // << Temporizador para evitar que suene todo el tiempo
    public float stepInterval = 0.5f; // << Tiempo entre pasos (ajustable)

    // Update is called once per frame
    void Update()
    {
        //MOVIMIENTO

        if (!IsWin && !Ui.Inst.Pause)
        {

            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Speed = MaxSpeed;
                stepInterval = 0.3f; // << Pasos más rápidos al correr
            }
            else
            {
                Speed = MinSpeed;
                stepInterval = 0.5f; // << Pasos normales al caminar
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();

            }

            transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * Speed);

            // Aquí agregas el sonido de pasos
            if ((Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f) && IsGrounder)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepInterval)
                {
                    Source.PlayOneShot(StepSound);
                    stepTimer = 0f;
                }
            }
            else
            {
                stepTimer = stepInterval;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Ui.Inst.ShowPauseScreen();
            }

        }

        
    }

    public void Jump()
    {
        if (IsGrounder)
        {
            Rb.AddForce(new Vector3(0, Jumpforce, 0), ForceMode.Impulse);
            Source.PlayOneShot(JumpSound);
        }
        

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounder = true;

        }

        if (collision.gameObject.tag == "DeathZone")
        {
            transform.position = SpawnPoint.position;

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
