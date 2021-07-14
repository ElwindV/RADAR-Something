using System;
using Audio;
using UnityEngine;

namespace Items
{
    public class BodyPickup : MonoBehaviour
    {
        [SerializeField] private Bodypart bodypart;
        [SerializeField] private GameObject prefab;

        [Header("Audio")] 
        [SerializeField] private AudioCueEventChannelSO audioEventChannel;
        [SerializeField] private AudioCueSO pickupSound;

        public void OnTriggerEnter(Collider other)
        {
            if (! other.CompareTag("Player")) return;

            var body = other.GetComponent<Body>();
            
            body.SetBodyPart(bodypart, prefab);
            
            audioEventChannel.RaiseEvent(pickupSound);
            
            gameObject.SetActive(false);
        }
    }
}
