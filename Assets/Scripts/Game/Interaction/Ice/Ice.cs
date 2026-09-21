using System.Collections;
using UnityEngine;

public class Ice : MonoBehaviour, IMassProvider
{
    public float currentMass = 1f;
    [SerializeField] private float meltDuration = 3f;
    public float CurrentMass => currentMass;
    private bool melting;
    private Renderer[] renderers;
    private Color[] originalColors;
    private int[] colorProperties;
    private MaterialPropertyBlock propertyBlock;
    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        renderers = GetComponentsInChildren<Renderer>(true);
        originalColors = new Color[renderers.Length];
        colorProperties = new int[renderers.Length];
        propertyBlock = new MaterialPropertyBlock();
        for (int i = 0; i < renderers.Length; i++)
        {
            Material material = renderers[i].sharedMaterial;
            int property = material.HasProperty("_Color") ? Shader.PropertyToID("_Color") :
                material.HasProperty("_TintColor") ? Shader.PropertyToID("_TintColor") : -1;
            colorProperties[i] = property;
            if (property != -1)
                originalColors[i] = material.GetColor(property);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!melting && collision.CompareTag("Empty"))
        {
            melting = true;
            StartCoroutine(DisableAndDestroy());
        }
    }

    IEnumerator DisableAndDestroy()
    {
        float elapsed = 0f;
        while (elapsed < meltDuration)
        {
            elapsed += Time.deltaTime;
            float opacity = 1f - Mathf.Clamp01(elapsed / meltDuration);
            for (int i = 0; i < renderers.Length; i++)
            {
                // 旧粒子材质没有颜色属性，生成闪点按自身的短寿命自然结束。
                if (colorProperties[i] == -1)
                    continue;
                Color color = originalColors[i];
                color.a *= opacity;
                // 用实例属性同步淡出棱面和裂纹，不改共享材质或碰撞尺寸。
                renderers[i].GetPropertyBlock(propertyBlock);
                propertyBlock.SetColor(colorProperties[i], color);
                renderers[i].SetPropertyBlock(propertyBlock);
            }
            yield return null;
        }
        body.simulated = false;
        Destroy(gameObject);
    }
}
