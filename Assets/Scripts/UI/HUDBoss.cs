
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

        private void BossState(float healthAmount)
        {
            healthBar.fillAmount = healthAmount / 1000;
            
            if(healthBar.fillAmount <= 0.0f)
            {
                Destroy(gameObject);
            }
        }
        
        private void OnEnable()
        {
            FinalBoss.BossHealthDelegate += BossState;
        }
        
        private void OnDisable()
        {
            FinalBoss.BossHealthDelegate -= BossState;
        }
        
    }
}
