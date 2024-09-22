using UnityEngine;

public class CiscoFanCompartmentSlot : FanCompartmentSlot
{
    [Header("Cisco FC Slot Attributes")]
    public LED FanLED;
    public override void RunOKLEDS()
    {
        base.RunOKLEDS();
        FanLED.setGreen();
    }

    public override void RunFaultyLEDS()
    {
        base.RunFaultyLEDS();
        FanLED.setRed();
    }

    public override void StopLEDS()
    {
        base.StopLEDS();
        FanLED.setOff();
    }
}
