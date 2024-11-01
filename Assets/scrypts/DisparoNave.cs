using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisparoNave : MonoBehaviour
{
   public Vector3 direction;  // Cambié 'vector3' a 'Vector3'
   public float speed;

    // Update is called once per frame
    void Update()
    {   // traslade a la dirección que le ponga x, y, z con velicidad establecida
        transform.Translate(direction * speed * Time.deltaTime);
    }
}

