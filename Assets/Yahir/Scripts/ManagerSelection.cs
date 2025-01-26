using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;

public class ManagerSelection : MonoBehaviour
{
    [SerializeField]
    AudioClip m_audio;
    [SerializeField]
    private List<Image> m_listImageBackGround = new List<Image>();
    [SerializeField]
    private List<Image> m_imagePlayer = new List<Image>();
    [SerializeField]
    private List<TMP_Text> m_namePlayer = new List<TMP_Text>();

    bool m_player1 = false;
    bool m_player2 = false;
    bool m_player3 = false;
    bool m_player4 = false;

    public Button myButton; // Arrastra tu botón desde el editor a este campo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.playerInputManager.enabled = true;
        GameManager.Instance.playerInputManager.EnableJoining();

        if (myButton != null)
        {
            myButton.interactable = false; // Esto desactiva el botón
        }
        AudioManager.Instance.PlayMusic(m_audio);
    }

    // Update is called once per frame
    void Update()
    {
        newPlayer();
    }

    private void newPlayer()
    {
        if (GameManager.Instance.getSizeListPlayers() > 0)
        {
            if (m_player1 == false && GameManager.Instance.getSizeListPlayers() == 1)
            {
                m_player1 = true;

                Image imageComponent = m_listImageBackGround[0];

                Color color = new Color(0.886f, 0.317f, 0.529f, 0.5f);

                imageComponent.color = color;
                //setAlphaImage(imageComponent);

                activeImagePlayer(0);

            }
            else if (m_player2 == false && GameManager.Instance.getSizeListPlayers() == 2)
            {
                m_player2 = true;

                Image imageComponent = m_listImageBackGround[1];

                Color color = new Color(0.278f, 0.647f, 0.843f, 0.5f);

                imageComponent.color = color;
                //setAlphaImage(imageComponent);

                activeImagePlayer(1);
                myButton.interactable = true;
            }
            else if (m_player3 == false && GameManager.Instance.getSizeListPlayers() == 3)
            {
                m_player3 = true;

                Image imageComponent = m_listImageBackGround[2];

                Color color = new Color(0.725f, 0.945f, 0.561f, 0.5f);

                imageComponent.color = color;
                //setAlphaImage(imageComponent);

                activeImagePlayer(2);
            }
            else if (m_player4 == false && GameManager.Instance.getSizeListPlayers() == 4)
            {
                m_player4 = true;

                Image imageComponent = m_listImageBackGround[3];

                Color color = new Color(0.698f, 0.133f, 0.647f, 0.5f);

                imageComponent.color = color;
                //setAlphaImage(imageComponent);

                activeImagePlayer(3);
            }
        }
    }

    private void setAlphaImage(Image imageComponent)
    {
        if (imageComponent != null)
        {
            // Obtén el color actual
            Color currentColor = imageComponent.color;

            // Cambia el valor del canal alpha
            currentColor.a = Mathf.Clamp01(0.5f); // Asegúrate de que esté entre 0 y 1

            // Asigna el nuevo color al componente Image
            imageComponent.color = currentColor;
        }
    }

    private void activeImagePlayer(int i)
    {
        if (!m_imagePlayer[i].gameObject.activeSelf)
        {
            m_imagePlayer[i].gameObject.SetActive(true);
        }
        if (!m_namePlayer[i].gameObject.activeSelf)
        {
            m_namePlayer[i].gameObject.SetActive(true);
        }
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
}
