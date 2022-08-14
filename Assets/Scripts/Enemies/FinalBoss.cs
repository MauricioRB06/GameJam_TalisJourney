
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the final boss of the game.
//
// Documentation and References:
//
//  Unity Awake Method: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//  Unity Coroutine: https://docs.unity3d.com/ScriptReference/Coroutine.html
//  C# Enums: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

// Enumerator that has the states that the boss may have with which the configurations will be made.
public enum BossState
{
    FireState,
    WindState,
    WaterState,
    ElectricState
}

namespace Enemies
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    
    public class FinalBoss : MonoBehaviour
    {
        
        [Header("Boss Settings")] [Space(5)]
        [Tooltip("Sets the status of the boss.")]
        [SerializeField] private BossState bossState;
        [Tooltip("Sets the initial life of the boss.")]
        [Range(100f, 1000f)] [SerializeField] private float bossLife = 500f;
        [Tooltip("Sets the damage that the boss will receive with the fire projectile.")]
        [Range(5f, 10f)] [SerializeField] private float damageByFire = 5f;
        [Tooltip("Sets the damage that the boss will receive with the wind projectile.")]
        [Range(5f, 10f)] [SerializeField] private float damageByWind = 5f;
        [Tooltip("Sets the damage that the boss will receive with the water projectile.")]
        [Range(5f, 10f)] [SerializeField] private float damageByWater = 5f;
        [Tooltip("Sets the damage that the boss will receive with the electric projectile.")]
        [Range(5f, 10f)] [SerializeField] private float damageByElectric = 5f;
        [Tooltip("Sets the time it will take for the boss to change to another state.")]
        [SerializeField][Range(10.0f, 30.0f)] private float stateChangerTime = 15.0f;
        [Tooltip("Sets the time between each boss attack.")]
        [SerializeField][Range(2.0f, 10.0f)] private float bossAttackTime = 5.0f;
        [Tooltip("Sets the object that will represent the boss's fire shield.")]
        [SerializeField] private GameObject bossFireShield;
        [Tooltip("Sets the object that will represent the boss's wind shield.")]
        [SerializeField] private GameObject bossWindShield;
        [Tooltip("Sets the object that will represent the boss's water shield.")]
        [SerializeField] private GameObject bossWaterShield;
        [Tooltip("Sets the object that will represent the boss's electric shield.")]
        [SerializeField] private GameObject bossElectricShield;
        [Space(15)]
        
        [Header("Projectile Settings")] [Space(5)]
        [Tooltip("Set the projectiles that will be launched by the final boss.")]
        [SerializeField] private GameObject summonerProjectile;
        [Tooltip("Set the points from which the projectiles will be launched.")]
        [SerializeField] private Transform[] spawnPoints;
        [Space(15)]
        
        [Header("Final Boss Camera Settings")] [Space(5)]
        [Tooltip("Place here the final camera confinement that is replaced after the boss dies.")]
        [SerializeField] private Collider2D finalConfiner;
        [Tooltip("Place here the Cinemachine virtual camera containing the CameraConfiner plug-in.")]
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [Space(15)]
        
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("The waiting time between each movement")]
        [Range(0.1F, 5F)] [SerializeField] private float movementWaitingTime = 1;
        [Tooltip("Final Boss movement speed")]
        [Range(1F, 10F)] [SerializeField] private float movementSpeed = 1;
        [Space(15)]
        
        [Header("Movement Points Route")] [Space(5)]
        [Tooltip("There must be at least 2 points on the route.")]
        [SerializeField] private Transform[] movementPoints;
        [Space(15)]
        
        [Header("Final Battle Settings")] [Space(5)]
        [Tooltip("Place here the TileMap that will be unlocked when the boss dies.")]
        [SerializeField] private GameObject finalGround;
        [Tooltip("Place here the final Teleporter that will be unlocked when the boss dies.")]
        [SerializeField] private GameObject finalPortal;
        [Tooltip("Place here the AudioClip that will play when the boss dies.")]
        [SerializeField] private AudioClip completeGame;
        
        // Variables that will store the internal components of the object to be able to access and configure them.
        private Animator _boosAnimator;
        private CapsuleCollider2D _bossCollider;
        private AudioSource _bossAudioSource;
        
        // Variable to determine if the boss is dead or not.
        private bool _bossIsDead;
        
        // Delegate in charge of communicating the value of the boss's life.
        public static event Action<float> BossHealthDelegate;
        
        // Checks how much time has elapsed at one point before switching to the next point.
        private float _currentWaitTime;
        
        // Navigates through the movementPoints to indicate to the object to which it should move.
        private int _movementPointIterator;
        
        // Variables that control the time for the boss to attack and change state.
        private float _currentChangeStateTime;
        private float _currentAttackTime;
        
        // ID Parameters for animator.
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Attack = Animator.StringToHash("Attack");
        
        // Sets the initial settings of the object.
        private void Awake()
        {
            _boosAnimator = GetComponent<Animator>();
            _bossCollider = GetComponent<CapsuleCollider2D>();
            _bossAudioSource = GetComponent<AudioSource>();
                
            if ( movementPoints.Length < 2)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            
            if ( spawnPoints.Length < 1)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} needs at least " +
                               "one Spawn point to launch projectiles, please enter at least one.</b></color>");
            }
            
            if ( finalPortal == null)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} needs a teleporter object" +
                               " to unlock when he dies, please enter one.</b></color>");
            }
            else
            {
                finalPortal.SetActive(false);
            }
            
            if ( finalGround == null)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} needs a TileMap item" +
                               " to unlock when he dies, please enter one.</b></color>");
            }
            else
            {
                finalGround.SetActive(false);
            }
            
            _currentWaitTime = movementWaitingTime;
            _currentChangeStateTime = stateChangerTime;
            _currentAttackTime = bossAttackTime;
        }
        
        // If the boss is not yet dead, it is in charge of moving it from one side of the map to the other.
        private void Update()
        {
            
            if (_bossIsDead) return;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                movementPoints[_movementPointIterator].transform.position, 
                movementSpeed * Time.deltaTime);
            
            // First we check whether we should change the status.
            if (_currentChangeStateTime <= 0) 
            {
                BossChangeState();
                _currentChangeStateTime = stateChangerTime;
            }
            else
            {
                _currentChangeStateTime -= Time.deltaTime; 
            }
            
            // Then we confirm if we can attack.
            if (_currentAttackTime <= 0) 
            {
                _bossAudioSource.Play();
                BossAttack();
                _currentAttackTime = bossAttackTime;
            }
            else
            {
                _currentAttackTime -= Time.deltaTime; 
            }
            
            // Finally it is confirmed if we have already reached Point B to start moving to Point A.
            if (!(Vector2.Distance(transform.position, 
                movementPoints[_movementPointIterator].transform.position) < 0.1f)) return;

            if (_currentWaitTime <= 0) 
            {
                if (movementPoints[_movementPointIterator] != movementPoints[movementPoints.Length - 1])
                {
                    _movementPointIterator++;
                }
                else
                {
                    _movementPointIterator = 0;
                }

                _currentWaitTime = movementWaitingTime;
            }
            else
            {
                _currentWaitTime -= Time.deltaTime; 
            }
        }
        
        // Creates a projectile at each Spawn Point assigned to the boss.
        private void BossAttack()
        {
            _boosAnimator.SetTrigger(Attack);
            foreach (var elements in spawnPoints)
            {
                Instantiate(summonerProjectile, elements);
            }
        }
        
        // Randomly changes the status of the boss and activates the corresponding visual effect.
        private void BossChangeState()
        {
            _bossCollider.enabled = false;

            switch (bossState)
            {
                case BossState.FireState:
                    bossFireShield.SetActive(false);
                    break;
                case BossState.WindState:
                    bossWindShield.SetActive(false);
                    break;
                case BossState.WaterState:
                    bossWaterShield.SetActive(false);
                    break;
                case BossState.ElectricState:
                    bossElectricShield.SetActive(false);
                    break;
            }
            
            bossState = (BossState) UnityEngine.Random.Range(0, 4);

            StartCoroutine(RestoreShield());
        }
        
        // A coroutine that enables the boss collider and displays the new shield corresponding to the change of state.
        private IEnumerator RestoreShield()
        {
            yield return new WaitForSeconds(1);
            
            _bossCollider.enabled = true;
            
            switch (bossState)
            {
                case BossState.FireState:
                    bossFireShield.SetActive(true);
                    break;
                case BossState.WindState:
                    bossWindShield.SetActive(true);
                    break;
                case BossState.WaterState:
                    bossWaterShield.SetActive(true);
                    break;
                case BossState.ElectricState:
                    bossElectricShield.SetActive(true);
                    break;
            }
        }
        
        // Check if the colliding projectile is capable of damaging you based on the state.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (bossState)
            {
                case BossState.WaterState when collision.transform.CompareTag("FireProjectile"):
                    Destroy(collision.gameObject);
                    bossLife -= damageByFire;
                    break;
                case BossState.ElectricState when collision.transform.CompareTag("WindProjectile"):
                    Destroy(collision.gameObject);
                    bossLife -= damageByWind;
                    break;
                case BossState.FireState when collision.transform.CompareTag("WaterProjectile"):
                    Destroy(collision.gameObject);
                    bossLife -= damageByWater;
                    break;
                case BossState.WindState when collision.transform.CompareTag("ElectricProjectile"):
                    Destroy(collision.gameObject);
                    bossLife -= damageByElectric;
                    break;
            }
            
            BossHealthDelegate?.Invoke(bossLife);
            BossCheckLife();
        }
        
        // Check if the boss's life has dropped below 0 to trigger the death process.
        private void BossCheckLife()
        {
            if (!(bossLife <= 0)) return;
            
            bossLife = 0;
            _bossCollider.enabled = false;
            _bossIsDead = true;
            transform.position = new Vector3(5f, 3f,0f);
            finalGround.SetActive(true);
            bossFireShield.SetActive(false);
            bossWindShield.SetActive(false);
            bossWaterShield.SetActive(false);
            bossElectricShield.SetActive(false);
            _boosAnimator.SetTrigger(Dead);
            virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = finalConfiner;
            _bossAudioSource.PlayOneShot(completeGame);
        }
        
        // This function is called from the animation controller.
        // When called, it reveals the final portal and destroys the boss.
        public void KillBoss()
        {
            finalPortal.SetActive(true);
            Destroy(transform.parent.gameObject);
        }
        
    }
}
