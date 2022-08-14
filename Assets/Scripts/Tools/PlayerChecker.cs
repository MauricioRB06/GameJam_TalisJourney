
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  To free the player from matching with other objects and make it a persistent object.
//
//  Documentation and References:
//
//  Unity OnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;

namespace Tools
{
    // Component required fot this script to work.
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class PlayerChecker : MonoBehaviour
    {
        
        // When the player collides, it removes the pairings and makes it a persistent object.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.transform.CompareTag("Player")) return;
            col.transform.SetParent(null);
            DontDestroyOnLoad(col.gameObject);
            Destroy(gameObject);
        }
        
    }
}
