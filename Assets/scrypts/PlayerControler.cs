using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5f;  // Define la velocidad del jugador
    public GameObject shootPrefab;
    public Transform shootOrigin;  // Agregué el punto y coma al final

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 
                            Input.GetAxis("Vertical") * speed * Time.deltaTime, 
                            0);

        // Disparar con la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(shootPrefab, shootOrigin.position, shootOrigin.rotation);
        }
    }
}
