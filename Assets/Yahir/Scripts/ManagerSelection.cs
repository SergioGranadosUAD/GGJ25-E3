using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ManagerSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject m_dataPlayersBackground;

    private List<GameObject> m_listImageBackGround = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.playerInputManager.enabled = true;
        GameManager.Instance.playerInputManager.EnableJoining();

        if (m_dataPlayersBackground == null) { return; }

        //foreach (GameObject child in m_dataPlayersBackground)
        //{
        //    m_listImageBackGround.Add(child.gameObject);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void newPlayer()
    {

    }
}
