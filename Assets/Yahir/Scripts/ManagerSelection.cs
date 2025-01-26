using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;

public class ManagerSelection : MonoBehaviour
{
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


    int m_index = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.playerInputManager.enabled = true;
        GameManager.Instance.playerInputManager.EnableJoining();
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

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
