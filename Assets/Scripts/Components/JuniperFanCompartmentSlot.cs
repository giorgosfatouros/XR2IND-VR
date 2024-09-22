using UnityEngine;

public class JuniperFanCompartmentSlot : FanCompartmentSlot
{
    [Header("Juniper FC Slot Attributes")]
    public LED CraftInterfaceOk;
    public LED CraftInterfaceFail;

    public override void RunOKLEDS()
    {
        base.RunOKLEDS();
        CraftInterfaceOk.setGreen();
    }

    public override void RunFaultyLEDS()
    {
        base.RunFaultyLEDS();
        CraftInterfaceFail.setRed();
    }

    public override void StopLEDS()
    {
        base.StopLEDS();
        CraftInterfaceOk.setOff();
        CraftInterfaceFail.setOff();
    }
}
