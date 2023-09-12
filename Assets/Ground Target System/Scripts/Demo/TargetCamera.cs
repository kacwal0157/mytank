using UnityEngine;

[RequireComponent(typeof(TargetSelector))]
public class TargetCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    private TargetSelector targetSelector;

    void Awake()
    {
        targetSelector = GetComponent<TargetSelector>();
        if(targetSelector != null)
            targetSelector.OnSelectionChanged += HandleOnSelectionChanged;
    }

    private void HandleOnSelectionChanged(TargetSelection targetSelection)
    {
        SetTarget(targetSelection.gameObject);
    }

    private void SetTarget(GameObject target)
    {
        targetCamera.transform.SetParent(target.transform);
        targetCamera.transform.localPosition = new Vector3(0.0f, 3.0f, -1.0f);
        targetCamera.transform.localRotation = Quaternion.Euler(new Vector3(70.0f, 0.0f, 0.0f));
    }
}
