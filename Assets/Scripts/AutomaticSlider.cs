using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnValueChangedEvent : UnityEvent<float>
{
    
}

public class AutomaticSlider : MonoBehaviour
{
    [SerializeField] [Min(0.01f)] private float duration = 1.0f;
    [SerializeField] private bool autoReverse;
    [SerializeField] private bool smoothStep;
    [SerializeField] private OnValueChangedEvent onValueChanged;

    private float value;

    private float SmoothedValue => 3.0f * value * value - 2.0f * value * value * value;

    public bool Reversed
    {
        get;
        set;
    }

    public bool AutoReverse
    {
        get => autoReverse;
        set => autoReverse = value;
    }

    void FixedUpdate()
    {
        float delta = Time.deltaTime / duration;

        if (Reversed)
        {
            value -= delta;

            if (value <= 0.0f)
            {
                if (autoReverse)
                {
                    value = Mathf.Min(1.0f, -value);
                    Reversed = false;
                }
                else
                {
                    value = 0.0f;
                    enabled = false;
                }
            }
        }
        else
        {
            value += delta;

            if (value >= 1.0f)
            {
                if (autoReverse)
                {
                    value = Mathf.Max(0.0f, 2.0f - value);
                    Reversed = true;
                }
                else
                {
                    value = 1.0f;
                    enabled = false;
                }
            }
        }

        onValueChanged.Invoke(smoothStep ? SmoothedValue : value);
    }
}