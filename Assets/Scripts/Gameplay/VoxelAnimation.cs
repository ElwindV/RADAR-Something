using System;
using UnityEngine;

namespace Gameplay
{
    public class VoxelAnimation : MonoBehaviour
    {
        public GameObject[] frames;
        private int _index = 0;
        private int _maxFrames = 0;

        private float _counter = 0f;
        private readonly float _rate = .07f;

        public void Start()
        {
            for (var i = 1; i < frames.Length; i++)
            {
                frames[i].SetActive(false);
            }
            
            _maxFrames = frames.Length;
        }

        public void FixedUpdate()
        {
            _counter += Time.fixedDeltaTime;

            if (! (_counter > _rate)) return;
            _counter = 0;
            Increment();

        }

        private void Increment()
        {
            if (_index + 1 > _maxFrames - 1) return;
            
            frames[_index].SetActive(false);
            _index++;
            frames[_index].SetActive(true);
        }
    }
}
