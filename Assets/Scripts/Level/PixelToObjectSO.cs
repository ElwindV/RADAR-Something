using JetBrains.Annotations;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Level/PixelToObject", order = 1)]
    public class PixelToObjectSO : ScriptableObject
    {
        public Color inputColor;
        public GameObject[] prefabs;
        
        public PixelToObjectSO floor;

        public void SpawnObject(Vector3 position, Transform parentTransform)
        {
            if (prefabs.Length < 1) return;

            var index = Random.Range(0, prefabs.Length);
            var outputObject = prefabs[index];

            var gameObject = Instantiate(outputObject,  position, Quaternion.identity);
            gameObject.name = name;

            gameObject.transform.parent = parentTransform;

            if (floor != null) floor.SpawnObject(position, gameObject.transform);
        }
        
    }
}
