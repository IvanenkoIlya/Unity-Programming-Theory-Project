using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class PrototypeHeroDemo : MonoBehaviour
{

   [Header("Variables")]
   [SerializeField] float m_maxSpeed = 4.5f;
   [SerializeField] float m_sprintMultiplier = 2.0f;
   [SerializeField] float m_jumpForce = 7.5f;
   [SerializeField] bool m_hideSword = false;
   [Header("Effects")]
   [SerializeField] GameObject m_RunStopDust;
   [SerializeField] GameObject m_JumpDust;
   [SerializeField] GameObject m_LandingDust;

   private PlayerControls playerControls;
   private InputAction moveInput;
   private InputAction sprintInput;
   
   private Animator m_animator;
   private Rigidbody2D m_body2d;
   private Sensor_Prototype m_groundSensor;
   private AudioSource m_audioSource;
   private AudioManager_PrototypeHero m_audioManager;
   private bool m_grounded = false;
   private bool m_moving = false;
   private int m_facingDirection = 1;
   private float m_disableMovementTimer = 0.0f;
   private float m_currentSprintMultiplier;

   // Use this for initialization
   void Start()
   {
      m_animator = GetComponent<Animator>();
      m_body2d = GetComponent<Rigidbody2D>();
      m_audioSource = GetComponent<AudioSource>();
      m_audioManager = AudioManager_PrototypeHero.instance;
      m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
   }

   private void Awake()
   {
      playerControls = new PlayerControls();
   }

   private void OnEnable()
   {
      moveInput = playerControls.Player.Move;
      moveInput.Enable();

      sprintInput = playerControls.Player.Sprint;
      sprintInput.Enable();

      playerControls.Player.Jump.Enable();
      playerControls.Player.Jump.performed += ctx => OnJump();
   }

   private void OnDisable()
   {
      moveInput.Disable();
      sprintInput.Disable();
      playerControls.Player.Jump.Disable();
   }

   // Update is called once per frame
   void Update()
   {
      // Decrease timer that disables input movement. Used when attacking
      m_disableMovementTimer -= Time.deltaTime;

      //Check if character just landed on the ground
      if (!m_grounded && m_groundSensor.State())
      {
         m_grounded = true;
         m_animator.SetBool("Grounded", m_grounded);
      }

      //Check if character just started falling
      if (m_grounded && !m_groundSensor.State())
      {
         m_grounded = false;
         m_animator.SetBool("Grounded", m_grounded);
      }

      // -- Handle input and movement --
      float inputX = 0.0f;

      if (m_disableMovementTimer < 0.0f)
         inputX = moveInput.ReadValue<Vector2>().x;

      // Check if current move input is larger than 0 and the move direction is equal to the characters facing direction
      if (Mathf.Abs(inputX) > Mathf.Epsilon && Mathf.Sign(inputX) == m_facingDirection)
         m_moving = true;

      else
         m_moving = false;

      // Swap direction of sprite depending on move direction
      if (inputX > 0)
      {
         GetComponent<SpriteRenderer>().flipX = false;
         m_facingDirection = 1;
      }

      else if (inputX < 0)
      {
         GetComponent<SpriteRenderer>().flipX = true;
         m_facingDirection = -1;
      }

      // We don't want the player to sprint / stop sprinting mid air
      if(m_grounded)
         m_currentSprintMultiplier = sprintInput.ReadValue<float>() > 0 ? m_sprintMultiplier : 1;

      // SlowDownSpeed helps decelerate the characters when stopping
      float SlowDownSpeed = m_moving ? 1.0f : 0.5f;
      // Set movement
      m_body2d.AddForce(new Vector2(inputX * m_maxSpeed * SlowDownSpeed * m_currentSprintMultiplier, m_body2d.velocity.y) - m_body2d.velocity, ForceMode2D.Impulse);
      //m_body2d.velocity = new Vector2(inputX * m_maxSpeed * SlowDownSpeed * sprintMultiplier, m_body2d.velocity.y);

      // Set AirSpeed in animator
      m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

      // Set Animation layer for hiding sword
      int boolInt = m_hideSword ? 1 : 0;
      m_animator.SetLayerWeight(1, boolInt);

      // -- Handle Animations --
      //Debug.Log($"m_grounded: {m_grounded} m_disableMovementTimer: {m_disableMovementTimer} m_moving: {m_moving}");
      ////Jump
      //if (Input.GetButtonDown("Jump") && m_grounded && m_disableMovementTimer < 0.0f)
      //{
      //   m_animator.SetTrigger("Jump");
      //   m_grounded = false;
      //   m_animator.SetBool("Grounded", m_grounded);
      //   m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
      //   m_groundSensor.Disable(0.2f);
      //}
      //Run
      if (m_grounded && m_moving)
         m_animator.SetInteger("AnimState", 1);

      //Idle
      else
         m_animator.SetInteger("AnimState", 0);
   }

   private void OnJump()
   {
      //Jump
      if (m_grounded && m_disableMovementTimer < 0.0f)
      {
         m_animator.SetTrigger("Jump");
         m_grounded = false;
         m_animator.SetBool("Grounded", m_grounded);
         m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
         m_groundSensor.Disable(0.2f);
      }
   }

   // Function used to spawn a dust effect
   // All dust effects spawns on the floor
   // dustXoffset controls how far from the player the effects spawns.
   // Default dustXoffset is zero
   void SpawnDustEffect(GameObject dust, float dustXOffset = 0)
   {
      if (dust != null)
      {
         // Set dust spawn position
         Vector3 dustSpawnPosition = transform.position + new Vector3(dustXOffset * m_facingDirection, 0.0f, 0.0f);
         GameObject newDust = Instantiate(dust, dustSpawnPosition, Quaternion.identity) as GameObject;
         // Turn dust in correct X direction
         newDust.transform.localScale = newDust.transform.localScale.x * new Vector3(m_facingDirection, 1, 1);
      }
   }

   // Animation Events
   // These functions are called inside the animation files
   void AE_runStop()
   {
      m_audioManager.PlaySound("RunStop");
      // Spawn Dust
      float dustXOffset = 0.6f;
      SpawnDustEffect(m_RunStopDust, dustXOffset);
   }

   void AE_footstep()
   {
      m_audioManager.PlaySound("Footstep");
   }

   void AE_Jump()
   {
      m_audioManager.PlaySound("Jump");
      // Spawn Dust
      SpawnDustEffect(m_JumpDust);
   }

   void AE_Landing()
   {
      m_audioManager.PlaySound("Landing");
      // Spawn Dust
      SpawnDustEffect(m_LandingDust);
   }
}
