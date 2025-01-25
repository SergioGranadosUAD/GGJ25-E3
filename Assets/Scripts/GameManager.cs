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
        public int Score;
        public int playerID;
    }

    public static GameManager Instance { get; private set; }

    private List<Transform> m_spawnPoint = new List<Transform>();
    private int m_currentSpawnIndex = 0;

    [SerializeField]
    public float m_gameTime = 180.0f;

    [SerializeField]
    public TMP_Text m_textMeshPro;

    List<PlayerData> m_playerList = new List<PlayerData>();

    private bool m_timeActive = true;

    PlayerInputManager playerInputManager;

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
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
        playerInputManager.onPlayerLeft += OnPlayerLeft;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
        playerInputManager.onPlayerLeft -= OnPlayerLeft;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        gameTime();
        gameOver();
    }

    private void gameTime()
    {
        if (m_timeActive != true)
        {
            return;
        }
        m_gameTime -= Time.deltaTime;
        int time = (int)m_gameTime;
        m_textMeshPro.text = time.ToString();
    }

    public void gameOver()
    {
        if (m_gameTime < 0.0f && m_timeActive != false)
        {
            m_timeActive = false;
        }
    }
    public void setNewPlayer(GameObject player)
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
        setNewPlayer(playerGameObject);

        setPositionPalyer(playerGameObject);
    }

    private void OnPlayerLeft(PlayerInput playerInput)
    {

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject obj = GameObject.FindWithTag("SpawnPoint");

        foreach (Transform child in obj.transform)
        {
            m_spawnPoint.Add(child.gameObject.transform);
        }
        m_gameTime = 180.0f;
    }

    public void setPositionPalyer(GameObject playerGameObject)
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