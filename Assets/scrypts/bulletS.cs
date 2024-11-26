using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BULLET : MonoBehaviour
{
    public float Speed;
    public float maxTravelDistance = 12f;  // Distancia máxima que puede recorrer la bala

    private Rigidbody2D Rigidbody2D;
    private Vector3 Direction;
    private Vector3 startPosition;  // Almacena la posición inicial de la bala
    public AudioClip Sound; 

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;  // Guardar la posición inicial de la bala
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);

    }

    private void FixedUpdate()
    {
        // Aplicar la velocidad en la dirección asignada
        Rigidbody2D.velocity = Direction * Speed;

        // Calcular la distancia recorrida
        float distanceTravelled = Vector3.Distance(startPosition, transform.position);

        // Si la distancia recorrida es mayor o igual a la máxima permitida, destruir la bala
        if (distanceTravelled >= maxTravelDistance)
        {
            DestroyBullet();
        }
    }

    // Método para establecer la dirección de la bala
    public void SetDirection(Vector3 direction)
    {
        Direction = direction;
    }

    // Método para destruir la bala
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

   private void OnTriggerEnter2D(Collider2D other)
{
    // Detectar si colisiona con un Alien normal
    Alien alien = other.GetComponent<Alien>();
    if (alien != null)
    {
        alien.Hit();
    }

    // Detectar si colisiona con el AlienBoss
    AlienBoss alienBoss = other.GetComponent<AlienBoss>();
    if (alienBoss != null)
    {
        alienBoss.Hit();
    }

    // Detectar si colisiona con el Soldado
    Soldado_Movimiento soldado = other.GetComponent<Soldado_Movimiento>();
    if (soldado != null)
    {
        soldado.Hit();
    }

    // Destruir la bala después de colisionar con cualquiera de los objetos anteriores
    DestroyBullet();
}

}

////pruebaa