
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the main character's controller.
//
// Documentation and References:
//
//  Unity Awake: http://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity FixedUpdate: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
//  C# Enumerators: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/enum
//  C# Expression Bodies: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/statements-expressions-operators/expression-bodied-members
//  C# Properties: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/properties
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

// Enumerator containing the player's powers.
public enum PlayerStatePowerUp
{
    FirePowerUp,
    WindPowerUp,
    WaterPowerUp,
    ElectricPowerUp
}

namespace Player
{
    // Components required for this script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    
    public class PlayerController : MonoBehaviour
    {
        
        // Variable that will contain the unique instance of the player (Singleton Pattern).
        public static PlayerController Instance;
        
        [Header("Player Stats Settings")] [Space(5)]
        [Tooltip("Set the movement speed of the character.")]
        [ SerializeField ] [Range(3,10)] private float movementSpeed = 6f;
        [Tooltip("Set the jump strength of the character.")]
        [ SerializeField ] [Range(5,15)] private float jumpForce = 12f;
        [Tooltip("Set here the layer where the ground is painted.")]
        [ SerializeField ] private LayerMask groundLayer;
        [Tooltip("Set here the position from which the floor will be checked.")]
        [ SerializeField ] private Transform groundPoint;
        [Tooltip("Set here the position from which the walls will be checked.")]
        [ SerializeField ] private Transform wallPoint;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [Tooltip("Place here the projectile of the fire PowerUp.")]
        [ SerializeField ] private GameObject firePowerUp;
        [Tooltip("Place here the projectile of the wind PowerUp.")]
        [ SerializeField ] private GameObject windPowerUp;
        [Tooltip("Place here the projectile of the water PowerUp.")]
        [ SerializeField ] private GameObject waterPowerUp;
        [Tooltip("Place here the projectile of the eletric PowerUp.")]
        [ SerializeField ] private GameObject electricPowerUp;
        [Space(15)]
        
        [Header("SFX Settings")] [Space(5)]
        [Tooltip("Place here the audio clip of the player's jump.")]
        [SerializeField] private AudioClip playerJump;
        [Tooltip("lace here the audio clip of the player's walk.")]
        [SerializeField] private AudioClip playerWalk;
        [Tooltip("lace here the audio clip of the player's Dead.")]
        [SerializeField] private AudioClip playerDead;
        
        // Delegates to report on character life and game pause status.
        public static event Action<int> PlayerPowerUp;
        public static event Action<bool> PlayerPause;
        
        // Variable to control the pause state of the game.
        private bool _isPaused;
        
        // Variables to enable and disable the player's contours.
        private bool _attackControllerEnabled = true;
        private bool _moveControllerEnabled = true;
        
        // We use it to detect the direction in which the object is facing
        public int FacingDirection { get; private set; }

        // We use it to avoid having to create a new vector every time the character moves
        private Vector2 _newVelocity;
        
        // Variables to control Knockback.
        private bool _isKnockbackActive;
        private float _knockbackStartTime;
        
        // Variable to control the power that the player is controlling.
        private PlayerStatePowerUp _playerPowerUp;
        
        // Variables to control the player's components.
        private Rigidbody2D _playerRigidbody;
        private Animator _playerAnimator;
        private AudioSource _playerAudioSource;
        
        // Variables to know if we are touching the floor or the walls.
        private bool _isGrounded;
        private bool _wallCheck;
        
        // Component to manage the character's life.
        public PlayerHealth PlayerHealth { get; private set; }
        
        // Variables to control the player's attack and read the input from the controls.
        private float _inputX;
        private bool _canAttack;

        // ID Parameters for the animation controller.
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Damage = Animator.StringToHash("Damage");
        private static readonly int Death = Animator.StringToHash("Death");

        // Initial settings of the player.
        private void Awake()
        {
            // Singleton Pattern.
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            _playerPowerUp = PlayerStatePowerUp.FirePowerUp;
            _playerRigidbody = GetComponent<Rigidbody2D>();
            _playerAnimator = GetComponent<Animator>();
            PlayerHealth = GetComponent<PlayerHealth>();
            _playerAudioSource = GetComponent<AudioSource>();
            _isPaused = false;
            _attackControllerEnabled = true;
            _moveControllerEnabled = true;
        }
        
        // Checks the player's status and updates the speed.
        private void FixedUpdate()
        {
            _isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.2f, groundLayer);
            _wallCheck = Physics2D.Raycast(wallPoint.position,
                Vector2.right * FacingDirection, 0.4f, groundLayer);
            
            if (!_wallCheck)
            {
                _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
            }
            else
            {
                if (!_isGrounded && ((FacingDirection == 1 && _inputX < 0) || (FacingDirection == -1 && _inputX > 0)))
                {
                    _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
                }
                else if (!_isGrounded)
                {
                    _playerRigidbody.velocity = new Vector2(0f, -5.0f);
                }
                else if ((FacingDirection == 1 && _inputX < 0) || (FacingDirection == -1 && _inputX > 0))
                {
                    _playerRigidbody.velocity = new Vector2(_inputX * movementSpeed, _playerRigidbody.velocity.y);
                }
            }
            
            _playerAnimator.SetFloat(Speed, Mathf.Abs(_playerRigidbody.velocity.x));
            _playerAnimator.SetBool(IsGrounded, _isGrounded);
            
            // Flip the character
            if (_playerRigidbody.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
                FacingDirection = 1;
            }
            else if (_playerRigidbody.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FacingDirection = -1;
            }
        }
        
        // Input Action Event - Reads user input and stores it.
        public void OnMove(InputAction.CallbackContext context)
        {
            if (_moveControllerEnabled)
            {
                _inputX = Mathf.RoundToInt((context.ReadValue<Vector2>().x));
            }
        }
        
        // Input Action Event - Reads user input to verify if I jump with fire PowerUp.
        public void OnJumpFire(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.FirePowerUp;
            PlayerPowerUp?.Invoke(1);
            _canAttack = true;
        }
        
        // Input Action Event - Reads user input to verify if I jump with wind PowerUp.
        public void OnJumpWind(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WindPowerUp;
            PlayerPowerUp?.Invoke(2);
            _canAttack = true;
        }
        
        // Input Action Event - Reads user input to verify if I jump with water PowerUp.
        public void OnJumpWater(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.WaterPowerUp;
            PlayerPowerUp?.Invoke(3);
            _canAttack = true;
        }
        
        // Input Action Event - Reads user input to verify if I jump with electric PowerUp.
        public void OnJumpElectric(InputAction.CallbackContext context)
        {
            if (!_moveControllerEnabled) return;
            if (!context.started || !_isGrounded) return;
            
            _playerAnimator.SetTrigger(Jump);
            _playerAudioSource.PlayOneShot(playerJump);
            _playerRigidbody.velocity = new Vector2(_playerRigidbody.velocity.x, jumpForce);
            _playerPowerUp  = PlayerStatePowerUp.ElectricPowerUp;
            PlayerPowerUp?.Invoke(4);
            _canAttack = true;
        }
        
        // Input Action Event - Reads user input to verify if the player attacked.
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (!_attackControllerEnabled) return;
            if (!_canAttack) return;
            
            _playerAnimator.SetTrigger(Attack);
            
            var playerTransform = transform;
            
            switch (context.started)
            {
                case true when _playerPowerUp == PlayerStatePowerUp.FirePowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(firePowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.WindPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(windPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.WaterPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(waterPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
                
                case true when _playerPowerUp == PlayerStatePowerUp.ElectricPowerUp && _canAttack:
                    _canAttack = false;
                    Instantiate(electricPowerUp, playerTransform.position, playerTransform.rotation);
                    break;
            }
        }
        
        // Function that is executed when the character dies.
        public void Dead()
        {
            _playerAnimator.SetTrigger(Death);
            transform.tag = "Dead";
            _playerAudioSource.PlayOneShot(playerDead);
        }
        
        // Function that activates the death HUD.
        public void ActivateDeadHUD()
        {
            HUDGameplay.Instance.Dead();
        }
        
        // Function that destroys the player. 
        public void KillPlayer()
        {
            Destroy(gameObject);
        }
        
        // Function that plays the Knockback animation.
        public void KnockBackAnimation()
        {
            _playerAnimator.SetTrigger(Damage);
        }
        
        // Function that starts the Knockback coroutine.
        public void KnockBack(float knockbackDuration, float knockbackPower, Transform obj)
        {
            StartCoroutine(KnockbackImpulse(knockbackDuration, knockbackPower, obj));
        }
        
        // Coroutine that applies the Knockback force to the character.
        private IEnumerator KnockbackImpulse(float knockbackDuration, float knockbackPower, Transform obj)
        {
            float timer = 0;
            
            while (knockbackDuration > timer)
            {
                
                timer += Time.deltaTime;
                Vector2 direction = (transform.position - obj.position).normalized;
                if (Math.Abs(direction.x - -1.0f) < 0.1 || Math.Abs(direction.x - 1.0f) < 0.1)
                {
                    _playerRigidbody.AddForce(new Vector2(direction.x * knockbackPower, direction.y * knockbackPower));
                }
                else
                {
                    _playerRigidbody.AddForce(new Vector2(direction.x * knockbackPower/4, direction.y * knockbackPower/4));
                }
                
            }
            
            yield return 0;
        }
        
        // Change the pause state of the game when the game is started by pressing the corresponding key.
        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                PlayerPause?.Invoke(!_isPaused);
                _isPaused = !_isPaused;
            }
        }
        
        // This function is called from the animation controller.
        // Allows the character to attack again.
        public void CanAttack()
        {
            _canAttack  = true;
        }

        // Changes the state of the player's attack controls, to allow or disallow him to attack.
        public void ChangeAttackState()
        {
            _attackControllerEnabled = !_attackControllerEnabled;
        }
        
        // Changes the state of the player's movement controls, to allow or disallow him to attack.
        public void ChangeControllerState()
        {
            _moveControllerEnabled = !_moveControllerEnabled;
        }
        
        // This function is called from the animation controller.
        // Plays a walking sound if the player is touching the ground.
        public void PlayerWalk()
        {
            if (!_isGrounded) return;
            _playerAudioSource.PlayOneShot(playerWalk);
        }
        
    }
}
