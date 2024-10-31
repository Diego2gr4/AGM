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

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;  // Guardar la posición inicial de la bala
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
        Alien alien = other.GetComponent<Alien>();
        Soldado_Movimiento soldado = other.GetComponent<Soldado_Movimiento>();
        if (alien != null)
        {
            alien.Hit();
        }
        if (soldado != null)
        {
            soldado.Hit();
        }
        DestroyBullet();
    }
}
