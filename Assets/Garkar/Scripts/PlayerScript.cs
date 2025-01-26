using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
  //Components
  private PlayerInput m_playerInput;
  public PlayerInput playerInput
  {
    get
    {
      if (m_playerInput == null)
      {
        m_playerInput = GetComponent<PlayerInput>();
      }
      return m_playerInput;
    }
  }

  private Rigidbody2D m_rigidbody2D;
  public Rigidbody2D rigidbody
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
  public BoxCollider2D boxCollider
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

  private Animator m_animator;
  public Animator animator
  {
    get
    {
      if (m_animator == null)
      {
        m_animator = GetComponent<Animator>();
      }
      return m_animator;
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
  private float m_trappedBounceMultiplier = 0.7f;

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

  public PlayerInput m_currentPlayerInput;

  InputAction m_moveAction;
  InputAction m_aimAction;
  InputAction m_jumpAction;
  InputAction m_shootAtion;
  InputAction m_especialAction;
  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    m_armGO = transform.GetChild(0).gameObject;
    m_armGO.transform.position = transform.position + transform.right * m_armDistance;
    m_currentResistance = m_maxResistance;
    m_currentProjectileGO = m_defaultProjectileGO;
    if (playerInput)
    {
      SetPlayernput(playerInput);
    }

  }

  public void SetPlayernput(PlayerInput pi)
  {
    m_playerInput = pi;

    m_moveAction = m_playerInput.actions.FindAction("Movement");
    m_aimAction = m_playerInput.actions.FindAction("Aim");
    m_jumpAction = m_playerInput.actions.FindAction("Jump");
    m_shootAtion = m_playerInput.actions.FindAction("Shoot");
    m_especialAction = m_playerInput.actions.FindAction("Special");
    //m_playerInput.actions.FindAction("Movement").canceled += OnMove;
    //m_playerInput.actions.FindAction("Aim").performed += OnAimAct;
    //m_playerInput.actions.FindAction("Aim").canceled += OnAimAct;
    //m_playerInput.actions.FindAction("Jump").performed += OnJumpAct;
    //m_playerInput.actions.FindAction("Shoot").performed += OnShootAct;
    //m_playerInput.actions.FindAction("Shoot").canceled += OnShootAct;
    //m_playerInput.actions.FindAction("Special").canceled += OnSpecialAct;
  }

  // Update is called once per frame
  void Update()
  {
    m_currentVelocity = rigidbody.linearVelocity;
    checkIfCanShoot();
    checkIfCanRegen();
    checkGrounded();
    OnMovement(m_moveAction.ReadValue<Vector2>());
    OnAim(m_aimAction.ReadValue<Vector2>());
    if (m_jumpAction.WasPerformedThisFrame())
    {
      OnJump();
    }
    m_isShooting = m_shootAtion.IsPressed();
  }
  private void OnMove(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      OnMovement(context.ReadValue<Vector2>());
    }
  }

  private void OnMovement(Vector2 value)
  {
    Vector2 inputDir = value.normalized;

    if(inputDir.magnitude > 0.0f)
    {
      animator.SetBool("isMoving", true);
    }
    else
    {
      animator.SetBool("isMoving", false);
    }

    if (!m_isTrapped)
    {
      if (m_isGrounded)
      {
        rigidbody.linearVelocityX = inputDir.x * m_speed;

      }
      else
      {
        rigidbody.AddForceX(inputDir.x * m_speed * m_airSpeedMultiplier);
      }
      //Clamp to max speed in X axis.
      rigidbody.linearVelocityX = Mathf.Clamp(rigidbody.linearVelocityX, -m_speed, m_speed);
    }
    else
    {
      rigidbody.AddForce(inputDir * m_trappedSpeed);
      //if(Rigidbody.linearVelocity.magnitude > m_trappedSpeed)
      //{
      //  Rigidbody.linearVelocity = Rigidbody.linearVelocity.normalized * m_trappedSpeed;
      //}
    }
  }

  private void OnAimAct(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      OnAim(context.ReadValue<Vector2>());
    }
  }

  private void OnAim(Vector2 value)
  {
    if (value.magnitude >= 0.5f)
    {
      m_aimDir = value.normalized;
    }
    m_armGO.transform.position = new Vector2(transform.position.x, transform.position.y) + m_aimDir * m_armDistance;
  }

  private void OnJumpAct(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      OnJump();
    }
  }

  private void OnJump()
  {
    if (m_isTrapped)
    {
      m_currentTrappedTimer += m_trappedTimeReductionValue;
      return;
    }

    if (m_isGrounded)
    {
      rigidbody.AddForceY(m_jumpHeight);
    }
    else if (!m_hasDoubleJumped)
    {
      rigidbody.linearVelocityY = 0;
      rigidbody.AddForceY(m_jumpHeight);
      m_hasDoubleJumped = true;
    }
  }

  private void OnShootAct(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      OnShoot();
    }
  }
  private void OnShoot()
  {
    m_isShooting = true;
    //if (value > 0.5f)
    //{
    //}
    //else
    //{
    //  m_isShooting = false;
    //}
  }

  private void OnSpecialAct(InputAction.CallbackContext context)
  {
    if (context.performed)
    {
      //OnSpecial(context.ReadValue<float>());
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
    if (!m_isTrapped && m_canBeTrapped)
    {
      m_currentResistance -= damageAmount;
      if (m_currentResistance <= 0)
      {
        trapPlayer();
      }
    }
    rigidbody.linearVelocity = dir * pushForce;
    m_currentRegenTime = 0.0f;
  }

  private void trapPlayer()
  {
    animator.SetBool("isTrapped", true);
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
      bulletComp.OwningPlayerID = playerInput.playerIndex;

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
    if (m_isTrapped)
    {
      return;
    }

    LayerMask layerMask = LayerMask.GetMask("Walls");

    Vector2[] points = new Vector2[3] { new Vector2(boxCollider.bounds.min.x, boxCollider.bounds.min.y),
                                        new Vector2(boxCollider.bounds.max.x, boxCollider.bounds.min.y),
                                        new Vector2(boxCollider.bounds.center.x, boxCollider.bounds.min.y)
                                        };

    //
    for (int i = 0; i < points.Length; ++i)
    {
      if (Physics2D.Raycast(points[i], Vector3.down, .1f, layerMask))
      {
        Debug.DrawRay(points[i], Vector3.down * .1f, Color.green);
        m_isGrounded = true;
        animator.SetBool("isJumping", m_isGrounded);
        m_hasDoubleJumped = false;
        return;
      }
      else
      {
        Debug.DrawRay(points[i], Vector3.down * .1f, Color.red);
      }
    }

    float maxWalljumpHeight = boxCollider.bounds.max.y - boxCollider.size.y * .75f;
    Vector2 leftWalljumpPoint = new Vector2(boxCollider.bounds.min.x, maxWalljumpHeight);
    if (Physics2D.Raycast(leftWalljumpPoint, Vector3.left, .1f, layerMask))
    {
      Debug.DrawRay(leftWalljumpPoint, Vector3.left * .1f, Color.green);
      m_isGrounded = true;
      animator.SetBool("isJumping", m_isGrounded);
      m_hasDoubleJumped = false;
      return;
    }
    else
    {
      Debug.DrawRay(leftWalljumpPoint, Vector3.left * .1f, Color.red);
    }

    Vector2 rightWalljumpPoint = new Vector2(boxCollider.bounds.max.x, maxWalljumpHeight);
    if (Physics2D.Raycast(rightWalljumpPoint, Vector3.right, .1f, layerMask))
    {
      Debug.DrawRay(rightWalljumpPoint, Vector3.right * .1f, Color.green);
      m_isGrounded = true;
      animator.SetBool("isJumping", m_isGrounded);
      m_hasDoubleJumped = false;
      return;
    }
    else
    {
      Debug.DrawRay(rightWalljumpPoint, Vector3.right * .1f, Color.red);
    }

    //No ground found
    m_isGrounded = false;
    animator.SetBool("isJumping", m_isGrounded);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    if (collision.transform.CompareTag("Walls"))
    {
      if (!m_isTrapped)
      {
        rigidbody.linearVelocityX = 0;
      }
      else
      {
        rigidbody.linearVelocity = Vector2.Reflect(m_currentVelocity * m_trappedBounceMultiplier, collision.contacts[0].normal);
      }
    }
    else if (collision.transform.CompareTag("Player"))
    {
      PlayerScript enemyPlayer = collision.gameObject.gameObject.GetComponent<PlayerScript>();
      if (enemyPlayer != null)
      {
        if (enemyPlayer.IsTrapped)
        {
          enemyPlayer.rigidbody.AddForce(rigidbody.linearVelocity.normalized * m_pushForce);
        }
      }
    }
  }

  private IEnumerator startShotCooldownTimer()
  {
    m_canShoot = false;
    float currentTime = 0;
    while (currentTime < m_shotCooldown)
    {
      currentTime += Time.deltaTime;
      yield return null;
    }
    m_canShoot = true;
  }

  private IEnumerator startTrappedTimer()
  {
    rigidbody.sharedMaterial.bounciness = 1.0f;
    rigidbody.gravityScale = m_trappedGravityScale;
    m_isTrapped = true;
    m_currentTrappedTimer = 0;
    while (m_currentTrappedTimer < m_trappedTime)
    {
      m_currentTrappedTimer += Time.deltaTime;
      yield return null;
    }
    rigidbody.sharedMaterial.bounciness = 0.0f;
    rigidbody.gravityScale = 1.0f;
    m_isTrapped = false;
    m_currentResistance = m_maxResistance;
    animator.SetBool("isTrapped", false);

    m_canBeTrapped = false;
    m_currentTrappedTimer = 0;
    while (m_currentTrappedTimer < m_trappedIFrameTime)
    {
      m_currentTrappedTimer += Time.deltaTime;
      yield return null;
    }
    m_canBeTrapped = true;
  }

}
