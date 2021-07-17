using Audio;
using Gameplay;
using UnityEngine;

namespace Items
{
    public class MakeOverPickup : MonoBehaviour
    {
        [SerializeField] private GameObject[] hairPrefabs;
        [SerializeField] private GameObject[] scarfPrefabs;
        [SerializeField] private GameObject[] chassisPrefabs;

        [Header("Audio")] 
        [SerializeField] private AudioCueEventChannelSO audioEventChannel;
        [SerializeField] private AudioCueSO pickupSound;

        public void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            var body = other.GetComponent<Body>();

            if (hairPrefabs.Length > 0)
            {
                var hairPrefab = hairPrefabs[Random.Range(0, hairPrefabs.Length - 1)];
                body.SetBodyPart(Bodypart.Hair, hairPrefab);
            }
            
            if (scarfPrefabs.Length > 0)
            {
                var scarfPrefab = scarfPrefabs[Random.Range(0, scarfPrefabs.Length - 1)];
                body.SetBodyPart(Bodypart.Scarf, scarfPrefab);
            }
            
            if (chassisPrefabs.Length > 0)
            {
                var chassisPrefab = chassisPrefabs[Random.Range(0, chassisPrefabs.Length - 1)];
                body.SetBodyPart(Bodypart.Chassis, chassisPrefab);
            }
            
            audioEventChannel.RaiseEvent(pickupSound);

            gameObject.SetActive(false);
        }
    }
}