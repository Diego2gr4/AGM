using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{
    public float Speed;
    public float maxTravelDistance = 12f;

    private Rigidbody2D Rigidbody2D;
    private Vector3 Direction;
    private Vector3 startPosition;

    private GameObject shooter; // Referencia al objeto que disparó la bala
    public AudioClip Sound;
   private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // Asignar la capa "AlienBossBullet" a la bala para que no colisione con otras balas
        gameObject.layer = LayerMask.NameToLayer("AlienBossBullet");
        Camera.main.GetComponent<AudioSource>().PlayOneShot(Sound);

    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = Direction * Speed;

        float distanceTravelled = Vector3.Distance(startPosition, transform.position);

        if (distanceTravelled >= maxTravelDistance)
        {
            DestroyBullet();
        }
    }

    // Método para establecer la dirección y el objeto que dispara
    public void SetDirection(Vector3 direction, GameObject shooter)
    {
        Direction = direction;
        this.shooter = shooter; // Almacenar el objeto que disparó
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Ignorar colisiones con el objeto que disparó la bala
        if (other.gameObject == shooter)
        {
            return;
        }

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
