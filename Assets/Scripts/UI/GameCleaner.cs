
using Player;
using UnityEngine;

namespace UI
{
    public class GameCleaner : MonoBehaviour
    {
        private GameObject _player;
        private GameObject _hud;

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
