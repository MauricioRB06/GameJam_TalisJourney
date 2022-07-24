using Cinemachine;
using UnityEngine;

namespace Tools
{
    public class CameraFinder : MonoBehaviour
    {
        private CinemachineVirtualCamera _virtualCamera;
        private GameObject _playerReference;
        private bool _canSeePlayer;

        private void Awake()
        {
            _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
        
        private void Start()
        {
            _playerReference = GameObject.FindWithTag("Player");
            _canSeePlayer = _virtualCamera.Follow;
        }
        
         private void Update() 
         {
             
         if (_canSeePlayer) return; 
         
         _virtualCamera.Follow = _playerReference.transform;
         
         }
    }
}
