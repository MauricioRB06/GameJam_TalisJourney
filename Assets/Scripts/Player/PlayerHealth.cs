using System;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float healthAmount;
        [SerializeField] private float maxHealth;

        public static event Action<float> PlayerHealthDelegate;

        // 
        private bool _playerIsDead;
        
        // 
        private void Start()
        {
            healthAmount = maxHealth;
        }
        
        // 
        private void Dead()
        {
            GetComponent<PlayerController>().ChangeAttackState();
            GetComponent<PlayerController>().ChangeControllerState();
            GetComponent<PlayerController>().Dead();
        }
        
        // 
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
        
        // 
        public void CureHealth (float healthToGive)
        {
            healthAmount += healthToGive;
            if (healthAmount > maxHealth) healthAmount = maxHealth;
            PlayerHealthDelegate?.Invoke(healthAmount);
        }
        
    }
}
