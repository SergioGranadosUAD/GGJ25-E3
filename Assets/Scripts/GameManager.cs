using NUnit.Framework;
using UnityEngine;

using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    struct PlayerData
    {
        public GameObject player;
        public int Score;
        public int IDPlayer;
    }

    List<PlayerData> m_playerList = new List<PlayerData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
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
        newPlayer.Score = m_playerList.Count + 1;
        newPlayer.Score = 0;

        m_playerList.Add(newPlayer);
    }

}