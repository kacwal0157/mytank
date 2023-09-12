using UnityEngine;
using System.Linq;

[AddComponentMenu("Game Native/Ground Target System/Target Selector")]
public class TargetSelector : MonoBehaviour
{
    public delegate void SelectionChanged(TargetSelection targetSelection);
    public event SelectionChanged OnSelectionChanged;

    [SerializeField]
    private TargetDefinition[] targetDefinitions;

    [SerializeField]
    private GameObject defaultTarget;

    [SerializeField]
    private LayerMask clickLayers;

    private GameObject selectionInstance;
    private TargetSelection lastSelection;

    public int DefinitionCount
    {
        get { return targetDefinitions.Length; }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 100.0f, clickLayers))
            {
                TargetSelection targetSelection = hit.transform.GetComponent<TargetSelection>();
                if (targetSelection != null)
                    SetTargetDefinition(targetSelection);
            }
        }
    }

    public void SetTargetDefinition(TargetSelection targetSelection, int index = -1)
    {
        if (targetSelection == null)
            return;

        TargetDefinition targetDefinition = null;

        // Allows for cycling
        if (index > -1 && index < targetDefinitions.Length && targetDefinitions.Length > 0)
            targetDefinition = targetDefinitions[index];
        
        // Destroy current
        if (selectionInstance != null)
            Destroy(selectionInstance);

        // Obtain target type, if available
        if(index < 0)
            targetDefinition = targetDefinitions.Where(x => x.TargetType == targetSelection.TargetType).FirstOrDefault();

        // Apply the default target, if one was not defined.
        if (targetDefinition == null)
        {
            if (defaultTarget != null)
                selectionInstance = Instantiate(defaultTarget);
        }
        else
        {
            if (targetDefinition.TargetPrefab != null)
                selectionInstance = Instantiate(targetDefinition.TargetPrefab);
        }

        // Set the transform within the new parent object.
        if (selectionInstance != null)
        {
            selectionInstance.transform.SetParent(targetSelection.transform);
            selectionInstance.transform.localPosition = new Vector3(0, 10);
            selectionInstance.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        if (lastSelection != targetSelection)
        {
            lastSelection = targetSelection;
            // Notify other scripts of the change.
            if (OnSelectionChanged != null)
                OnSelectionChanged(targetSelection);
        }
    }
}
