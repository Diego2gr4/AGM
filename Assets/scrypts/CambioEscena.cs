using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CambioEscena : MonoBehaviour
{
    public void ChangeSceneByIndex(int sceneIndex)
    {
        // Verifica que el índice esté dentro del rango válido
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(sceneIndex);
        }
        else
        {
            Debug.LogError("Índice de escena fuera de rango.");
        }
    }

    public void Cerrar (){
        Debug.Log("Cerrando Juego");
        Application.Quit();
    }
}
