using UnityEngine;

public class PositionInterpolator : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;
    [SerializeField] private Transform relativeTo;

    public void Interpolate(float _t)
    {
        Vector3 p;

        p = relativeTo ? Vector3.LerpUnclamped(relativeTo.TransformPoint(@from), relativeTo.TransformPoint(to), _t) : Vector3.LerpUnclamped(@from, to, _t);
        
        body.MovePosition(p);
    }
}