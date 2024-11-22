using System.Collections;
using UnityEngine;

public class EnemyNav : MonoBehaviour
{
    public float horizontalSpeed = 3f;  // Velocidad horizontal del enemigo
    public float verticalSpeed = 1f;   // Velocidad vertical hacia abajo
    public float horizontalLimit = 5f; // Límite horizontal del movimiento
    public GameObject shootPrefab;     // Prefab del proyectil
    public Transform shootOrigin;      // Punto de origen del disparo
    public Transform player;           // Referencia al jugador
    public float shotSpeed = 5f;       // Velocidad del disparo
    public float shootDistance = 10f;  // Distancia máxima para disparar al jugador
    public float bulletSpacing = 0.2f; // Tiempo entre disparos múltiples
    public float shootCooldown = 1f;   // Tiempo de espera entre disparos
    public float health = 3f;          // Salud del enemigo

    private int direction = 1;         // Dirección horizontal: 1 = derecha, -1 = izquierda
    private float lastShootTime;       // Tiempo del último disparo
    private Animator animator;         // Controlador de animaciones

    void Start()
    {
        // Buscar automáticamente al jugador por su tag
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Player'. Asegúrate de asignarlo correctamente.");
        }

        // Obtener el componente Animator, si existe
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (player == null) return;

        MoveHorizontally();
        MoveDownwards();
        HandleShooting();
    }

    void MoveHorizontally()
    {
        // Mover el enemigo horizontalmente en la dirección actual
        transform.position += new Vector3(horizontalSpeed * direction * Time.deltaTime, 0, 0);

        // Cambiar la dirección si se alcanza el límite horizontal
        if (transform.position.x >= horizontalLimit)
        {
            direction = -1;
        }
        else if (transform.position.x <= -horizontalLimit)
        {
            direction = 1;
        }
    }

    void MoveDownwards()
    {
        // Movimiento constante hacia abajo
        transform.position += new Vector3(0, -verticalSpeed * Time.deltaTime, 0);
    }

    void HandleShooting()
    {
        // Verificar si el jugador está dentro de la distancia de disparo y si se puede disparar
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= shootDistance && Time.time > lastShootTime + shootCooldown)
        {
            StartCoroutine(ShootMultipleBullets());
            lastShootTime = Time.time;
        }
    }

    private IEnumerator ShootMultipleBullets()
    {
        if (animator != null)
        {
            animator.SetTrigger("disparoa");
        }

        // Disparar múltiples balas con un pequeño retraso entre cada disparo
        for (int i = 0; i < 3; i++) // Cambia 3 por el número de balas deseado
        {
            ShootAtPlayer();
            yield return new WaitForSeconds(bulletSpacing);
        }
    }

    private void ShootAtPlayer()
    {
        if (shootPrefab == null || shootOrigin == null || player == null) return;

        // Crear el proyectil
        GameObject shot = Instantiate(shootPrefab, shootOrigin.position, Quaternion.identity);

        // Calcular la dirección hacia el jugador y normalizarla
        Vector3 direction = (player.position - shootOrigin.position).normalized;

        // Aplicar dirección y velocidad al proyectil
        Rigidbody2D rb = shot.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * shotSpeed;
        }
    }

    public void Hit()
    {
        // Reducir la salud cuando recibe daño
        health -= 1;

        // Destruir el objeto si la salud llega a 0
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
