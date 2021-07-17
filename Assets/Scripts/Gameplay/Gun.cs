using Audio;
using UnityEngine;

namespace Gameplay
{
    public class Gun : MonoBehaviour
    {
        [SerializeField] 
        private Transform bulletSpawnPoint;
        
        [SerializeField] 
        [Range(0,2)] 
        private float shootDelay = 1f;

        [SerializeField] 
        private uint bulletsPerShot = 1;

        [SerializeField]
        private float spread = 0f;

        [SerializeField]
        private GameObject bulletPrefab;
        
        private float _lastFireTime = float.NegativeInfinity;
        
        [Header("Audio")] 
        public AudioCueEventChannelSO eventChannelSO;
        public AudioCueSO shootSfx;

        public void Fire()
        {
            if (! CanFire()) return;
            
            eventChannelSO.RaiseEvent(shootSfx);
            _lastFireTime = Time.time;

            for (var i = 0; i < bulletsPerShot; i++)
            {
                var projectile = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                projectile.transform.Rotate(Vector3.up * Random.Range(-spread / 2f, spread / 2f));
                Destroy(projectile, 5f);
            }
        }

        private bool CanFire() 
        {
            return (_lastFireTime + shootDelay) < Time.time;
        }
    }
}
