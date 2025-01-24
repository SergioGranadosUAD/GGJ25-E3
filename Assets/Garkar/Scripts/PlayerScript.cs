using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerScript : MonoBehaviour
{
  //Components
  private PlayerInput m_playerInput
  {
    get {
      if (m_playerInput = null)
      {
        m_playerInput = GetComponent<PlayerInput>();
      }
      return m_playerInput;
      }
    set { m_playerInput = value; }
  }

  private Rigidbody2D m_rigidbody2D
  {
    get
    {
      if(m_rigidbody2D = null)
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

  Vector2 m_movementDir = Vector2.zero;
  Vector2 m_aimDir = Vector2.zero;
  bool m_hasDoubleJumped = false;
  bool m_canJump = false;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
      
  }

  // Update is called once per frame
  void Update()
  {
      m_rigidbody2D.AddForce(m_movementDir * m_speed * Time.deltaTime);
  }

  private void OnMovement(InputValue value)
  {
    m_movementDir = value.Get<Vector2>();
  }

  private void OnAim(InputValue value)
  {
    m_aimDir = value.Get<Vector2>();
  }

  private void OnJump(InputValue value)
  {
    //Jump
  }

  private void OnShoot(InputValue value)
  {

  }

  private void OnSpecial(InputValue value)
  {

  }
}
