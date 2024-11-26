using UnityEngine;
using TMPro; // Necesario si usas TextMeshPro

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
    public GameObject GameOverCanvas;

    // Contador de balas
    public int MaxBullets = 100; // Máximo de balas
    private int currentBullets;
    public TextMeshProUGUI AmmoText; // Referencia al texto del UI para las balas
    private float shootCooldown = 0.9f;  // Tiempo de espera entre disparos
    public float lastShootTime = 0;  // Tiempo del último disparo
    public TextMeshProUGUI HealthText;
   

    private int Health = 10; // Salud inicial
   

    void Start()
    {
       Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();

        // Inicializar balas
        currentBullets = MaxBullets;
        UpdateAmmoUI(); // Mostrar las balas en pantalla
    }

    void Update()
    {
        // Movimiento horizontal
        Horizontal = Input.GetAxisRaw("Horizontal");

        // Cambiar la dirección del personaje según la dirección del movimiento
        if (Horizontal < 0.0f)
        {
            transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
        }
        else if (Horizontal > 0.0f)
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }

        // Cambiar la animación de correr
        Animator.SetBool("caminar", Horizontal != 0.0f);

        // Verificar si el personaje está tocando el suelo usando el GroundLayer
        Grounded = Collider2D.IsTouchingLayers(GroundLayer);

        // Si el jugador presiona "W" y está en el suelo, saltar
        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
        {
            Jump();
        }

        // Disparo con el clic izquierdo del mouse
        if (Input.GetMouseButtonDown(0))  // Click izquierdo
        {
            Shoot();
        }
    }

    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
    }

    private void Shoot()
    {
       // Verificar si hay balas disponibles
        if (currentBullets <= 0)
        {
            Debug.Log("Sin balas");
            return;
        }

        // Verificar si ha pasado suficiente tiempo desde el último disparo
        if (Time.time < lastShootTime + shootCooldown)
        {
            return;
        }

        // Activar la animación de disparo
        Animator.SetTrigger("disparo");

        if (BulletPrefab == null) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = (mousePosition - transform.position).normalized;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-2.0f, 2.0f, 2.0f);
        }

        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.5f + new Vector3(0, 0.21f, 0), Quaternion.identity);

        BULLET bulletComponent = bullet.GetComponent<BULLET>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDirection(direction);
        }

        // Reducir el contador de balas y actualizar la UI
        currentBullets--;
        UpdateAmmoUI();

        lastShootTime = Time.time;
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }
 private void UpdateAmmoUI()
    {
        // Actualizar el texto de balas en pantalla
        if (AmmoText != null)
        {
            AmmoText.text = "Balas: " + currentBullets.ToString();
        }
    }
     public void Reload(int bullets)
    {
        currentBullets = Mathf.Min(currentBullets + bullets, MaxBullets);
        UpdateAmmoUI();
    }
    public void Hit()
    {
        Health -= 1;
        UpdateHealthUI(); // Actualizar la UI al recibir daño

        if (Health == 0)
        {
            if (GameOverCanvas != null)
            {
                GameOverCanvas.SetActive(true);
            }

            Destroy(gameObject);
        }
    }

    private void UpdateHealthUI()
    {
        // Actualizar el texto del UI con la vida actual
        if (HealthText != null)
        {
            HealthText.text = "Vidas: " + Health.ToString();
        }
    }
    public void AddLife(int amount)
{
    Health += amount; // Aumentar la vida
    Debug.Log("Vida aumentada. Vida actual: " + Health);
     UpdateHealthUI();
}

}
