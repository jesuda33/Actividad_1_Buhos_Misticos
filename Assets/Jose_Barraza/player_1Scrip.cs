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

    // Zona de reaparición
    public Transform SpawnPoint;

    private Rigidbody Rb;

    // Componentes de Audio
    public AudioSource Source;
    public AudioClip JumpSound;
    public AudioClip[] StepSounds; // << Cambiado a arreglo de clips
    private float stepTimer = 0f;
    public float stepInterval = 0.5f;

    void Start()
    {
        Source = GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!IsWin && !Ui.Inst.Pause)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftShift))
            {
                Speed = MaxSpeed;
                stepInterval = 0.3f;
            }
            else
            {
                Speed = MinSpeed;
                stepInterval = 0.5f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * Speed);

            // Sonido de pasos
            if ((Mathf.Abs(x) > 0.1f || Mathf.Abs(y) > 0.1f) && IsGrounder)
            {
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepInterval && StepSounds.Length > 0)
                {
                    int index = Random.Range(0, StepSounds.Length); // << Escoge un sonido aleatorio
                    Source.PlayOneShot(StepSounds[index]);
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
