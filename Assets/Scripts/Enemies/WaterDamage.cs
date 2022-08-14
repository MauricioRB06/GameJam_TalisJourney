
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the water zone.
//
// Documentation and References:
//
//  Unity OnTriggerEnter2D": "https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace Enemies
{
    // Component required for this script to work.
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class WaterDamage : MonoBehaviour
    {
        
        // If the player collides with the object, damage equal to the character's life is applied.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.CompareTag("Player")) return;
            
            col.transform.GetComponent<PlayerController>().PlayerHealth.TakeDamage(100.0f);
            col.transform.GetComponent<PlayerController>().KnockBackAnimation();
        }
        
    }
}
