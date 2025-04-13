using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScrip : MonoBehaviour
{
    public int points = 10; // Los puntos que otorga la moneda
    public float rotationSpeed = 100f; // Velocidad de rotación
    public AudioClip destroySound; // Clip de sonido al destruir
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Hacer que la moneda gire continuamente en su propio eje
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el jugador entra en el trigger de la moneda
        if (other.CompareTag("player"))
        {
            // Reproducir sonido
            if (destroySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(destroySound);
            }

            // Sumar puntos al UI
            if (Ui.Inst != null)
            {
                Ui.Inst.AddPoints(points); // Se suman los puntos
            }

            // Destruir la moneda
            Destroy(gameObject);  // La moneda se destruye después de dar los puntos
        }
    }
}
