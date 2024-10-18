using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlowAnimator : MonoBehaviour
{
    public TMP_Text textMesh;      // TextMeshPro组件
    public float glowIntensity = 2.0f;  // 发光强度
    public Color glowColor = Color.cyan;  // 发光颜色
    public float moveAmplitude = 0.5f;  // 移动幅度
    public float frequency = 1.0f;  // 移动频率

    private Material textMaterial;  // 文本材质
    private Vector3 originalPosition;  // 初始位置

    void Start()
    {
        if (textMesh == null)
        {
            Debug.LogError("TextMesh component not assigned.");
            return;
        }

        // 保存初始位置
        originalPosition = textMesh.transform.position;
        // 获取材质的副本以修改参数
        textMaterial = textMesh.fontMaterial;
    }

    void Update()
    {
        // 计算基于时间的波动位置
        float yOffset = moveAmplitude * Mathf.Sin(Time.time * frequency * 2 * Mathf.PI);
        textMesh.transform.position = originalPosition + new Vector3(0, yOffset, 0);

        // 设置发光颜色和强度
        textMaterial.SetColor("_GlowColor", glowColor);
        textMaterial.SetFloat("_GlowPower", glowIntensity * Mathf.Abs(yOffset));
    }
}