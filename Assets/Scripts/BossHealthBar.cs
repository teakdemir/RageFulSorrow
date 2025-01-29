using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue/maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
