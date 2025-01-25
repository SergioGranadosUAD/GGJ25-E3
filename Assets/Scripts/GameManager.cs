using NUnit.Framework;
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    struct PlayerData
    {
        public GameObject player;
        public string playerName;
        public int Score;
        public int playerID;
    }

    public static GameManager Instance { get; private set; }

    private List<Transform> m_spawnPoint = new List<Transform>();
    private int m_currentSpawnIndex = 0;

    [SerializeField]
    private float m_timeLimit = 180.0f;

    private float m_currentTime = 0.0f;

    public TMP_Text m_textMeshPro;

    List<PlayerData> m_playerList = new List<PlayerData>();

    private bool m_timeActive = true;

    public PlayerInputManager playerInputManager;

    public bool selectionPlayerInputActive = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
        playerInputManager = GetComponent<PlayerInputManager>();
        if (playerInputManager == null) { return; }
    }

    private void OnEnable()
    {
        playerInputManager.enabled = false;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        updateGameTime();
        checkGameOver();
    }

    private void updateGameTime()
    {
        if (m_textMeshPro == null) { return; }
        if (m_timeActive != true) { return; }

        m_currentTime -= Time.deltaTime;
        int time = (int)m_currentTime;
        m_textMeshPro.text = time.ToString();
    }

    public void checkGameOver()
    {
        if (m_currentTime < 0.0f && m_timeActive != false)
        {
            m_timeActive = false;
        }
    }
    public void addNewPlayer(GameObject player)
    {
        for (int i = 0; i <  m_playerList.Count; i++) 
        {
            if (player == m_playerList[i].player)
            {
                return;
            }
        }

        PlayerData newPlayer = new PlayerData();

        newPlayer.player = player;
        newPlayer.playerID = m_playerList.Count + 1;
        newPlayer.Score = 0;

        m_playerList.Add(newPlayer);
    }

    private void OnPlayerJoined(PlayerInput playerInput)
    {
        GameObject playerGameObject = playerInput.gameObject;
        addNewPlayer(playerGameObject);

        setPlayerPosition(playerGameObject);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject obj = GameObject.FindWithTag("SpawnPoint");

        if (obj == null) { return; }

        foreach (Transform child in obj.transform)
        {
            m_spawnPoint.Add(child.gameObject.transform);
        }

        GameObject objTime = GameObject.FindWithTag("Time");

        if (objTime == null) { return; }

        m_textMeshPro = objTime.GetComponentInChildren<TMP_Text>();

        m_currentTime = m_timeLimit;
    }

    public void setPlayerPosition(GameObject playerGameObject)
    {
        Transform spawnPoint = m_spawnPoint[m_currentSpawnIndex];
        playerGameObject.transform.position = spawnPoint.position;

        m_currentSpawnIndex++;

        if (m_currentSpawnIndex > 3)
        {
            m_currentSpawnIndex = 0;
        }
    }

}