using UnityEngine;

public class Soldado_Movimiento : MonoBehaviour
{
    public GameObject BulletPrefab;  // Prefab de la bala
    public float Speed = 5f;  // Velocidad por defecto
    public float JumpForce = 10f;  // Fuerza de salto por defecto

    public LayerMask GroundLayer;  // Capa que representa el suelo

    private Rigidbody2D Rigidbody2D;
    private Collider2D Collider2D;
    private float Horizontal;
    private bool Grounded;
    private Animator Animator;
    private int Health = 100;
    private float shootCooldown = 0.9f;  // Tiempo de espera entre disparos (1 segundo)
    public float lastShootTime=0;  // Tiempo del último disparo

    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Movimiento horizontal
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Cambiar la dirección del personaje según la dirección del movimiento
        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);  // Hacia la izquierda
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);  // Hacia la derecha
        }

        // Cambiar la animación de correr
        Animator.SetBool("caminar", Horizontal != 0.0f);

        // Verificar si el personaje está tocando el suelo usando el GroundLayer
        Grounded = Collider2D.IsTouchingLayers(GroundLayer);

        // Si el jugador presiona "W" y está en el suelo, saltar
        if (Input.GetKeyDown(KeyCode.W) && Grounded)
        {
            Jump();
        }

        // Disparo con la tecla espacio
        if (Input.GetKeyDown(KeyCode.Space))  // Disparar con la tecla espacio
        {
            Shoot();
        }
    }

    private void Jump()
    {
        // Aplicar fuerza de salto
        Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
        // Verificar si ha pasado suficiente tiempo desde el último disparo
        if (Time.time < lastShootTime + shootCooldown)
        {
            return;  // Si no ha pasado suficiente tiempo, salir de la función
        }

        // Activar la animación de disparo con un trigger
        Animator.SetTrigger("disparo");

        if (BulletPrefab == null) return;

        Vector3 direction;

        // Verificar la dirección del personaje según la escala
        if (transform.localScale.x == 2.0f)  // Hacia la derecha
            direction = Vector2.right;
        else  // Hacia la izquierda
            direction = Vector2.left;

        // Instanciar la bala en la posición correcta
GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f + new Vector3(0, 0.21f, 0), Quaternion.identity);

        // Asegurarse de que la bala recibe la dirección correcta
        BULLET bulletComponent = bullet.GetComponent<BULLET>();
        if (bulletComponent == null) return;
        bulletComponent.SetDirection(direction);

        // Actualizar el tiempo del último disparo
        lastShootTime = Time.time;
    }


    private void FixedUpdate()
    {
        // Aplicar velocidad horizontal
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}
