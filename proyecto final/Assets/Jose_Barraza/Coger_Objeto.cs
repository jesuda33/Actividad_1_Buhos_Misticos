using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coger_Objeto : MonoBehaviour
{
    public float tiempoAntesDeDestruir = 3f;        // Tiempo antes de destruir el objeto recogido
    public AudioClip sonidoRecoger;                // Sonido que se reproduce al recoger el objeto

    private AudioSource audioSource;

    private void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el objeto tiene el tag "Objeto", lo recoge y lo destruye después de unos segundos
        if (other.CompareTag("Objeto"))
        {
            if (sonidoRecoger != null && audioSource != null)
            {
                audioSource.PlayOneShot(sonidoRecoger);
            }

            // Habilitar la destrucción automática al objeto recogido
            Destruction destructionScript = other.GetComponent<Destruction>();
            if (destructionScript != null)
            {
                destructionScript.EnableAutoDestruction(); // Activar destrucción automática
            }

            StartCoroutine(SostenerYDestruir(other.gameObject));
        }
        // Si el objeto tiene el tag "Eliminar", lo destruye de inmediato
        else if (other.CompareTag("Eliminar"))
        {
            Destroy(other.gameObject);
        }
    }

    private IEnumerator SostenerYDestruir(GameObject objeto)
    {
        // Hacer hijo del personaje para que lo siga
        objeto.transform.SetParent(this.transform);

        // Ajustar posición relativa (puedes cambiar estos valores)
        objeto.transform.localPosition = new Vector3(0, 1, 0);

        // Esperar el tiempo antes de destruirlo
        yield return new WaitForSeconds(tiempoAntesDeDestruir);

        // Destruir el objeto
        Destroy(objeto);
    }
}
