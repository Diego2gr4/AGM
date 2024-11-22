using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisparo : MonoBehaviour
{
    public Vector3 direction;   // Dirección de movimiento de la bala
    public float speed = 5f;    // Velocidad de la bala
    public float maxTravelDistance = 10f; // Distancia máxima que puede recorrer antes de ser destruido

    private Vector3 startPosition; // Posición inicial de la bala

    void Start()
    {
        // Registrar la posición inicial
        startPosition = transform.position;
    }

    void Update()
    {
        // Mover la bala en la dirección especificada
        transform.Translate(direction.normalized * speed * Time.deltaTime);

        // Calcular la distancia recorrida
        float distanceTravelled = Vector3.Distance(startPosition, transform.position);

        // Si la distancia recorrida es mayor o igual a la máxima permitida, destruir la bala
        if (distanceTravelled >= maxTravelDistance)
        {
            DestroyBullet();
        }
    }

    // Método para establecer la dirección de la bala
    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized; // Normalizar la dirección para que tenga magnitud 1
    }

    // Método para destruir la bala
    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    // Método llamado cuando la bala colisiona con otro objeto
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar el tag del objeto con el que colisionó (puedes modificar los tags según tus necesidades)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ejemplo: Aplicar daño al jugador (suponiendo que tiene un script con un método de daño)
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Reducir la salud del jugador en 1 (puedes personalizar esto)
            }
        }

        // Destruir la bala al impactar con cualquier objeto
        DestroyBullet();
    }
}
