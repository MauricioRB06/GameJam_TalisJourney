
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Define the composition of the basic enemies of the game.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/Collision2D.OnCollisionEnter2D.html
//  C# Enums: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

// Enumerator containing the types of enemy, with which their properties will be configured.
public enum EnemyType
{
    FireEnemy,
    WindEnemy,
    WaterEnemy,
    ElectricEnemy
}

namespace Enemies
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    
    public class EnemyBasic : MonoBehaviour
    {
        
        [Header("Enemy Settings")] [Space(5)]
        [Tooltip("Sets the enemy type.")]
        [SerializeField] private EnemyType enemyType;
        [Space(15)]
        
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("The waiting time between each movement.")]
        [Range(0.1F, 5F)] [SerializeField] private float movementWaitingTime = 1;
        [Tooltip("Enemy movement speed.")]
        [Range(1F, 10F)] [SerializeField] private float movementSpeed = 1;
        [Space(15)]
        
        [Header("Movement Points Route")] [Space(5)]
        [Tooltip("There must be at least 2 points on the route.")]
        [SerializeField] private Transform[] movementPoints;
        [Space(15)]
        
        [Header("Damage Settings")] [Space(5)] 
        [Tooltip("If it is not a platform, it may cause damage to the player.")]
        [Range(1.0F, 30.0f)][SerializeField] private float damageToGive = 5;
        [Tooltip("Strength to be applied to the player.")]
        [Range(20.0f, 60.0f)][SerializeField] private float knockbackForce = 40;
        [Tooltip("It is the duration that the Knockback will last.")]
        [Range(1.0F, 2.0f)][SerializeField] private float knockbackDuration = 1;
        [Space(15)]
        
        // Checks how much time has elapsed at one point before switching to the next point.
        private float _currentWaitTime;
        
        // Navigates through the movementPoints to indicate to the object to which it should move.
        private int _movementPointIterator;

        // Sets the initial settings of the object.
        private void Awake()
        {
            if ( movementPoints.Length < 2)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            
            _currentWaitTime = movementWaitingTime;
        }
        
        // Checks if the object is movable and makes it scroll through the list of movement points.
        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, 
                movementPoints[_movementPointIterator].transform.position, 
                movementSpeed * Time.deltaTime);
            
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
        
        // Check if the object collided with the player to apply damage and knockback.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Player"))
            {
                collision.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(damageToGive);
                collision.transform.GetComponent<PlayerController>().KnockBackAnimation();
                collision.transform.GetComponent<PlayerController>().KnockBack(knockbackDuration,
                    knockbackForce, transform);
            }
            else switch (enemyType)
            {
                case EnemyType.WaterEnemy when collision.transform.CompareTag("FireProjectile"):
                    Destroy(collision.gameObject);
                    Destroy(transform.parent.gameObject);
                    break;
                case EnemyType.ElectricEnemy when collision.transform.CompareTag("WindProjectile"):
                    Destroy(collision.gameObject);
                    Destroy(transform.parent.gameObject);
                    break;
                case EnemyType.FireEnemy when collision.transform.CompareTag("WaterProjectile"):
                    Destroy(collision.gameObject);
                    Destroy(transform.parent.gameObject);
                    break;
                case EnemyType.WindEnemy when collision.transform.CompareTag("ElectricProjectile"):
                    Destroy(collision.gameObject);
                    Destroy(transform.parent.gameObject);
                    break;
            }
        }
        
    }
}
