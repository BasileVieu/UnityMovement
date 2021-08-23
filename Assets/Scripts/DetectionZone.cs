using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class DetectionZone : MonoBehaviour
{
    [SerializeField] private UnityEvent onFirstEnter;
    [SerializeField] private UnityEvent onLastExit;
    
    List<Collider> colliders = new List<Collider>();

    void Awake()
    {
        enabled = false;
    }

    void FixedUpdate()
    {
        for (var i = 0; i < colliders.Count; i++)
        {
            Collider tempCollider = colliders[i];

            if (!tempCollider
                || !tempCollider.gameObject.activeInHierarchy)
            {
                colliders.RemoveAt(i--);

                if (colliders.Count == 0)
                {
                    onLastExit.Invoke();

                    enabled = false;
                }
            }
        }
    }

    void OnTriggerEnter(Collider _other)
    {
        if (colliders.Count == 0)
        {
            onFirstEnter.Invoke();

            enabled = true;
        }

        colliders.Add(_other);
    }

    void OnTriggerExit(Collider _other)
    {
        if (colliders.Remove(_other)
            && colliders.Count == 0)
        {
            onLastExit.Invoke();

            enabled = false;
        }
    }

    void OnDisable()
    {
        #if UNITY_EDITOR
        if (enabled
            && gameObject.activeInHierarchy)
        {
            return;
        }
        #endif
        
        if (colliders.Count > 0)
        {
            colliders.Clear();
            onLastExit.Invoke();
        }
    }
}