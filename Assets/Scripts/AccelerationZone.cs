using UnityEngine;

public class AccelerationZone : MonoBehaviour
{
    [SerializeField] [Min(0.0f)] private float acceleration = 10.0f;
    [SerializeField] [Min(0.0f)] private float speed = 10.0f;

    void Accelerate(Rigidbody _body)
    {
        Vector3 velocity = transform.InverseTransformDirection(_body.velocity);

        if (velocity.y >= speed)
        {
            return;
        }

        velocity.y = acceleration > 0.0f ? Mathf.MoveTowards(velocity.y, speed, acceleration * Time.deltaTime) : speed;
        
        _body.velocity = transform.TransformDirection(velocity);

        if (_body.TryGetComponent(out MovingSphere sphere))
        {
            sphere.PreventSnapToGround();
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        Rigidbody body = _other.attachedRigidbody;

        if (body)
        {
            Accelerate(body);
        }
    }

    void OnTriggerStay(Collider _other)
    {
        Rigidbody body = _other.attachedRigidbody;

        if (body)
        {
            Accelerate(body);
        }
    }
}