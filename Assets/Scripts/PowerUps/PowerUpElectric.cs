
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Establish the behavior of the Electricity PowerUp.
// 
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using System.Collections;
using Player;
using UnityEngine;

namespace PowerUps
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class PowerUpElectric : MonoBehaviour
    {
        
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("Set the velocity of the projectile.")]
        [SerializeField][Range(1.0f,20.0f)] private float projectileSpeed = 8.0f;
        [Tooltip("Set the life of the projectile in seconds.")]
        [SerializeField][Range(2f,8.0f)] private float projectileLife = 5.0f;
        
        // Variable to set the direction of movement of the projectile.
        private Vector3 _movementDirection;
        
        // Sets the initial projectile settings based on the player's direction of movement.
        private void Awake()
        {
            if (PlayerController.Instance.FacingDirection == 1)
            {
                _movementDirection = transform.right;
            }
            else
            {
                _movementDirection = -transform.right;
                transform.GetComponent<SpriteRenderer>().flipX = true;
            }
            
            transform.tag = "ElectricProjectile";
            StartCoroutine(DestroyProjectile());
        }
        
        // Updates the position of the projectile based on the direction of movement and velocity.
        private void Update()
        {
            transform.position += _movementDirection * (projectileSpeed * Time.deltaTime);
        }
        
        // If the projectile collides it destroys the self-destruct routine and destroys the object.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            StopCoroutine(DestroyProjectile());
            Destroy(gameObject);
        }
        
        // Coroutine that starts when the projectile is created, and destroys it when the life time is completed.
        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(projectileLife);
            Destroy(gameObject);
        }
        
    }
}
