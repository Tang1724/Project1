using MFramework.Event;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MassDisplay : MonoBehaviour, IEventReceiver<PlatformMassChangedEvent>
{
    public Floor floor;
    public Text massText;
    public TMP_Text worldMassText;

    private void Awake()
    {
        if (floor == null)
            floor = GetComponentInParent<Floor>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlatformMassChangedEvent>(this);
        if (floor != null)
            ShowMass(floor.TotalMass, floor.RequiredMass);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlatformMassChangedEvent>(this);
    }

    public void OnEvent(PlatformMassChangedEvent evt)
    {
        if (evt.Platform == floor)
            ShowMass(evt.TotalMass, evt.RequiredMass);
    }

    private void ShowMass(float mass, float requiredMass)
    {
        string value = mass.ToString("0.#") + "/" + requiredMass.ToString("0.#");
        if (massText != null)
            massText.text = value;
        if (worldMassText != null)
        {
            worldMassText.text = value;
            worldMassText.color = mass >= requiredMass ? new Color(0.48f, 0.90f, 0.78f) : new Color(0.96f, 0.86f, 0.65f);
        }
    }

    private void LateUpdate()
    {
        if (worldMassText == null || transform.parent == null)
            return;

        // 电梯模型使用不同的非等比缩放，数字牌保持世界尺寸，避免文字被压扁。
        Vector3 scale = transform.parent.lossyScale;
        if (Mathf.Approximately(scale.x, 0f) || Mathf.Approximately(scale.y, 0f) || Mathf.Approximately(scale.z, 0f))
            return;
        transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
    }
}
