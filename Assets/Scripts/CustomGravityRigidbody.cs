using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour
{
    [SerializeField] private bool floatToSleep;
    [SerializeField] private float submergenceOffset = 0.5f;
    [SerializeField] [Min(0.1f)] private float submergenceRange = 1.0f;
    [SerializeField] [Min(0.0f)] private float buoyancy = 1.0f;
    [SerializeField] private Vector3 buoyancyOffset = Vector3.zero;
    [SerializeField] [Range(0.0f, 10.0f)] private float waterDrag = 1.0f;
    [SerializeField] private LayerMask waterMask = 0;

    private new Rigidbody body;

    private Vector3 gravity;

    private float floatDelay;
    private float submergence;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }

    void FixedUpdate()
    {
        if (floatToSleep)
        {
            if (body.IsSleeping())
            {
                floatDelay = 0.0f;

                return;
            }

            if (body.velocity.sqrMagnitude < 0.0001f)
            {
                floatDelay += Time.deltaTime;

                if (floatDelay >= 1.0f)
                {
                    return;
                }
            }
            else
            {
                floatDelay = 0.0f;
            }
        }

        gravity = CustomGravity.GetGravity(body.position);

        if (submergence > 0.0f)
        {
            float drag = Mathf.Max(0.0f, 1.0f - waterDrag * submergence * Time.deltaTime);

            body.velocity *= drag;
            body.angularVelocity *= drag;
            body.AddForce(gravity * -(buoyancy * submergence), ForceMode.Acceleration);
            
            submergence = 0.0f;
        }

        body.AddForceAtPosition(gravity, transform.TransformPoint(buoyancyOffset), ForceMode.Acceleration);
    }

    void EvaluateSubmergence()
    {
        Vector3 upAxis = -gravity.normalized;

        if (Physics.Raycast(body.position + upAxis * submergenceOffset, -upAxis, out RaycastHit hit,
                            submergenceRange + 1.0f, waterMask, QueryTriggerInteraction.Collide))
        {
            submergence = 1.0f - hit.distance / submergenceRange;
        }
        else
        {
            submergence = 1.0f;
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        if ((waterMask & (1 << _other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence();
        }
    }

    void OnTriggerStay(Collider _other)
    {
        if (!body.IsSleeping()
            && (waterMask & (1 << _other.gameObject.layer)) != 0)
        {
            EvaluateSubmergence();
        }
    }
}