using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Destruction : MonoBehaviour
{
    //Destucion del ladrillo

    /*
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            Destroy(gameObject);

            // linea aqui hay que cambiarla
            if (GameObject.FindObjectsOfType<Destruction>().Length == 1)
            {
                if(Ui.Inst != null)
                {
                    Ui.Inst.ShowScreen();

                }
                
            }

        }

        


    }

}
