using UnityEngine;

namespace UI
{
    // 
    [RequireComponent(typeof(AudioSource))]
    
    public class AudioUI : MonoBehaviour
    {
        [Header("Audio")]
        [Space(5)]
        [Tooltip("The audio clip to play when the button is pressed.")]
        [SerializeField] private AudioClip buttonHover;
        [Tooltip("The audio clip to play when the button is pressed.")]
        [SerializeField] private AudioClip buttonClick;

        private AudioSource _audioSourceUI;
        
        private void Awake()
        { 
            _audioSourceUI = GetComponent<AudioSource>();
        }
        
        public void UIButtonHover()
        {
            _audioSourceUI.PlayOneShot(buttonHover);
        }

        public void UIButtonClick()
        {
            _audioSourceUI.PlayOneShot(buttonClick);
        }
        
    }
}
