
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Control the behavior of objects that can harm the player.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
// 
// -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace DamageObjects
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PolygonCollider2D))]
    
   public class DamageObject : MonoBehaviour
    {
        
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("Sets whether the object will move or not.")]
        [SerializeField] private bool isMovableDamageObject;
        [Tooltip("The waiting time between each movement.")]
        [Range(0.1F, 5F)] [SerializeField] private float movementWaitingTime = 1;
        [Tooltip("Object movement speed.")]
        [Range(1F, 10F)] [SerializeField] private float movementSpeed = 1;
        [Space(15)]
        
        [Header("Movement Points Route")] [Space(5)]
        [Tooltip("There must be at least 2 points on the route.")]
        [SerializeField] private Transform[] movementPoints;
        [Space(15)]
        
        [Header("Damage Settings")] [Space(5)] 
        [Tooltip("Damage that will be done to the player when colliding with this object.")]
        [Range(1.0F, 30.0f)][SerializeField] private float damageToGive = 5;
        [Tooltip("Strength to be applied to the player.")]
        [Range(20.0f, 100.0f)][SerializeField] private float knockbackForce = 80;
        [Tooltip("It is the duration that the Knockback will last.")]
        [Range(1.0F, 2.0f)][SerializeField] private float knockbackDuration = 1;
        
        // Checks how much time has elapsed at one point before switching to the next point.
        private float _currentWaitTime;
        
        // Navigates through the movementPoints to indicate to the object to which it should move.
        private int _movementPointIterator;
        
        // Verify that if the object is mobile, it has the minimum number of waypoints necessary.
        private void Awake()
        {
            if (isMovableDamageObject && movementPoints.Length < 2)
            {
                Debug.LogError($"<color=#D22323><b>The object: {gameObject.name} has been configured" +
                               " as movable, please add at least 2 points in the movement route.</b></color>");
            }
            
            _currentWaitTime = movementWaitingTime;
        }
        
        // Checks if the object is movable and makes it scroll through the list of movement points.
        private void Update()
        {
            if (!isMovableDamageObject) return;
            
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
            if (!collision.transform.CompareTag("Player")) return;
            
            collision.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(damageToGive);
            collision.transform.GetComponent<PlayerController>().KnockBackAnimation();
            collision.transform.GetComponent<PlayerController>().KnockBack(knockbackDuration,
                knockbackForce, transform);
        }
        
    }
}
