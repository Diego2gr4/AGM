using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienBoss : MonoBehaviour
{
    public Transform Soldado;  // Referencia al personaje que está siendo atacado (Soldado)
    public GameObject BulletPrefab;  // Prefab de la bala
    private Animator Animator;
public static bool BossDefeated = false; // Indica si el boss fue destruido
    private int Health = 15;  // Salud del enemigo
    private float LastShoot;  // Tiempo del último disparo
    public float ShootCooldown = 1f;  // Tiempo de espera entre disparos
    public float BulletSpacing = 0.2f;  // Tiempo entre balas

    private Rigidbody2D Rigidbody2D;
    private Vector3 lastSoldadoPosition; // Almacenar la última posición conocida del Soldado

    // Variables para saltos
    public float JumpForce = 1f;   // Fuerza del salto
    public float JumpDistance = 2f; // Distancia horizontal del salto
    public float JumpInterval = 3f; // Intervalo de tiempo entre ciclos
    public LayerMask GroundLayer;  // Asigna la capa del suelo desde el inspector


    void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();

        // Iniciar la corutina del salto
        StartCoroutine(JumpCycle());
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
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);  // Mirando a la derecha
        }
        else
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);  // Mirando a la izquierda
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
    Animator.SetTrigger("DisparoBoss");

    for (int i = 0; i < 2; i++)  // Disparar dos balas
    {
        // Disparar una bala desde la mano izquierda
        if (i == 0)
        {
            Shoot(new Vector3(-2f, -2.5f, 0));  // Desplazamiento hacia la izquierda
        }
        // Disparar una bala desde la mano derecha
        else if (i == 1)
        {
            Shoot(new Vector3(2f, -2.5f, 0));  // Desplazamiento hacia la derecha
        }
        yield return new WaitForSeconds(BulletSpacing);  // Esperar antes de disparar la siguiente
    }

    Animator.SetTrigger("idleB");
}

private void Shoot(Vector3 handOffset)
{
    if (BulletPrefab == null) return;

    // Obtener la posición actual del Soldado
    Vector3 direction = (Soldado.position - transform.position).normalized;

    // Crear una nueva posición ajustada según el desplazamiento de las manos
    Vector3 shootPosition = transform.position + handOffset;

    // Instanciar la bala en la nueva posición
    GameObject bullet = Instantiate(BulletPrefab, shootPosition, Quaternion.identity);

    BulletBoss bulletComponent = bullet.GetComponent<BulletBoss>();
    if (bulletComponent != null)
    {
        bulletComponent.SetDirection(direction, gameObject); // Pasar referencia del disparador
    }
}



    public void Hit()
    {
        Debug.Log("Recibio el disparo");

        // Reducir la salud cuando recibe daño
        Health -= 1;

        // Si la salud llega a 0, destruir el objeto
        if (Health <= 0)
        {
            BossDefeated = true;
            Destroy(gameObject);
        }
    }

    private IEnumerator JumpCycle()
    {
        while (true)
        {
            // Salto a la derecha
            yield return Jump(new Vector2(JumpDistance, JumpForce));

            // Esperar un poco en el aire antes de volver
            yield return new WaitForSeconds(0.5f);

            // Salto de regreso al punto inicial
            yield return Jump(new Vector2(-JumpDistance, JumpForce));

            // Esperar un poco en el aire antes del siguiente salto
            yield return new WaitForSeconds(0.5f);

            // Salto a la izquierda
            yield return Jump(new Vector2(-JumpDistance, JumpForce));

            // Esperar un poco en el aire antes de volver al centro
            yield return new WaitForSeconds(0.5f);

            // Salto de regreso al punto inicial
            yield return Jump(new Vector2(JumpDistance, JumpForce));

            // Esperar 20 segundos antes de repetir el ciclo
            yield return new WaitForSeconds(JumpInterval);
        }
    }

    private IEnumerator Jump(Vector2 jumpDirection)
    {
        // Aplicar fuerza al Rigidbody2D
        Rigidbody2D.AddForce(jumpDirection, ForceMode2D.Impulse);

        // Activar animación de salto (opcional)
        Animator.SetTrigger("jump");

        // Esperar hasta que el AlienBoss aterrice (simulación)
        yield return new WaitForSeconds(1f);
    }
    private bool IsGrounded()
{
    float rayLength = 0.1f;  // Longitud del raycast
    Vector2 origin = transform.position;  // Punto de origen del raycast
    RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, GroundLayer);
    
    Debug.DrawRay(origin, Vector2.down * rayLength, Color.red);  // Visualizar el raycast
    return hit.collider != null;  // Devuelve true si colisiona con algo en la capa de suelo
}
}
