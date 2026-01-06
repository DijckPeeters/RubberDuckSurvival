using UnityEngine;
using UnityEngine.UI;

public class XPBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxXP(int xp)
    {
        slider.maxValue = xp;
        slider.value = 0; // XP begint altijd bij 0 als je levelt
    }

    public void SetXP(int xp)
    {
        slider.value = xp;
    }
}