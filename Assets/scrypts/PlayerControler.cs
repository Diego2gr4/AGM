using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float speed = 5f;  // Define la velocidad del jugador

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 
                            Input.GetAxis("Vertical") * speed * Time.deltaTime, 
                            0);
    }
}
