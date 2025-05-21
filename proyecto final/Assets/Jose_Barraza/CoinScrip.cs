using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScrip : MonoBehaviour
{
    public int points = 10; // Los puntos que otorga la moneda
    public float rotationSpeed = 100f; // Velocidad de rotación
    public float moveSpeed = 1f; // Velocidad de movimiento
    public float moveRange = 0.5f; // Rango de movimiento
    public AudioClip destroySound; // Clip de sonido al destruir
    private AudioSource audioSource;

    private Vector3 startPosition;
    private float moveTime;

    // Para el movimiento aleatorio
    private Vector3 targetPosition;
    private float changeTargetTime;

    // Sistema de partículas
    public ParticleSystem coinParticles;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position; // Guardamos la posición inicial de la moneda

        // Establecemos un destino inicial aleatorio
        SetRandomTargetPosition();

        // Si el sistema de partículas está asignado, se asegura de que esté en funcionamiento
        if (coinParticles != null)
        {
            coinParticles.Play();
        }
    }

    private void Update()
    {
        // Hacer que la moneda gire continuamente sobre su propio eje (eje Y)
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);

        // Movimiento aleatorio: Cambiar el destino en intervalos
        if (Time.time >= changeTargetTime)
        {
            SetRandomTargetPosition(); // Cambiar el destino aleatorio
        }

        // Mover hacia la posición aleatoria objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Hacer que las partículas sigan la posición de la moneda
        if (coinParticles != null)
        {
            coinParticles.transform.position = transform.position;
        }
    }

    // Función para establecer un destino aleatorio dentro de un rango determinado
    private void SetRandomTargetPosition()
    {
        // Asignamos una nueva posición aleatoria en los tres ejes (X, Y, Z)
        targetPosition = startPosition + new Vector3(
            Random.Range(-moveRange, moveRange),  // Movimiento aleatorio en el eje X
            Random.Range(-moveRange, moveRange),  // Movimiento aleatorio en el eje Y
            Random.Range(-moveRange, moveRange)   // Movimiento aleatorio en el eje Z
        );

        // Establecemos un nuevo tiempo para cambiar el destino
        changeTargetTime = Time.time + Random.Range(1f, 3f); // Cambiar el destino cada 1-3 segundos
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

            // Detener las partículas y destruir la moneda
            if (coinParticles != null)
            {
                coinParticles.Stop();
            }

            // Destruir la moneda
            Destroy(gameObject);  // La moneda se destruye después de dar los puntos
        }
    }
}
