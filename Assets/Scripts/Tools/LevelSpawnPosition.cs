
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  Place the player in the correct position at the beginning of each level.
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;

namespace Tools
{
    public class LevelSpawnPosition : MonoBehaviour
    {
        
        // Reference to the player.
        private GameObject _player;
        
        // Find the player and place it in the same position as the object containing this component.
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _player.GetComponent<Transform>().position = transform.position;
            Destroy(gameObject);
        }

    }
}
