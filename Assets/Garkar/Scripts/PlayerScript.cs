using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
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
  }

  private Rigidbody2D m_rigidbody2D;
  public Rigidbody2D Rigidbody
  {
    get
    {
      if (m_rigidbody2D == null)
      {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
      }
      return m_rigidbody2D;
    }
  }

  private BoxCollider2D m_boxCollider;
  public BoxCollider2D BoxCollider
  {
    get
    {
      if (m_boxCollider == null)
      {
        m_boxCollider = GetComponent<BoxCollider2D>();
      }
      return m_boxCollider;
    }
  }

  public bool IsTrapped 
  {
    get
    {
      return m_isTrapped;
    } 
  }

  [SerializeField]
  private float m_speed = 5.0f;
  [SerializeField]
  private float m_trappedSpeed = 1.0f;
  [SerializeField]
  private float m_jumpHeight = 10.0f;
  [SerializeField]
  private float m_armDistance = 1.5f;
  [SerializeField]
  private float m_airSpeedMultiplier = 5.0f;
  [SerializeField]
  private float m_maxResistance = 100.0f;
  [SerializeField]
  private float m_shotCooldown = 1.0f;
  [SerializeField]
  private float m_trappedTime = 5.0f;
  [SerializeField]
  private float m_trappedGravityScale = 0.01f;
  [SerializeField]
  private float m_resistanceRegenTime = 5.0f;
  [SerializeField]
  private float m_trappedIFrameTime = 2.0f;
  [SerializeField]
  private float m_timeToStartRegenWhenShot = 1.0f;
  [SerializeField]
  private float m_pushForce = 10.0f;
  [SerializeField]
  private float m_trappedTimeReductionValue = 0.1f;
  
  [SerializeField]
  private GameObject m_defaultProjectileGO;

  private float m_currentRegenTime = 0.0f;
  private float m_currentResistance = 0.0f;
  private float m_remainingPowerupDuration = 0.0f;
  private float m_currentTrappedTimer = 0.0f;
  private Vector2 m_movementDir = Vector2.zero;
  private Vector2 m_aimDir = Vector2.zero;
  private Vector2 m_currentVelocity = Vector2.zero;
  private bool m_hasDoubleJumped = false;
  private bool m_isGrounded = false;
  private bool m_isTrapped = false;
  private bool m_canShoot = true;
  private bool m_canBeTrapped = true;
  private bool m_isShooting = false;
  private GameObject m_armGO;
  private PowerupType m_currentPowerup = PowerupType.None;
  private GameObject m_currentProjectileGO;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    m_armGO = transform.GetChild(0).gameObject;
    m_armGO.transform.position = transform.position + transform.right * m_armDistance;
    m_currentResistance = m_maxResistance;
    m_currentProjectileGO = m_defaultProjectileGO;
  }

  // Update is called once per frame
  void Update()
  {
    m_currentVelocity = Rigidbody.linearVelocity;
    checkIfCanShoot();
    checkIfCanRegen();
    checkGrounded();
  }

  private void OnMovement(InputValue value)
  {
    Vector2 inputDir = value.Get<Vector2>();
    if (!m_isTrapped)
    {
      if(m_isGrounded)
      {
        Rigidbody.linearVelocityX = inputDir.x * m_speed;
        
      }
      else
      {
        Rigidbody.AddForceX(inputDir.x * m_speed * m_airSpeedMultiplier);
      }
      //Clamp to max speed in X axis.
      Rigidbody.linearVelocityX = Mathf.Clamp(Rigidbody.linearVelocityX, -m_speed, m_speed);
    }
    else
    {
      Rigidbody.AddForce(inputDir * m_trappedSpeed);
      //if(Rigidbody.linearVelocity.magnitude > m_trappedSpeed)
      //{
      //  Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * m_trappedSpeed;
      //}
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
    if(m_isTrapped)
    {
      m_currentTrappedTimer += m_trappedTimeReductionValue;
      return;
    }

    if(m_isGrounded)
    {
      Rigidbody.AddForceY(m_jumpHeight);
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
    if (value.Get<float>() > 0.5f)
    {
      m_isShooting = true;
    }
    else
    {
      m_isShooting = false;
    }
  }

  private void OnSpecial(InputValue value)
  {

  }

  public void equipPowerup(Powerup powerup)
  {
    m_currentPowerup = powerup.type;
    m_currentProjectileGO = powerup.prefab;
    m_remainingPowerupDuration = powerup.duration;
  }

  public void damagePlayer(Vector2 dir, float damageAmount, float pushForce)
  {
    if(!m_isTrapped && m_canBeTrapped)
    {
      m_currentResistance -= damageAmount;
      if (m_currentResistance <= 0)
      {
        trapPlayer();
      }
    }
    Rigidbody.linearVelocity = dir * pushForce;
    m_currentRegenTime = 0.0f;
  }

  private void trapPlayer()
  {
    StartCoroutine(startTrappedTimer());
  }

  private void respawnPlayer()
  {

  }

  private void checkIfCanShoot()
  {
    if (m_isTrapped || !m_canShoot || !m_isShooting)
    {
      return;
    }

    GameObject projectile = Instantiate(m_currentProjectileGO, m_armGO.transform.position, Quaternion.identity) as GameObject;
    if (projectile != null)
    {
      BulletBase bulletComp = projectile.GetComponent<BulletBase>();
      bulletComp.Direction = m_aimDir;
      bulletComp.OwningPlayerID = PlayerInput.playerIndex;

      StartCoroutine(startShotCooldownTimer());
    }
  }

  private void checkIfCanRegen()
  {
    if (m_currentResistance < m_maxResistance && !m_isTrapped)
    {
      m_currentRegenTime += Time.deltaTime;
      if (m_currentRegenTime > m_timeToStartRegenWhenShot)
      {
        float step = m_maxResistance / m_resistanceRegenTime;
        m_currentResistance += step * Time.deltaTime;
        m_currentResistance = Mathf.Clamp(m_currentResistance, 0, m_maxResistance);
      }
    }
  }
  private void checkGrounded()
  {
    if(m_isTrapped)
    {
      return;
    }

    LayerMask layerMask = LayerMask.GetMask("Walls");

    Vector2[] points = new Vector2[3] { new Vector2(BoxCollider.bounds.min.x, BoxCollider.bounds.min.y),
                                        new Vector2(BoxCollider.bounds.max.x, BoxCollider.bounds.min.y),
                                        new Vector2(BoxCollider.bounds.center.x, BoxCollider.bounds.min.y)
                                        };

    //
    for(int i = 0; i < points.Length; ++i)
    {
      if (Physics2D.Raycast(points[i], Vector3.down, .1f, layerMask)) {
        Debug.DrawRay(points[i], Vector3.down * .1f, Color.green);
        m_isGrounded = true;
        m_hasDoubleJumped = false;
        return;
      }
      else
      {
        Debug.DrawRay(points[i], Vector3.down * .1f, Color.red);
      }
    }

    float maxWalljumpHeight = BoxCollider.bounds.max.y - BoxCollider.size.y * .75f;
    Vector2 leftWalljumpPoint = new Vector2(BoxCollider.bounds.min.x, maxWalljumpHeight);
    if (Physics2D.Raycast(leftWalljumpPoint, Vector3.left, .1f, layerMask))
    {
      Debug.DrawRay(leftWalljumpPoint, Vector3.left * .1f, Color.green);
      m_isGrounded = true;
      m_hasDoubleJumped = false;
      return;
    }
    else
    {
      Debug.DrawRay(leftWalljumpPoint, Vector3.left * .1f, Color.red);
    }

    Vector2 rightWalljumpPoint = new Vector2(BoxCollider.bounds.max.x, maxWalljumpHeight);
    if (Physics2D.Raycast(rightWalljumpPoint, Vector3.right, .1f, layerMask))
    {
      Debug.DrawRay(rightWalljumpPoint, Vector3.right * .1f, Color.green);
      m_isGrounded = true;
      m_hasDoubleJumped = false;
      return;
    }
    else
    {
      Debug.DrawRay(rightWalljumpPoint, Vector3.right * .1f, Color.red);
    }

    //No ground found
    m_isGrounded = false;
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if(collision.transform.CompareTag("Walls"))
    {
      if(!m_isTrapped)
      {
        Rigidbody.linearVelocityX = 0;
      }
      else
      {
        Rigidbody.linearVelocity = Vector2.Reflect(m_currentVelocity, collision.contacts[0].normal);
      }
    }
    else if(collision.transform.CompareTag("Player"))
    {
      PlayerScript enemyPlayer = collision.gameObject.gameObject.GetComponent<PlayerScript>();
      if (enemyPlayer != null)
      {
        if(enemyPlayer.IsTrapped)
        {
          enemyPlayer.Rigidbody.AddForce(Rigidbody.linearVelocity.normalized * m_pushForce);
        }
      }
    }
  }

  private IEnumerator startShotCooldownTimer()
  {
    m_canShoot = false;
    float currentTime = 0;
    while(currentTime < m_shotCooldown)
    {
      currentTime += Time.deltaTime;
      yield return null;
    }
    m_canShoot = true;
  }

  private IEnumerator startTrappedTimer()
  {
    Rigidbody.sharedMaterial.bounciness = 1.0f;
    Rigidbody.gravityScale = m_trappedGravityScale;
    m_isTrapped = true;
    m_currentTrappedTimer = 0;
    while (m_currentTrappedTimer < m_trappedTime)
    {
      m_currentTrappedTimer += Time.deltaTime;
      yield return null;
    }
    Rigidbody.sharedMaterial.bounciness = 0.0f;
    Rigidbody.gravityScale = 1.0f;
    m_isTrapped = false;
    m_currentResistance = m_maxResistance;

    m_canBeTrapped = false;
    m_currentTrappedTimer = 0;
    while(m_currentTrappedTimer < m_trappedIFrameTime)
    {
      m_currentTrappedTimer += Time.deltaTime;
      yield return null;
    }
    m_canBeTrapped = true;
  }
  
}
