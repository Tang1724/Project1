using MFramework.Event;
using UnityEngine;

[RequireComponent(typeof(Bottle), typeof(SpriteRenderer))]
public class BottlePresenter : MonoBehaviour, IEventReceiver<BottleStateChangedEvent>
{
    private Bottle bottle;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer vesselRenderer;
    [SerializeField] private SpriteRenderer waterIndicator;

    private void Awake()
    {
        bottle = GetComponent<Bottle>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<BottleStateChangedEvent>(this);
        ShowState(bottle.CurrentState);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<BottleStateChangedEvent>(this);
    }

    public void OnEvent(BottleStateChangedEvent evt)
    {
        if (evt.Bottle == bottle)
            ShowState(evt.State);
    }

    private void ShowState(Bottle.BottleState state)
    {
        if (vesselRenderer != null)
        {
            vesselRenderer.color = state == Bottle.BottleState.Full ? new Color(0.45f, 1.2f, 1.65f) :
                                   state == Bottle.BottleState.Fly ? new Color(1.55f, 1.18f, 0.45f) : new Color(0.83f, 0.77f, 0.70f);
            if (waterIndicator != null)
            {
                waterIndicator.enabled = state != Bottle.BottleState.Empty;
                waterIndicator.color = state == Bottle.BottleState.Fly ? new Color(1f, 0.86f, 0.46f) : new Color(0.56f, 0.88f, 1f);
            }
            return;
        }

        spriteRenderer.color = state == Bottle.BottleState.Full ? Color.white :
                               state == Bottle.BottleState.Empty ? Color.gray : Color.yellow;
    }
}
