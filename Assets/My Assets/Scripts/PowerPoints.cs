using UnityEngine;
using UnityEngine.UI;

public class PowerPoints : MonoBehaviour
{
    public Slider power;
    float maxPower = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        power.maxValue = maxPower; 
        power.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerPoint()
    {
        float value = power.value;
        value += 1;
        power.value = value;

        if(power.value >= maxPower)
        {
            maxPower += 20;
            power.maxValue = maxPower;
            power.value = 0;
        }
    }
}
