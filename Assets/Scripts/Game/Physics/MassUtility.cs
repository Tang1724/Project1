using UnityEngine;

public static class MassUtility
{
    public static float CalculateTotalMass(GameObject target)
    {
        float mass = 0f;
        foreach (var provider in target.GetComponents<IMassProvider>())
            mass += provider.CurrentMass;

        foreach (Transform child in target.transform)
            mass += CalculateTotalMass(child.gameObject);

        return mass;
    }
}
