using UnityEngine;

namespace UI
{
    public class PlayerDestroyer : MonoBehaviour
    {
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            
            if(_player != null)
            {
                Destroy(_player);
            }
        }

    }
}