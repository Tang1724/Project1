using UnityEngine;

public class LiftPresentation : MonoBehaviour
{
    [SerializeField] private Transform platform;
    [SerializeField] private Transform[] suspensionPoints;
    [SerializeField] private SpriteRenderer[] chains;
    [SerializeField] private float platformTopOffset;

    private void LateUpdate()
    {
        if (platform == null)
            return;

        for (int i = 0; i < chains.Length; i++)
        {
            Vector3 top = suspensionPoints[i].position;
            float bottom = platform.position.y + platformTopOffset;
            float length = Mathf.Max(0f, top.y - bottom);
            chains[i].transform.position = new Vector3(top.x, bottom + length * 0.5f, top.z);
            chains[i].size = new Vector2(chains[i].size.x, length);
            chains[i].enabled = length > 0f;
        }
    }
}
