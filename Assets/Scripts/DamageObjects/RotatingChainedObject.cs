
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Define the behavior of rotating damage objects.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity FixedUpdate: https://docs.unity3d.com/ScriptReference/MonoBehaviour.FixedUpdate.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace DamageObjects
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class RotatingChainedObject : MonoBehaviour
    {
        
        [Header("Axis Settings")] [Space(5)]
        [Tooltip("Determine the pivot point of rotation of the chainedObject.")]
        [SerializeField] private Transform rotationAxis;
        [Space(15)]
        
        [Header("Rotation Settings")] [Space(5)]
        [Tooltip("True to rotate left and False to rotate right.")]
        [SerializeField] private bool rotationDirection;
        [Tooltip("Sets the rotation speed.")]
        [Range(100f, 300f)] [SerializeField] private float rotationSpeed = 100f;
        [Space(15)]
        
        [Header("Damage Settings")] [Space(5)]
        [Tooltip("Sets the amount of damage it causes when it collides.")]
        [Range(5f, 50f)] [SerializeField] private float damageToGive = 10f;
        [Tooltip("Strength to be applied to the player.")]
        [Range(20.0f, 100.0f)][SerializeField] private float knockbackForce = 80;
        [Tooltip("It is the duration that the Knockback will last.")]
        [Range(1.0F, 2.0f)][SerializeField] private float knockbackDuration = 1;
        [Space(15)]
        
        // Stores and determines the direction of rotation of the object based on the set configurations.
        private Vector3 _currentRotationDirection;
        
        // Check that the components necessary for the operation are not empty and instantiate the sound effect.
        private void Awake()
        {
            if (rotationAxis == null)
            {
                Debug.LogError(
                    "<color=#D22323><b>The chained object axis cannot be empty, please add one</b></color>");
            }
        }

        // While the object is rotating, it spins around its axis of rotation at the set speed.
        private void  FixedUpdate()
        {
            _currentRotationDirection = rotationDirection ? Vector3.forward : Vector3.back;
            
            transform.RotateAround(rotationAxis.position, _currentRotationDirection,
                rotationSpeed * Time.deltaTime);
        }

        // Check if it collided with the player, to cause the established damage.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            
            collision.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(damageToGive);
            collision.transform.GetComponent<PlayerController>().KnockBackAnimation();
            collision.transform.GetComponent<PlayerController>().KnockBack(knockbackDuration,
                knockbackForce, transform);
        }

    }
}
