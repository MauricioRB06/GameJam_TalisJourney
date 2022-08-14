
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
// The Purpose Of This Script Is:
//
//  Sets the behavior of the tutorial's instructional posters.
//
// Documentation and References:
//
//  Unity Awake: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Awake.html
//  Unity OnTriggerEnter2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerEnter2D.html
//  Unity OnTriggerExit2D: https://docs.unity3d.com/ScriptReference/MonoBehaviour.OnTriggerExit2D.html
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;

namespace Items
{
    // Components required for this script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(BoxCollider2D))]
    
    public class MessagePopUp : MonoBehaviour
    {
        
        [Header("Message")] [Space(5)]
        [Tooltip("Place here the object containing the message to be revealed.")]
        [SerializeField] private GameObject message;
        
        // Sets the message's initial state.
        private void Awake()
        { 
            message.SetActive(false);
        }
        
        // When the player enters the sign area, he reveals the message.
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                message.SetActive(true);
            }
        }
        
        // When the player leaves the sign area, he hides the message.
        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                message.SetActive(false);
            }
        }
        
    }
}
