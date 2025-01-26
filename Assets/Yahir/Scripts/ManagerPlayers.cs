using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class ManagerPlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject _GameObject;
    [SerializeField]
    private GameObject _GameObject1;
    [SerializeField]
    private GameObject _GameObject2;
    [SerializeField]
    private GameObject _GameObject3;

    [SerializeField]
    public GameObject m_player1;
    [SerializeField]
    private GameObject m_player2;
    [SerializeField]
    private GameObject m_player3;
    [SerializeField]
    private GameObject m_player4;

    [SerializeField]
    private TMP_Text m_playerScore1;
    [SerializeField]
    private TMP_Text m_playerScore2;
    [SerializeField]
    private TMP_Text m_playerScore3;
    [SerializeField]
    private TMP_Text m_playerScore4;

    public static ManagerPlayers Instance { get; private set; }

    private List<TMP_Text> m_listRenderScore = new List<TMP_Text>();

    bool instanceObject = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance.getSizeListPlayers() < 3)
        {
            m_player4.gameObject.SetActive(false);
            m_player3.gameObject.SetActive(false);
        }
        else if (GameManager.Instance.getSizeListPlayers() < 4)
        {
            m_player4.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (instanceObject)
        {
            instancePlayers();
            instanceObject = false;
        }

        m_playerScore1.text = GameManager.Instance.m_playerList[0].Score.ToString();
        m_playerScore2.text = GameManager.Instance.m_playerList[1].Score.ToString();
        if (GameManager.Instance.getSizeListPlayers() == 4)
        {
            m_playerScore3.text = GameManager.Instance.m_playerList[2].Score.ToString();
            m_playerScore4.text = GameManager.Instance.m_playerList[3].Score.ToString();
        }
        else if (GameManager.Instance.getSizeListPlayers() == 3)
        {
            m_playerScore3.text = GameManager.Instance.m_playerList[2].Score.ToString();
        }
    }

    public void AssignDeviceToOtherObject(PlayerInput fromPlayer, GameObject targetObject)
    {
        PlayerInput targetPlayerInput = targetObject.GetComponent<PlayerInput>();
        if (targetPlayerInput != null)
        {
            targetPlayerInput.SwitchCurrentControlScheme(fromPlayer.currentControlScheme);
        }
    }

    private void instancePlayers()
    {
        GameManager.Instance.SetPlayableObject(_GameObject, 0);
        GameManager.Instance.SetPlayableObject(_GameObject1, 1);
        if (GameManager.Instance.getSizeListPlayers() == 4)
        {
            GameManager.Instance.SetPlayableObject(_GameObject2, 2);
            GameManager.Instance.SetPlayableObject(_GameObject3, 3);
        }
        else if (GameManager.Instance.getSizeListPlayers() == 3)
        {
            GameManager.Instance.SetPlayableObject(_GameObject2, 2);
        }
    }
}
