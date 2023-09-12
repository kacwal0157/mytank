using UnityEngine;

[System.Serializable]
public class TargetDefinition
{
    // Replace these enums with your own custom types.
    public enum TargetTypes : int
    {
        None = 0,
        Type_1,
        Type_2,
        Type_3,
        Type_4
    }

    [SerializeField]
    private TargetTypes targetType;
    public TargetTypes TargetType
    {
        get { return targetType; }
    }

    [SerializeField]
    private GameObject targetPrefab;
    public GameObject TargetPrefab
    {
        get { return targetPrefab; }
    }
}
