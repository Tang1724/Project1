using UnityEngine;

public class AmbientMotion : MonoBehaviour
{
    public Vector3 positionAmplitude;
    public float rotationAmplitude;
    public float frequency = 0.7f;
    public float phase;
    [Range(0f, 1f)] public float alphaPulse;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private SpriteRenderer sprite;
    private Color initialColor;

    private void Awake()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        sprite = GetComponent<SpriteRenderer>();
        if (sprite != null)
            initialColor = sprite.color;
    }

    private void Update()
    {
        float wave = Mathf.Sin(Time.time * frequency + phase);
        transform.localPosition = initialPosition + positionAmplitude * wave;
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, rotationAmplitude * wave);
        if (sprite != null && alphaPulse > 0f)
            sprite.color = new Color(initialColor.r, initialColor.g, initialColor.b, initialColor.a * (1f - alphaPulse * (0.5f + 0.5f * wave)));
    }
}
