using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.Controls;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    AudioClip m_audio;
    [SerializeField]
    private List<GameObject> m_listImageBackGround = new List<GameObject>();

    [SerializeField]
    private TMP_Text m_text1;
    [SerializeField]
    private TMP_Text m_text2;
    [SerializeField]
    private TMP_Text m_text3;
    [SerializeField]
    private TMP_Text m_text4;

    public Button myButton; // Arrastra tu botón desde el editor a este campo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayMusic(m_audio);

        if (GameManager.Instance.getSizeListPlayers() == 4)
        {
            m_listImageBackGround[0].SetActive(true);
            m_listImageBackGround[1].SetActive(true);
            m_listImageBackGround[2].SetActive(false);
            m_listImageBackGround[3].SetActive(false);
        }
        else if (GameManager.Instance.getSizeListPlayers() == 3)
        {
            m_listImageBackGround[0].SetActive(true);
            m_listImageBackGround[2].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        scoreUpdate();
    }

    private void scoreUpdate()
    {
        m_text1.text = GameManager.Instance.m_playerList[0].Score.ToString();
        m_text2.text = GameManager.Instance.m_playerList[1].Score.ToString();
        if (GameManager.Instance.getSizeListPlayers() == 4)
        {
            m_text3.text = GameManager.Instance.m_playerList[2].Score.ToString();
            m_text4.text = GameManager.Instance.m_playerList[3].Score.ToString();
        }
        else if (GameManager.Instance.getSizeListPlayers() == 3)
        {
            m_text3.text = GameManager.Instance.m_playerList[2].Score.ToString();
        }
    }
}
