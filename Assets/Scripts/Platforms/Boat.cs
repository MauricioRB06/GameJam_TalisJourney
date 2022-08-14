
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the boats that the player will use to cross the water.
//
// Documentation and References:
//
//  Unity OnnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//  Unity OnCollisionEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
//  Unity OnCollisionExit2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnCollisionExit2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using PowerUps;
using UnityEngine;

namespace Platforms
{
    // Components required for this Script to work.
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class Boat : MonoBehaviour
    {
        [Header("Boat Settings")] [Space(5)]
        [Tooltip("Sets the default speed that the boat will have when moving.")]
        [SerializeField][Range(0, 40)] private int boatVelocity = 25;
        
        // If the ship is hit by a wind type projectile it will move to the right based on the speed that is set.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("WindProjectile")) return;
            
            col.GetComponent<PowerUpWind>().BoatCollision();
            var boatTransform = transform;
            boatTransform.position += boatTransform.right * (boatVelocity * Time.deltaTime);
        }
        
        // When the player enters the ship, he is related to it in order to move it.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            collision.transform.SetParent(transform);
        }
        
        // When the player leaves the boat, he unparent with the boat to no longer move with it.
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.transform.CompareTag("Player")) return;
            collision.transform.SetParent(null);
        }
        
        // This function stops the speed of the boat and destroys this component so that it can no longer move.
        public void DestroyBoat()
        {
            boatVelocity = 0;
            Destroy(this);
        }
        
    }
}
