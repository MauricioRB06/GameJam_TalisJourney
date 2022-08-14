
// Copyright (c) 2022 MauricioRB06 <https://github.com/MauricioRB06>
// MIT License < Please Read LICENSE.md >
// Collaborators: @barret50cal3011 @DanielaCaO @Kradyn
// 
//  The Purpose Of This Script Is:
//
//  It contains the functions to play audio that will be called from the component of each button.
//
//  -----------------------------
// Last Update: 14/08/2022 By MauricioRB06

using UnityEngine;

namespace UI
{
    // Component required for script to work.
    [RequireComponent(typeof(AudioSource))]
    
    public class AudioUI : MonoBehaviour
    {
        
        [Header("Audio")]
        [Space(5)]
        [Tooltip("The audio clip to play when the button is pressed.")]
        [SerializeField] private AudioClip buttonHover;
        [Tooltip("The audio clip to play when the button is pressed.")]
        [SerializeField] private AudioClip buttonClick;
        
        // Reference to the AudioSource of the user interface.
        private AudioSource _audioSourceUI;
        
        // Initial Settings.
        private void Awake()
        { 
            _audioSourceUI = GetComponent<AudioSource>();
        }
        
        // Plays a sound when called.
        public void UIButtonHover()
        {
            _audioSourceUI.PlayOneShot(buttonHover);
        }
        
        // Plays a sound when called.
        public void UIButtonClick()
        {
            _audioSourceUI.PlayOneShot(buttonClick);
        }
        
    }
}
