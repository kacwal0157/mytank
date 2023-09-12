using UnityEngine;

[AddComponentMenu("Game Native/Ground Target System/Target Selection")]
public class TargetSelection : MonoBehaviour
{
    public string TargetName { get { return this.name; } }

    [SerializeField] private TargetDefinition.TargetTypes targetType;
    public TargetDefinition.TargetTypes TargetType { get { return targetType; } }
}
