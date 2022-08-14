
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Establishes the behavior of the bushes.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;

namespace Platforms
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Animator))]
    
    public class Bush : MonoBehaviour
    {
        
        // ID parameter for the animator controller.
        private static readonly int Burn = Animator.StringToHash("Burn");
        
        // Variable to store the reference to the animator component.
        private Animator _animator;
        
        // Set the initial settings of the bush.
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        
        // If it collides with a fire-type projectile, it starts the destruction animation.
        private void OnCollisionEnter2D (Collision2D collision)
        {
            if (collision.gameObject.CompareTag("FireProjectile"))
            {
                _animator.SetTrigger(Burn);
            }
        }
        
        // This function is called from the animation controller.
        // It destroys the bush.
        public void BushFire()
        {
            Destroy(gameObject);
        }
        
    }
}
