using UnityEngine;

public class ButtonToggleCanvas : MonoBehaviour
{
    public GameObject targetCanvas; // Canvas que será visible u ocultado

    // Función para mostrar el Canvas
    public void ShowCanvas()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(true); // Activar el Canvas
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas en el Inspector.");
        }
    }

    // Función para ocultar el Canvas
    public void HideCanvas()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(false); // Desactivar el Canvas
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas en el Inspector.");
        }
    }

    // Función para alternar entre mostrar y ocultar
    public void ToggleCanvas()
    {
        if (targetCanvas != null)
        {
            targetCanvas.SetActive(!targetCanvas.activeSelf); // Alternar el estado del Canvas
        }
        else
        {
            Debug.LogWarning("No se asignó un Canvas en el Inspector.");
        }
    }
}
