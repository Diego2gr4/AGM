using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camara : MonoBehaviour
{
    public Transform Soldado;

    void Update()
    {
        if (Soldado != null)
        {
            Vector3 position = transform.position;
            position.x = Soldado.position.x;
            transform.position = position;
        }
    }
}
