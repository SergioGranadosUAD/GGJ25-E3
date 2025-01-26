using UnityEngine;
using UnityEngine.InputSystem;

public class ManagerPlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject _GameObject;

    public static ManagerPlayers Instance { get; private set; }

    bool instanceObject = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        for (int i = 0; i < GameManager.Instance.getSizeListPlayers(); i++)
        {
            GameManager.Instance.SetPlayableObject(_GameObject, i);
        }
    }
}
