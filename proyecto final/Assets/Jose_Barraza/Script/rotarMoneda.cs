using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotarMoneda : MonoBehaviour
{
    public Vector3 direccion;

    void Update()
    {
        transform.Rotate(direccion);
    }
}
