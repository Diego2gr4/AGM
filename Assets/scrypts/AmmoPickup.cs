using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    // Start is called before the first frame update
    public int AmmoAmount = 5; // Cantidad de balas que da este objeto

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Intentar obtener el script del jugador
            Debug.Log("Jugador detectado en el trigger.");
            Soldado_Movimiento player = other.GetComponent<Soldado_Movimiento>();

            if (player != null)
            {
                // Recargar balas en el jugador
                player.Reload(AmmoAmount);

                // Destruir el objeto de recarga
                Destroy(gameObject);
            }
        }
    }
}
