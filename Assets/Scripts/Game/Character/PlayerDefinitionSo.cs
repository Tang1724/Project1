using UnityEngine;

[CreateAssetMenu(menuName = "Game/Player Definition")]
public class PlayerDefinitionSo : ScriptableObject
{
    public float fullspeed = 4f;
    public float emptyspeed = 6f;
    public float flyspeed = 6f;
    public float jumpFullForce = 5f;
    public float jumpEmptyForce = 8f;
    public float FlyForce = 5f;

    public float GetSpeed(PlayerState.State state)
    {
        return state == PlayerState.State.Full ? fullspeed :
               state == PlayerState.State.Empty ? emptyspeed : flyspeed;
    }

    public float GetJumpForce(PlayerState.State state)
    {
        return state == PlayerState.State.Full ? jumpFullForce :
               state == PlayerState.State.Empty ? jumpEmptyForce : FlyForce;
    }
}
