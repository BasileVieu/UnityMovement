using UnityEngine;

public class GravitySphere : GravitySource
{
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] [Min(0.0f)] private float innerFallOffRadius = 1.0f;
    [SerializeField] [Min(0.0f)] private float innerRadius = 5.0f;
    [SerializeField] [Min(0.0f)] private float outerRadius = 10.0f;
    [SerializeField] [Min(0.0f)] private float outerFallOffRadius = 10.0f;

    private float innerFallOffFactor;
    private float outerFallOffFactor;

    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        innerFallOffRadius = Mathf.Max(innerFallOffRadius, 0.0f);
        innerRadius = Mathf.Max(innerRadius, innerFallOffRadius);
        outerRadius = Mathf.Max(outerRadius, innerRadius);
        outerFallOffRadius = Mathf.Max(outerFallOffRadius, outerRadius);
        
        innerFallOffFactor = 1.0f / (innerRadius - innerFallOffRadius);
        outerFallOffFactor = 1.0f / (outerFallOffRadius - outerRadius);
    }

    public override Vector3 GetGravity(Vector3 _position)
    {
        Vector3 vector = transform.position - _position;

        float distance = vector.magnitude;

        if (distance > outerFallOffRadius
            || distance < innerFallOffRadius)
        {
            return Vector3.zero;
        }

        float g = gravity / distance;

        if (distance > outerRadius)
        {
            g *= 1.0f - (distance - outerRadius) * outerFallOffFactor;
        }
        else if (distance < innerRadius)
        {
            g *= 1.0f - (innerRadius - distance) * innerFallOffFactor;
        }

        return g * vector;
    }

    private void OnDrawGizmos()
    {
        Vector3 p = transform.position;

        if (innerFallOffRadius > 0.0f
            && innerFallOffRadius < innerRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, innerFallOffRadius);
        }

        Gizmos.color = Color.yellow;

        if (innerRadius > 0.0f
            && innerRadius < outerRadius)
        {
            Gizmos.DrawWireSphere(p, innerRadius);
        }
        
        Gizmos.DrawWireSphere(p, outerRadius);

        if (outerFallOffRadius > outerRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, outerFallOffRadius);
        }
    }
}