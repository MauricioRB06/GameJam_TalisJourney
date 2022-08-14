
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  Sets the behavior of the Final Boss user interface.
//
//  Documentation and References:
//
//  Unity OnEnable: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnEnable.html
//  Unity OnDisable: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnDisable.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using Enemies;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HUDBoss : MonoBehaviour
    {
        
        [Header("References")]
        [Tooltip("The HUDBoss's health bar.")]
        [SerializeField] private Image healthBar;
        
        // Updates the life bar in the interface.
        private void BossState(float healthAmount)
        {
            healthBar.fillAmount = healthAmount / 1000;
            
            if(healthBar.fillAmount <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
        
        // Subscribes the update function to the delegate who reports on the life of the boss.
        private void OnEnable()
        {
            FinalBoss.BossHealthDelegate += BossState;
        }
        
        // Unsubscribes the update function to the delegate who reports on the life of the boss.
        private void OnDisable()
        {
            FinalBoss.BossHealthDelegate -= BossState;
        }
        
    }
}
