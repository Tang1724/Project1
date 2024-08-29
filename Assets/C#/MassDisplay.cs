using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MassDisplay : MonoBehaviour
{
    public Floor floor;
    public Text massText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        massText.text = floor.totalMass.ToString() + "/" + floor.Mass.ToString();
    }
}
