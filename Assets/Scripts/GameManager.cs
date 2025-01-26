using NUnit.Framework;
using UnityEngine;

using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public struct PlayerInputData
{
    public string defaultControlScheme;
    public string defaultActionMap;
    public InputActionAsset actions;

    public PlayerInputData(PlayerInput playerInput)
    {
        defaultControlScheme = playerInput.defaultControlScheme;
        defaultActionMap = playerInput.defaultActionMap;
        actions = playerInput.actions;
    }
}

    public class PlayerData
    {
        public GameObject player;
        public string playerName;
        public int Score = 0;
        public int playerID;
    }

public class GameManager : MonoBehaviour
{
    [SerializeField]
    AudioClip m_audio;

    public PlayerInput originalPlayerInput;
    public PlayerInput newPlayerInput;
    public List<PlayerInputData> data = new List<PlayerInputData>();
    //public void saveData()
    //{
    //    PlayerInputData playerControl;
    //    // Disable the original PlayerInput
    //    originalPlayerInput.enabled = false;

    //    // Transfer input actions
    //    newPlayerInput.actions = originalPlayerInput.actions;

    //    // Transfer control scheme and devices
    //    newPlayerInput.defaultControlScheme = originalPlayerInput.defaultControlScheme;
    //    newPlayerInput.defaultActionMap = originalPlayerInput.defaultActionMap;
    //    newPlayerInput.SwitchCurrentControlScheme(originalPlayerInput.defaultControlScheme);

    //    // Enable the new PlayerInput
    //    newPlayerInput.enabled = true;
    //}

    public static GameManager Instance { get; private set; }

    private List<Transform> m_spawnPoint = new List<Transform>();
    private int m_currentSpawnIndex = 0;

    [SerializeField]
    private float m_timeLimit = 180.0f;

    private float m_currentTime = 0.0f;

    public TMP_Text m_textMeshPro;

    public List<PlayerData> m_playerList = new List<PlayerData>();

    private bool m_timeActive = true;

    public PlayerInputManager playerInputManager;

    public bool selectionPlayerInputActive = false;

    public List<PlayerInput> m_playersInstance = new List<PlayerInput>();

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

    private void Start()
    {
        AudioManager.Instance.PlayMusic(m_audio);
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += OnPlayerJoined;
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
        m_playersInstance.Add(playerInput);
        PlayerInputData playerControl = new PlayerInputData(playerInput);
        GameObject playerGameObject = playerInput.gameObject;
        playerGameObject.transform.parent = transform;
        addNewPlayer(playerGameObject);
        //setPlayerPosition(playerGameObject);
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

    public int getSizeListPlayers()
    {
        return m_playerList.Count;
    }

    public void SetPlayableObject(GameObject newPlayerPrefab, int i)
    {
        GameObject newPlayer = Instantiate(newPlayerPrefab);
        newPlayer.GetComponent<PlayerScript>().SetPlayernput (m_playersInstance[i]);
        //newPlayer.transform.position = m_playersInstance[i].transform.position;

        // Asignar el PlayerInput al nuevo jugador
        //newPlayer.transform;
        //m_playersInstance[i].transform.SetParent(newPlayer.transform);
        setPlayerPosition(newPlayer);

        // Otras configuraciones si es necesario
    }

    public void addPoints(int playerID)
    {
        for (int i = 0; i < m_playerList.Count; i++)
        {
            if (m_playerList[i].playerID == playerID)
            {
                m_playerList[i].Score += 1;
            }
        }
    }

    public void substractPoints(int playerID)
    {
        for (int i = 0; i < m_playerList.Count; i++)
        {
            if (m_playerList[i].playerID == playerID)
            {
                m_playerList[i].Score -= 1;
            }
        }
    }
}