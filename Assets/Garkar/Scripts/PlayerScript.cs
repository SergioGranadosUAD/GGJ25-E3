using System.Runtime.CompilerServices;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
  //Components
  private PlayerInput m_playerInput;
    public PlayerInput PlayerInput
  {
    get {
      if (m_playerInput == null)
      {
        m_playerInput = GetComponent<PlayerInput>();
      }
      return m_playerInput;
      }
    set { m_playerInput = value; }
  }

  private Rigidbody2D m_rigidbody2D;
  public Rigidbody2D Rigidbody
  {
    get
    {
      if(m_rigidbody2D == null)
      {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
      }
      return m_rigidbody2D;
    }
    set
    {
      m_rigidbody2D = value;
    }
  }

  [SerializeField]
  private float m_speed = 5.0f;
  [SerializeField]
  private float m_jumpHeight = 10.0f;
  [SerializeField]
  private float m_armDistance = 1.5f;
  [SerializeField]
  private float m_airSpeedMultiplier = 5.0f;
  [SerializeField]
  private GameObject m_projectileGO;

  private Vector2 m_movementDir = Vector2.zero;
  private Vector2 m_aimDir = Vector2.zero;
  private bool m_hasDoubleJumped = false;
  private bool m_isGrounded = false;
  private bool m_isTrapped = false;
  private GameObject m_armGO;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    m_armGO = transform.GetChild(0).gameObject;
    m_armGO.transform.position = transform.right * m_armDistance;
  }

  // Update is called once per frame
  void Update()
  {
    
    
  }

  private void OnMovement(InputValue value)
  {
    if(!m_isTrapped)
    {
      if(m_isGrounded)
      {
        Rigidbody.linearVelocityX = value.Get<Vector2>().x * m_speed;
        
      }
      else
      {
        Rigidbody.AddForceX(value.Get<Vector2>().x * m_speed);
      }
      //Clamp to max speed in X axis.
      Rigidbody.linearVelocityX = Mathf.Clamp(Rigidbody.linearVelocityX, -m_speed, m_speed);
    }
  }

  private void OnAim(InputValue value)
  {
    if (value.Get<Vector2>().magnitude >= 0.5f)
    {
      m_aimDir = value.Get<Vector2>().normalized;
    }
    m_armGO.transform.position = new Vector2(transform.position.x, transform.position.y) + m_aimDir * m_armDistance;
  }

  private void OnJump(InputValue value)
  {
    if(m_isGrounded)
    {
      Rigidbody.AddForceY(m_jumpHeight);
      m_isGrounded = false;
    }
    else if(!m_hasDoubleJumped)
    {
      Rigidbody.linearVelocityY = 0;
      Rigidbody.AddForceY(m_jumpHeight);
      m_hasDoubleJumped = true;
    }
  }

  private void OnShoot(InputValue value)
  {
    GameObject projectile = Instantiate(m_projectileGO, m_armGO.transform.position, Quaternion.identity) as GameObject;
    if (projectile != null)
    {
      BulletBase bulletComp = projectile.GetComponent<BulletBase>();
      bulletComp.setProjectileDirection(m_aimDir);
      //Register to projectile manager
    }
  }

  private void OnSpecial(InputValue value)
  {

  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    m_isGrounded = true;
    m_hasDoubleJumped = false;
    Rigidbody.linearVelocityX = 0;
  }
}
