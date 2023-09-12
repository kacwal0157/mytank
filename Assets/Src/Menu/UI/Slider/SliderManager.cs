using UnityEngine;

public class SliderManager : MonoBehaviour
{
    public bool useRelativeRotation = true;

    private Quaternion relativeRotation;

    private void Start()
    {
        relativeRotation = transform.localRotation;
    }

    private void Update()
    {
        if (useRelativeRotation == true)
            transform.rotation = relativeRotation;
    }
}
