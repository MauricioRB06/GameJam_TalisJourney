
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  Changes the possibility of attacking the player, when the player collides with the object containing this script.
//
//  Documentation and References:
//
//  Unity OnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace Tools
{
    // Component required for this script to work.
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class PlayerAttackChanger : MonoBehaviour
    {
        
        // When the player collides, it calls the function that changes its attack state.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.gameObject.CompareTag("Player")) return;
            col.GetComponent<PlayerController>().ChangeAttackState();
            Destroy(gameObject);
        }
        
    }
}
