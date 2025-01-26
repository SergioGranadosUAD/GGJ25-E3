using UnityEngine;

public class TestFall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            GameObject gameObject = collision.gameObject;
            PlayerScript playerScript = gameObject.GetComponent<PlayerScript>();
            if (playerScript.LastPlayerID >= 0 )
            {
                GameManager.Instance.addPoints(playerScript.LastPlayerID + 1);
            }
            GameManager.Instance.setPlayerPosition(gameObject);
        }
    }
}
