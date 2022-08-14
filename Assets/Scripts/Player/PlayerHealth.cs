
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the component that manages the player's life.
//
// Documentation and References:
//
//  Unity Start: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        
        [Header("Movement Settings")] [Space(5)]
        [Tooltip("Sets the player's life value.")]
        [SerializeField] private float healthAmount;
        [Tooltip("Sets the maximum life value of the player.")]
        [SerializeField] private float maxHealth;
        
        // Delegate to report on the player's life.
        public static event Action<float> PlayerHealthDelegate;
        
        // Variable to control whether the player is dead or not.
        private bool _playerIsDead;
        
        // Initial call to update the HUD with the player's life.
        private void Start()
        {
            PlayerHealthDelegate?.Invoke(healthAmount);
        }
        
        // Function to disable player controls when he dies and start his animation.
        private void Dead()
        {
            GetComponent<PlayerController>().ChangeAttackState();
            GetComponent<PlayerController>().ChangeControllerState();
            GetComponent<PlayerController>().Dead();
        }
        
        // Function to apply damage to the player.
        public void TakeDamage (float damageAmount)
        {
            healthAmount -= damageAmount;
            
            PlayerHealthDelegate?.Invoke(healthAmount);

            if (!(healthAmount <= 0)) return;
            
            healthAmount = 0;
            
            if (!_playerIsDead)
            {
                Dead();
            }
            
            _playerIsDead = true;
        }
        
        // Function to heal the player.
        public void CureHealth (float healthToGive)
        {
            healthAmount += healthToGive;
            if (healthAmount > maxHealth) healthAmount = maxHealth;
            PlayerHealthDelegate?.Invoke(healthAmount);
        }
        
    }
}
