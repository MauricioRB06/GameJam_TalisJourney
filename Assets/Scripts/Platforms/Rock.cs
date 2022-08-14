
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Set the behavior of the rocks.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity OnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace Platforms
{
    // Components required for this script to work.
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class Rock : MonoBehaviour
    {
        
        // Variable to store the collider component.
        private BoxCollider2D _rockBoxCollider;
        
        // Set the initial settings of the rock.
        private void Awake()
        {
            _rockBoxCollider = GetComponent<BoxCollider2D>();
            _rockBoxCollider.isTrigger = true;
        }
        
        // If it collides with the player at the bottom, it takes away all the player's life.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            
            col.gameObject.GetComponent<PlayerController>().PlayerHealth.TakeDamage(100);
        }
        
    }
}
