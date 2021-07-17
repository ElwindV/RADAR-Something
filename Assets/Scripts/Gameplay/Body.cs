using JetBrains.Annotations;
using UnityEngine;

namespace Gameplay
{
    public class Body : MonoBehaviour
    {
        [SerializeField] private GameObject hair;
        [SerializeField] private GameObject scarf;
        [SerializeField] private GameObject chassis;
        [SerializeField] private GameObject leftArm;
        [SerializeField] private GameObject rightArm;

        [CanBeNull] private Gun _leftGun;
        [CanBeNull] private Gun _rightGun;

        public void Start()
        {
            _leftGun = leftArm.GetComponent<Gun>();
            _rightGun = rightArm.GetComponent<Gun>();
        }

        public void SetBodyPart(Bodypart part, GameObject prefab)
        {
            var partObject = part switch
            {
                Bodypart.Hair => hair,
                Bodypart.Scarf => scarf,
                Bodypart.Chassis => chassis,
                Bodypart.LeftArm => leftArm,
                Bodypart.RightArm => rightArm,
                _ => null,
            };

            if (partObject == null) return;
        
            Destroy(partObject);

            var newPart = Instantiate(prefab, transform, false);
            newPart.transform.localPosition = Vector3.zero;

            switch (part)
            {
                case Bodypart.Hair:
                    hair = newPart;
                    break;
                case Bodypart.Scarf:
                    scarf = newPart;
                    break;
                case Bodypart.Chassis:
                    chassis = newPart;
                    break;
                case Bodypart.LeftArm:
                    leftArm = newPart;
                    _leftGun = leftArm.GetComponent<Gun>();
                    break;
                case Bodypart.RightArm:
                    rightArm = newPart;
                    _rightGun = rightArm.GetComponent<Gun>();
                    break;
                default: break;
            }
        }

        public void LeftArmAction()
        {
            if (_leftGun != null) _leftGun.Fire();
        }

        public void RightArmAction()
        {
            if (_rightGun != null) _rightGun.Fire();
        }
    }
}