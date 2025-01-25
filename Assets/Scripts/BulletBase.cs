using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField]
    private float m_maxSpeed;
    [SerializeField]
    private float m_forceAmount = 10.0f;
    [SerializeField]
    private float m_projectileLifetime = 100.0f;

    [SerializeField]
    float m_resistance = 0.995f;

    public float m_stopBackwardThreshold = 0.1f;
    private Vector2 m_direction;

    private float m_actualTime;

    private Rigidbody2D m_rigidBody;

    private Vector2 m_force;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_rigidBody.linearDamping = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        m_force = m_direction * m_forceAmount;
        m_rigidBody.AddForce(m_force);

        if (m_rigidBody.linearVelocity.magnitude > m_maxSpeed)
        {
            m_rigidBody.linearVelocity = m_rigidBody.linearVelocity.normalized * m_maxSpeed;
        }

        m_forceAmount *= m_resistance;
        checkProjectileLifetime();
    }

    public void OnDestroy()
    {
        if (m_rigidBody != null) { }


    }

  private void checkProjectileLifetime()
  {
    m_actualTime += Time.deltaTime;

    if (m_actualTime >= m_projectileLifetime)
    {
      onProjectileHit();
    }
  }

  public void setProjectileDirection(Vector2 dir)
  {
    m_direction = dir;
  }

  private void onProjectileHit()
  {
    Destroy(gameObject);
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    GameObject gameObjectTrigger = collision.gameObject;
    if (gameObject == null)
    {
        return;
    }

  onProjectileHit();
  }
}
