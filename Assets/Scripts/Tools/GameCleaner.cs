
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  If it finds an object of type player, it eliminates it.
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Player;
using UnityEngine;

namespace Tools
{
    public class GameCleaner : MonoBehaviour
    {
        // References to the player and the user interface.
        private GameObject _player;
        private GameObject _hud;
        
        // When the level starts, it looks for references to the player and the interface and if so, removes them.
        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _hud = GameObject.FindGameObjectWithTag("HUD");
            
            if(_player != null)
            {
                PlayerController.Instance.KillPlayer();
            }
            else
            {
                _player = GameObject.FindGameObjectWithTag("Dead");
                if(_player != null)
                {
                    PlayerController.Instance.KillPlayer();
                }
            }
            
            if (_hud != null)
            {
                Destroy(_hud);
            }
            
            Destroy(gameObject);
        }

    }
}
