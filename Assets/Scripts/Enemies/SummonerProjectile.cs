
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Set the behavior of the projectiles of the final boss.
//
// Documentation and References:
//
//  Unity Awake Method: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity Update: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//  Unity Coroutine: https://docs.unity3d.com/ScriptReference/Coroutine.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using System.Collections;
using Player;
using UnityEngine;

namespace Enemies
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class SummonerProjectile : MonoBehaviour
    {
        
        [Header("Projectile Settings")] [Space(5)]
        [Tooltip("Define here the life of the projectile in seconds.")]
        [SerializeField][Range(2.0f,7.0f)] private float projectileLife = 5.0f;
        [Tooltip("Set the damage to be caused by the projectile.")]
        [SerializeField][Range(2.0f,10.0f)] private float projectileDamage = 5.0f;
        
        // Variables to control the speed and angle of movement of the projectile.
        private float _projectileSpeed;
        private Vector3 _projectileAngle;

        // It randomly defines the velocity and angle of motion that the projectile will have.
        private void Awake()
        {
            _projectileSpeed = Random.Range(4f, 9f);
            _projectileAngle = new Vector3(-1.0f, Random.Range(0.75f,-0.75f),0.0f);
            StartCoroutine(DestroyProjectile());
        }
        
        // Updates in each frame the position of the projectile based on its velocity and angle of movement.
        private void Update()
        {
            transform.position += _projectileAngle * (_projectileSpeed * Time.deltaTime);
        }
        
        // If the projectile collides it is destroyed and if it collides with the player it applies damage.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.transform.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(projectileDamage);
            }
            
            StopCoroutine(DestroyProjectile());
            Destroy(gameObject);
        }
        
        // Coroutine that destroys the projectile after its life time is over.
        private IEnumerator DestroyProjectile()
        {
            yield return new WaitForSeconds(projectileLife);
            Destroy(gameObject);
        }
        
    }
}
