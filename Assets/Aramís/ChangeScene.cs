using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string nombre)
    {
        Debug.Log("ChangeSceneTo" + nombre);
        SceneManager.LoadScene(nombre);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}