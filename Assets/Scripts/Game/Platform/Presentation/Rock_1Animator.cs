using MFramework.Event;
using UnityEngine;

public class Rock_1Animator : MonoBehaviour, IEventReceiver<PlatformMassChangedEvent>
{
    private Animator anim;
    private Floor floor;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        floor = GetComponent<Floor>();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<PlatformMassChangedEvent>(this);
        anim.SetFloat("Mass", floor.TotalMass);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlatformMassChangedEvent>(this);
    }

    public void OnEvent(PlatformMassChangedEvent evt)
    {
        if (evt.Platform == floor)
            anim.SetFloat("Mass", evt.TotalMass);
    }
}
