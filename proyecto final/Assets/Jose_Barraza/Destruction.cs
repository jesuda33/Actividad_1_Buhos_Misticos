using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    [Header("Audio y partículas")]
    public AudioClip destroySound;
    public AudioClip finalDestroySound; // Nuevo sonido al destruir completamente
    public GameObject destroyParticlesPrefab;
    private AudioSource audioSource;

    [Header("Configuración de destrucción")]
    [Range(0f, 1f)]
    public float destructionChance = 0.5f;

    [Tooltip("Si está activado, los efectos se reproducen aunque el objeto no se destruya.")]
    public bool efectosSiempre = true;

    [Header("Partículas adicionales")]
    public GameObject additionalParticlesPrefab;

    [Header("Saltos necesarios para destruir")]
    public int requiredJumps = 5;
    private int currentHits = 0;

    private bool isAutoDestructionEnabled = false;

    [Header("Movimiento vertical")]
    public float verticalMoveAmount = 0.5f;
    public float moveDuration = 0.2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            currentHits++;

            if (isAutoDestructionEnabled)
            {
                DestroyObject();
            }

            if (currentHits < requiredJumps)
            {
                if (efectosSiempre)
                {
                    PlayEffects();
                }
                return;
            }

            bool seDestruye = Random.value <= destructionChance;

            if (seDestruye || efectosSiempre)
            {
                PlayEffects();
            }

            if (seDestruye)
            {
                DestroyObject();
            }
        }
    }

    private void DestroyObject()
    {
        // Sumar puntos
        if (Ui.Inst != null)
        {
            Ui.Inst.AddPoints(10);
        }

        // Mostrar pantalla si es el último bloque
        if (GameObject.FindObjectsOfType<Destruction>().Length == 1)
        {
            if (Ui.Inst != null)
            {
                Ui.Inst.ShowScreen();
            }
        }

        // Reproducir sonido final de destrucción desde un objeto temporal
        if (finalDestroySound != null)
        {
            GameObject tempAudio = new GameObject("TempAudio");
            tempAudio.transform.position = transform.position;
            AudioSource tempSource = tempAudio.AddComponent<AudioSource>();
            tempSource.PlayOneShot(finalDestroySound);
            Destroy(tempAudio, finalDestroySound.length);
        }

        // Destruir el bloque (sin delay porque el sonido ya se reproduce aparte)
        Destroy(gameObject);
    }

    private void PlayEffects()
    {
        // Reproducir sonido
        if (destroySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(destroySound);
        }

        // Partículas de destrucción
        if (destroyParticlesPrefab != null)
        {
            Destroy(Instantiate(destroyParticlesPrefab, transform.position, Quaternion.identity), destroySound?.length ?? 2f);
        }

        // Partículas adicionales
        if (additionalParticlesPrefab != null)
        {
            Destroy(Instantiate(additionalParticlesPrefab, transform.position, Quaternion.identity), destroySound?.length ?? 2f);
        }

        // Mover verticalmente
        StartCoroutine(MoveVertically());
    }

    private IEnumerator MoveVertically()
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + Vector3.up * verticalMoveAmount;
        float elapsedTime = 0f;

        // Movimiento hacia arriba
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        // Movimiento hacia abajo
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPos, startPos, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }

    // Nueva función para habilitar la destrucción automática
    public void EnableAutoDestruction()
    {
        isAutoDestructionEnabled = true;
    }
}
