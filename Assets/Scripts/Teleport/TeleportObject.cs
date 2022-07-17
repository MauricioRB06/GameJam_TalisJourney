
using System;
using System.Collections;
using Player;
using UnityEngine;

namespace Teleport
{
    // Components required for this Script to work.
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CircleCollider2D))]
    
    public class TeleportObject: MonoBehaviour
    {
        [Header("Destination Settings")] [Space(5)]
        [Tooltip("Sets the destination to which the player will be transported")]
        [SerializeField] private GameObject destination;
        [Space(15)]
        
        [Header("Teleport Settings")] [Space(5)]
        [Tooltip("Sets whether the teleport is active or not")]
        [SerializeField] private bool teleportIsEnable = true;
        [Tooltip("Sets the teleport reuse time")]
        [Range(5.0f, 15.0f)] [SerializeField] private float reactivateTime = 10.0f;
        [Tooltip("Sets whether teleports are destroyed when used")]
        [SerializeField] private bool destroyWhenUsed;
        [Space(15)]
        
        [Header("SFX Settings")] [Space(5)]
        [Tooltip("Sound played constantly near the teleport")]
        [SerializeField] private GameObject sfxTeleport;
        [Tooltip("The sound is played when we enter the teleport")]
        [SerializeField] private GameObject sfxTeleportIn;
        [Tooltip("The sound is played when we exit the teleport")]
        [SerializeField] private GameObject sfxTeleportOut;
        
        // To control the activation of the teleport.
        private CircleCollider2D _teleportCollider;
        
        // Reactivates the teleport after being used.
        private IEnumerator ReactivateTeleport()
        {
            yield return new WaitForSeconds(reactivateTime);
            _teleportCollider.enabled = true;
        }
        
        // Coroutine that makes the player's sprites appear smoothly and Teleports the player.
        private IEnumerator PlayerFadeIn(SpriteRenderer playerSpriteRenderer, Color playerSpriteColor)
        {
            var teleportTransform = transform;
            
            for (var playerSpriteAlpha = 0.0f; playerSpriteAlpha <= 1.0f; playerSpriteAlpha += 0.1f)
            {
                playerSpriteColor.a = playerSpriteAlpha;
                playerSpriteRenderer.color = playerSpriteColor;
                yield return new WaitForSeconds(0.05f);
                
                if (Math.Abs(playerSpriteAlpha - 0.7f) < 0.1f)
                {
                    if (sfxTeleportOut != null)
                    {
                        Instantiate(sfxTeleportOut, teleportTransform.position, Quaternion.identity, teleportTransform);
                    }
                }
            }
            
            playerSpriteColor.a = 1;
            playerSpriteRenderer.color = playerSpriteColor;
            
            if (sfxTeleportOut != null)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
        }
        
        // Coroutine that makes the sprites of player disappear smoothly.
        private IEnumerator PlayerFadeOut(SpriteRenderer playerSpriteRenderer, Color playerSpriteColor, GameObject player)
        {
            for (var playerSpriteAlpha = 1.0f; playerSpriteAlpha >= 0.0f; playerSpriteAlpha -= 0.1f)
            {
                playerSpriteColor.a = playerSpriteAlpha;
                playerSpriteRenderer.color = playerSpriteColor;
                yield return new WaitForSeconds(0.05f);
            }
            
            playerSpriteColor.a = 0;
            playerSpriteRenderer.color = playerSpriteColor;
            
            StartCoroutine(PlayerFadeIn(playerSpriteRenderer, playerSpriteColor));
            
            if (sfxTeleport == null && sfxTeleportIn != null)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            else if (sfxTeleportIn != null)
            {
                Destroy(transform.GetChild(1).gameObject);
            }
            
            player.transform.position = destination.transform.position;
            player.GetComponent<PlayerController>().SetVelocityZero();
        }
        
        // Set the initial configuration for the teleport and check the necessary components.
        private void Awake()
        {
            if (destination == null)
            {
                Debug.LogError("<color=#D22323><b>" +
                               "The teleport destination cannot be empty, please add one</b></color>");
            }
            else
            {
                _teleportCollider = GetComponent<CircleCollider2D>();
                if (!_teleportCollider.isTrigger) _teleportCollider.isTrigger = true;
                
                var teleportTransform = transform;
                
                if (sfxTeleport != null)
                {
                    Instantiate(sfxTeleport, teleportTransform.position, Quaternion.identity, teleportTransform);
                }
            }
        }
        
        // Check if it is active, if it collided with the character and if it should be destroyed after being used.
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!teleportIsEnable) return;
            if (!collision.gameObject.CompareTag("Player")) return;

            var playerSpriteRenderer = collision.GetComponent<SpriteRenderer>();
            var playerSpriteColor = playerSpriteRenderer.color;
            var teleportTransform = transform;

            if (sfxTeleportIn != null)
            {
                Instantiate(sfxTeleportIn, teleportTransform.position, Quaternion.identity, teleportTransform);
            }
            
            StartCoroutine(PlayerFadeOut(playerSpriteRenderer, playerSpriteColor, collision.gameObject));
            
            collision.GetComponent<PlayerController>().SetVelocityZero();
                
            if (destroyWhenUsed)
            {
                destination.GetComponent<TeleportObject>().DestroyTeleport();
                Destroy(gameObject, 5.0f);
            }
            else
            {
                destination.GetComponent<TeleportObject>().TeleportUsed();
            }
        }
        
        // If the teleport was used, it initiates its restoration.
        private void TeleportUsed()
        {
            _teleportCollider.enabled = false;
            StartCoroutine(ReactivateTeleport());
        }
        
        // If the teleport must be destroyed after use.
        private void DestroyTeleport()
        {
            _teleportCollider.enabled = false;
            Destroy(gameObject, 5.0f);
        }
        
        // Allows you to activate or deactivate the teleport.
        public void ChangeTeleportState()
        {
            teleportIsEnable = !teleportIsEnable;
        }
    }
}
