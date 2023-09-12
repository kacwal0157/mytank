using UnityEngine;
using UnityEngine.UI;

public class TargetInformation : MonoBehaviour
{
    [SerializeField] private TargetSelector targetSelector;

    [SerializeField] private Text selectedTarget;
    [SerializeField] private Text targetType;

    private TargetSelection currentTarget;
    private int lastIndex = 0;
	
    void Start()
    {
        if(targetSelector != null)
            targetSelector.OnSelectionChanged += HandleOnSelectionChanged;
    }

    private void HandleOnSelectionChanged(TargetSelection targetSelection)
    {
        if (targetSelection == null)
            return;

        currentTarget = targetSelection;

        // Demo - Switch Index
        lastIndex = (int)targetSelection.TargetType - 1;

        if (selectedTarget != null)
            selectedTarget.text = string.Format("Selected Target: {0}", targetSelection.TargetName);

        if (targetType != null)
            targetType.text = string.Format("Target Type: {0}", (int)targetSelection.TargetType);
    }

    public void NextEffect()
    {
        if (lastIndex < targetSelector.DefinitionCount - 1)
            lastIndex++;
        else
            lastIndex = 0;

        targetSelector.SetTargetDefinition(currentTarget, lastIndex);
    }

    public void PreviousEffect()
    {
        if (lastIndex > 0)
            lastIndex--;
        else
            lastIndex = targetSelector.DefinitionCount - 1;

        targetSelector.SetTargetDefinition(currentTarget, lastIndex);
    }
}
