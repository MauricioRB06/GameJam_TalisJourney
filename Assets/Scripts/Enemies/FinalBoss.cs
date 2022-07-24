
using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

public enum BossState
{
    FireState,
    WindState,
    WaterState,
    ElectricState
}

namespace Enemies
{
    // 
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AudioSource))]
    
    public class FinalBoss : MonoBehaviour
    {
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("The waiting time between each movement")]
        [SerializeField] private BossState bossState;
        [Tooltip("The waiting time between each movement")]
        [Range(0.1F, 5F)] [SerializeField] private float movementWaitingTime = 1;
        [Tooltip("Object movement speed")]
        [Range(1F, 10F)] [SerializeField] private float movementSpeed = 1;
        [Tooltip("Object movement speed")]
        [Range(100f, 1000f)] [SerializeField] private float bossLife = 500f;
        [Tooltip("Object movement speed")]
        [Range(5f, 10f)] [SerializeField] private float damageByFire = 5f;
        [Tooltip("Object movement speed")]
        [Range(5f, 10f)] [SerializeField] private float damageByWind = 5f;
        [Tooltip("Object movement speed")]
        [Range(5f, 10f)] [SerializeField] private float damageByWater = 5f;
        [Tooltip("Object movement speed")]
        [Range(5f, 10f)] [SerializeField] private float damageByElectric = 5f;
        [Space(15)]
        
        [Header("Movement Points Route")] [Space(5)]
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private Transform[] movementPoints;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField][Range(10.0f, 30.0f)] private float stateChangerTime = 15.0f;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField][Range(2.0f, 10.0f)] private float bossAttackTime = 5.0f;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private GameObject bossFireShield;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private GameObject bossWindShield;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private GameObject bossWaterShield;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private GameObject bossElectricShield;
        [Tooltip("There must be at least 2 points on the route")]
        
        [Header("Movement Points Route")] [Space(5)]
        [SerializeField] private GameObject finalGround;
        [SerializeField] private GameObject finalPortal;
        [Space(15)]
        
        [Header("Final Boss Camera Settings")] [Space(5)]
        [SerializeField] private Collider2D finalConfiner;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [Space(15)]
        
        [Header("Movement Points Route")] [Space(5)]
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private GameObject summonerProjectile;
        [Tooltip("There must be at least 2 points on the route")]
        [SerializeField] private Transform[] spawnPoints;
        [Space(15)]
        
        [Header("PowerUps Settings")] [Space(5)]
        [SerializeField] private AudioClip completeGame;
        
        private Animator _boosAnimator;
        private CapsuleCollider2D _bossCollider;
        private AudioSource _bossAudioSource;
        private bool _bossIsDead;
        
        // 
        public static event Action<float> BossHealthDelegate;
        
         // Checks how much time has elapsed at one point before switching to the next point.
        private float _currentWaitTime;
        
        // Navigates through the movementPoints to indicate to the object to which it should move.
        private int _movementPointIterator;
        private float _currentChangeStateTime;
        private float _currentAttackTime;
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
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            
            if ( finalPortal == null)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            else
            {
                finalPortal.SetActive(false);
            }
            
            if ( finalGround == null)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            else
            {
                finalGround.SetActive(false);
            }
            
            _currentWaitTime = movementWaitingTime;
            _currentChangeStateTime = stateChangerTime;
            _currentAttackTime = bossAttackTime;
        }
        
        // Checks if the object is movable and makes it scroll through the list of movement points.
        private void Update()
        {
            
            if (_bossIsDead) return;
            
            transform.position = Vector2.MoveTowards(transform.position, 
                movementPoints[_movementPointIterator].transform.position, 
                movementSpeed * Time.deltaTime);
            
            // 
            if (_currentChangeStateTime <= 0) 
            {
                BossChangeState();
                _currentChangeStateTime = stateChangerTime;
            }
            else
            {
                _currentChangeStateTime -= Time.deltaTime; 
            }
            
            // 
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
            
            if (!(Vector2.Distance(transform.position, 
                movementPoints[_movementPointIterator].transform.position) < 0.1f)) return;
            
            // 
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
        
        //
        private void BossAttack()
        {
            _boosAnimator.SetTrigger(Attack);
            foreach (var elements in spawnPoints)
            {
                Instantiate(summonerProjectile, elements);
            }
        }
        
        //
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
        
        //
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
        
        // Check if the object collided with the player to apply damage.
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
        
        //
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
        
        // 
        public void KillBoss()
        {
            finalPortal.SetActive(true);
            Destroy(transform.parent.gameObject);
        }
        
    }
}
