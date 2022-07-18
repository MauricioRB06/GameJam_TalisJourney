using UnityEngine;

namespace Items
{
    public class LevelSpawnPosition : MonoBehaviour
    {
        private GameObject _player;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _player.GetComponent<Transform>().position = transform.position;
            Destroy(gameObject);
        }

    }
}
