using UnityEngine;

public class TestBullet : MonoBehaviour
{
    [SerializeField]
    GameObject GameObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject newObject = Instantiate(GameObject, transform.position, Quaternion.identity);
            
            BulletBase bulletBase = newObject.GetComponent<BulletBase>();

            Vector2 direction = new Vector2(-1.0f, 1.0f); 
            bulletBase.setDirectionShoot(direction);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject newObject = Instantiate(GameObject, transform.position, Quaternion.identity);

            BulletBase bulletBase = newObject.GetComponent<BulletBase>();

            Vector2 direction = new Vector2(-1.0f, 0.0f);
            bulletBase.setDirectionShoot(direction);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameObject newObject = Instantiate(GameObject, transform.position, Quaternion.identity);

            BulletBase bulletBase = newObject.GetComponent<BulletBase>();

            Vector2 direction = new Vector2(1.0f, 0.0f);
            bulletBase.setDirectionShoot(direction);
        }
    }
}
