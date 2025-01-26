using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void ChangeToScene(string nombre)
    {
        SceneManager.LoadScene(nombre);
    }

    public void LoadSceneByName()
    {
        int rand = Random.Range(1, 5);

        switch (rand)
        {
            case 1:
                SceneManager.LoadScene("Level 1_Layout");
                break;
            case 2:
                SceneManager.LoadScene("Level 2_Layout");
                break;
            case 3:
                SceneManager.LoadScene("Level 3_Layout");
                break;
            case 4:
                SceneManager.LoadScene("Level 4_Layout");
                break;
            default:
                Debug.LogWarning("Número inesperado (esto no debería ocurrir).");
                break;
        }
    }

    public void exitGame()
    {
        Application.Quit();
    }
}