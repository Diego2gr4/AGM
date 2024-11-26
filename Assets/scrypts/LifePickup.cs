using UnityEngine;

public class LifePickup : MonoBehaviour
{
    public int lifeAmount = 1; // Cantidad de vida que otorga este objeto
    public AudioClip pickupSound; // Sonido que se reproduce al recoger la vida

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Obtener el componente de vida del jugador
            Soldado_Movimiento player = other.GetComponent<Soldado_Movimiento>();
            if (player != null)
            {
                // Aumentar la vida del jugador
                player.AddLife(lifeAmount);

                // Reproducir el sonido del pickup
                if (pickupSound != null)
                {
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                }

                // Destruir el objeto de vida
                Destroy(gameObject);
            }
        }
    }
}
