using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightSwitch : Inractable
{
    public Light m_light;
    public bool isOn;
    private void Start()
    {
        UpdateLight();
    }

    void UpdateLight()
    {
        m_light.enabled = isOn;
    }

    public override string GetDescription()
    {
        if (isOn) return "Press [E] to turn <color=red>off</color> the light.";
        return "Press [E] to turn <color=green>on</color> the light.";
    }

    public override void Interact()
    {
        isOn = !isOn;
        UpdateLight();
    }
}
