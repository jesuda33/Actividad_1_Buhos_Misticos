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

    public Transform SpawnPoint;

    private Rigidbody Rb;

    public AudioSource Source;
    public AudioClip JumpSound;
    public AudioClip[] StepSounds;
    public AudioClip ZoneSound; // NUEVO: sonido al pasar por zona
    public AudioClip SoundCoin; // NUEVO: sonido para la moneda

    private float stepTimer = 0f;
    public float stepInterval = 0.5f;

    private bool isFrozen = false; // controla si el jugador está congelado

    void Start()
    {
        Source = GetComponent<AudioSource>();
        Rb = GetComponent<Rigidbody>();
        StartCoroutine(FreezePlayer(3f)); // Congela al inicio
    }

    void Update()
    {
        if (isFrozen || IsWin || Ui.Inst.Pause) return;

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
                int index = Random.Range(0, StepSounds.Length);
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

    public void Jump()
    {
        if (IsGrounder && !isFrozen)
        {
            Rb.AddForce(new Vector3(0, Jumpforce, 0), ForceMode.Impulse);
            Source.PlayOneShot(JumpSound);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounder = true;
        }

        if (collision.gameObject.tag == "DeathZone")
        {
            transform.position = SpawnPoint.position;
            StartCoroutine(FreezePlayer(3f));
        }

        if (collision.gameObject.CompareTag("Objeto"))
        {
            Destruction destructionScript = collision.gameObject.GetComponent<Destruction>();
            if (destructionScript != null)
            {
                destructionScript.EnableAutoDestruction();
                Debug.Log("Destrucción habilitada para el objeto: " + collision.gameObject.name);
            }
        }

        // NUEVO: Reproducción de sonido al colisionar con un objeto específico (moneda)
        if (collision.gameObject.CompareTag("Coin")) // Verifica el tag "Coin"
        {
            if (SoundCoin != null) // Asegúrate de tener el sonido asignado en el inspector
            {
                Source.PlayOneShot(SoundCoin); // Reproduce el sonido
                Debug.Log("Reproduciendo sonido de moneda con: " + collision.gameObject.name);
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounder = false;
        }
    }

    // NUEVO: detección de zona de sonido
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZonaSonido"))
        {
            if (ZoneSound != null)
            {
                Source.PlayOneShot(ZoneSound);
                Debug.Log("Reproduciendo sonido de zona: " + other.name);
            }

            // Si quieres que la zona desaparezca después de usarse, descomenta esto:
            // Destroy(other.gameObject);
        }

        // Si tocas una zona con tag "Coin", reproduce el sonido
        if (other.CompareTag("Coin")) // Verifica el tag "Coin"
        {
            if (SoundCoin != null) // Asegúrate de tener el sonido asignado en el inspector
            {
                Source.PlayOneShot(SoundCoin); // Reproduce el sonido
                Debug.Log("Reproduciendo sonido de moneda con: " + other.gameObject.name);
            }
        }
    }

    private IEnumerator FreezePlayer(float seconds)
    {
        isFrozen = true;
        yield return new WaitForSeconds(seconds);
        isFrozen = false;
    }
}
