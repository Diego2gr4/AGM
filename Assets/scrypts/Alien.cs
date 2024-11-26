using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public Transform Soldado;  // Referencia al personaje que está siendo atacado (Soldado)
    public GameObject BulletPrefab;  // Prefab de la bala
    private Animator Animator;

    private int Health = 3;  // Salud del enemigo
    private float LastShoot;  // Tiempo del último disparo
    public float ShootCooldown = 1f;  // Tiempo de espera entre disparos
    public float BulletSpacing = 0.2f;  // Tiempo entre balas

    private Rigidbody2D Rigidbody2D;
    private Vector3 lastSoldadoPosition; // Almacenar la última posición conocida del Soldado

    void Start()
    {
        transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Soldado == null)
        {
            return;  // Si no hay objetivo, salir de la función
        }

        // Actualizar la última posición conocida del Soldado
        lastSoldadoPosition = Soldado.position;

        // Calcular la dirección hacia el jugador
        Vector3 direction = Soldado.position - transform.position;

        // Cambiar la dirección del enemigo dependiendo de la posición de Soldado
        if (direction.x > 0.0f)
        {
            transform.localScale = new Vector3(-0.18f, 0.18f, 0.18f);  // Mirando a la derecha
        }
        else
        {
            transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);  // Mirando a la izquierda
        }

        // Disparar si la distancia es menor a 9.0f y ha pasado suficiente tiempo desde el último disparo
        float distance = Vector3.Distance(Soldado.position, transform.position); // Distancia total
        if (distance < 11.0f && Time.time > LastShoot + ShootCooldown)
        {
            StartCoroutine(ShootMultipleBullets());  // Iniciar la secuencia de disparo
            LastShoot = Time.time;  // Actualizar el tiempo del último disparo
        }
    }

    private IEnumerator ShootMultipleBullets()
    {
        // Activar animación de disparo
        Animator.SetTrigger("disparoa");

        for (int i = 0; i < 1; i++)  // Disparar 1 bala
        {
            Shoot();
            yield return new WaitForSeconds(BulletSpacing);  // Esperar antes de disparar la siguiente
        }

        // Volver a la animación por defecto
        Animator.SetTrigger("idle");
    }

    private void Shoot()
    {
        if (BulletPrefab == null) return;

        // Calcular la dirección hacia la última posición conocida del Soldado
        Vector3 direction = (lastSoldadoPosition - transform.position).normalized;

        // Instanciar la bala en la posición correcta
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f, Quaternion.identity);

        // Asegurarse de que la bala recibe la dirección correcta
        BulletA bulletComponent = bullet.GetComponent<BulletA>();
        if (bulletComponent == null) return;
        bulletComponent.SetDirection(direction);
    }

    public void Hit()
    {
        // Reducir la salud cuando recibe daño
        Health -= 1;

        // Si la salud llega a 0, destruir el objeto
        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
