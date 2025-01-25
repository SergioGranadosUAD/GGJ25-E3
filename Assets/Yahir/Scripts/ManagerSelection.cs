using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class ManagerSelection : MonoBehaviour
{
    [SerializeField]
    private List<Image> m_listImageBackGround = new List<Image>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GameManager.Instance.playerInputManager.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
