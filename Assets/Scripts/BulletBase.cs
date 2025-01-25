using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField]
    private float m_MaxSpeed;
    [SerializeField]
    private float m_ForceAmount = 10.0f;
    [SerializeField]
    private float m_timeLife = 100.0f;

    [SerializeField]
    float m_resistance = 0.995f;

    public float stopBackwardThreshold = 0.1f;
    private Vector2 m_directon;

    float m_timeActual;

    private Rigidbody2D rb;

    private Vector2 force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.setNewPlayer(gameObject);
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        force = new Vector2(m_directon.x, m_directon.y) * m_ForceAmount;
        rb.AddForce(force);

        if (rb.linearVelocity.magnitude > m_MaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * m_MaxSpeed;
        }

        m_ForceAmount *= m_resistance;
        timeLife();
    }

    public void OnDestroy()
    {
        if (rb != null) { }


    }

    private void timeLife()
    {
        m_timeActual += Time.deltaTime;

        if (m_timeActual >= m_timeLife)
        {
            Destroy(gameObject);
        }
    }

    public void setDirectionShoot(Vector2 directoin)
    {
        m_directon = directoin;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject gameObjectTrigger = collision.gameObject;
        if (gameObject == null)
        {
            return;
        }

        Destroy(gameObject);
    }
}

