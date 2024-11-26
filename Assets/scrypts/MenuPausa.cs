using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    [SerializeField] private GameObject BotonPausa; // Botón para pausar el juego
    [SerializeField] private GameObject Menupausa; // Menú que aparece cuando el juego está pausado
    [SerializeField] private MonoBehaviour Soldado_Movimiento; // Script que controla el movimiento del soldado

    private bool JuegoPausado = false; // Estado del juego (en pausa o no)

    private void Start()
    {
        // Validar que las referencias estén asignadas
        if (BotonPausa == null || Menupausa == null || Soldado_Movimiento == null)
        {
            Debug.LogError("Faltan referencias en el script MenuPausa. Por favor, verifica las asignaciones en el Inspector.");
        }

        // Asegurarse de que el menú de pausa esté desactivado al iniciar
        Menupausa.SetActive(false);
    }

    private void Update()
    {
        // Detectar la tecla Escape para pausar o reanudar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (JuegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausa();
            }
        }
    }

    public void Pausa()
    {
        JuegoPausado = true; // Cambiar el estado a pausado
        Time.timeScale = 0f; // Detener el tiempo del juego
        BotonPausa.SetActive(false); // Ocultar el botón de pausa
        Menupausa.SetActive(true); // Mostrar el menú de pausa
        Soldado_Movimiento.enabled = false; // Desactivar el control del soldado
    }

    public void Reanudar()
    {
        JuegoPausado = false; // Cambiar el estado a reanudado
        Time.timeScale = 1f; // Restaurar el tiempo del juego
        BotonPausa.SetActive(true); // Mostrar el botón de pausa
        Menupausa.SetActive(false); // Ocultar el menú de pausa
        Soldado_Movimiento.enabled = true; // Reactivar el control del soldado
    }

    public void Reiniciar()
    {
        Time.timeScale = 1f; // Restaurar el tiempo antes de reiniciar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Cargar la escena actual de nuevo
    }
}
