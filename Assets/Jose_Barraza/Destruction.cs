using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    // Destrucción del ladrillo con sonido, puntos y partículas

    public AudioClip destroySound; // Clip de sonido al destruir
    public GameObject destroyParticlesPrefab; // Prefab de partículas
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            // Reproducir sonido
            if (destroySound != null && audioSource != null)
            {
                audioSource.PlayOneShot(destroySound);
            }

            // Reproducir partículas
            if (destroyParticlesPrefab != null)
            {
                Instantiate(destroyParticlesPrefab, transform.position, Quaternion.identity);
            }

            // Sumar puntos
            if (Ui.Inst != null)
            {
                Ui.Inst.AddPoints(10); // Cambia 10 si quieres dar más o menos puntos
            }

            // Mostrar pantalla si ya no quedan más objetos de este tipo
            if (GameObject.FindObjectsOfType<Destruction>().Length == 1)
            {
                if (Ui.Inst != null)
                {
                    Ui.Inst.ShowScreen();
                }
            }

            // Destruir el objeto después de que termine el sonido
            float delay = (destroySound != null) ? destroySound.length : 0f;
            Destroy(gameObject, delay);
        }
    }
}
