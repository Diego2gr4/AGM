using System.Collections;
using UnityEngine;

public class ExitPointTrigger : MonoBehaviour
{
    public GameObject GameOverCanvas; // Canvas que se mostrará al terminar el nivel
    public float ascendSpeed = 2f;   // Velocidad a la que el objeto subirá
    public float destroyDelay = 4f;  // Tiempo antes de destruir el objeto
    public AudioClip Sound;          // Clip de sonido para reproducir
    private AudioSource audioSource; // Fuente de audio para reproducir el sonido

    private bool isAscending = false; // Bandera para indicar si el objeto está ascendiendo

    private void Start()
    {
        // Obtener el AudioSource principal (asegúrate de que la cámara o un objeto tenga uno)
        audioSource = Camera.main.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en la cámara principal.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Verificar si el jefe ha sido derrotado antes de continuar
            if (AlienBoss.BossDefeated)
            {
                // Mostrar el Canvas
                if (GameOverCanvas != null)
                {
                    GameOverCanvas.SetActive(true);
                }

                // Reproducir el sonido (si está configurado)
                if (audioSource != null && Sound != null)
                {
                    audioSource.PlayOneShot(Sound);
                }
                else
                {
                    Debug.LogWarning("Sonido o AudioSource no configurado correctamente.");
                }

                // Eliminar el objeto del jugador
                Destroy(other.gameObject);

                // Iniciar el ascenso del objeto
                isAscending = true;

                // Destruir el objeto que contiene este script después de un retraso
                Invoke(nameof(DestroySelf), destroyDelay);
            }
            else
            {
                Debug.Log("El jefe sigue vivo, no se puede completar el nivel.");
            }
        }
    }

    private void Update()
    {
        // Si el objeto está ascendiendo, moverlo hacia arriba
        if (isAscending)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
        }
    }

    private void DestroySelf()
    {
        // Destruir el objeto que contiene este script
        Destroy(gameObject);
    }
}
