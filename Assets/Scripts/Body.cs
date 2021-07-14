using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private GameObject hair;
    
    [SerializeField] private GameObject scarf;
    
    [SerializeField] private GameObject chassis;
    
    [SerializeField] private GameObject leftArm;
    
    [SerializeField] private GameObject rightArm;

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
    }

    public void LeftArmAction()
    {
        
    }

    public void RightArmAction()
    {
        
    }
}