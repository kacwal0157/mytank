using UnityEngine;

public class ProjectileArc : MonoBehaviour 
{
    [SerializeField]
    int iterations = 25;

    [SerializeField]
    Color errorColor;

    [HideInInspector]
    public Vector3 endPoint;

    private Color initialColor;
    private LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        initialColor = lineRenderer.material.color;
        //UpdateArc(10,10, Physics.gravity.magnitude, 45 * Mathf.Deg2Rad, new Vector3(5,5,5), true);
    }

    public void UpdateArc(float speed, float distance, float gravity, float angle, Vector3 direction, bool valid)
    {
        Vector2[] arcPoints = ProjectileMath.ProjectileArcPoints(iterations, speed, distance, gravity, angle);        
        Vector3[] points3d = new Vector3[arcPoints.Length];

        for (int i = 0; i < arcPoints.Length; i++)
        {
            points3d[i] = new Vector3(0, arcPoints[i].y, arcPoints[i].x);
        }

        endPoint = points3d[points3d.Length - 1];
        lineRenderer.positionCount = arcPoints.Length;
        lineRenderer.SetPositions(points3d);

        transform.rotation = Quaternion.LookRotation(direction);

        lineRenderer.material.color = valid ? initialColor : errorColor;
    }
}
