using UnityEngine;

public class ACOLT : PushableButton
{
    public bool pressed;
    [SerializeField] JuniperSwitch device;
    [SerializeField] LED[] craftInterfaceLEDs;

    public void PressButton()
    {
        transform.localPosition = onPosition;
        if (device.operating)
        {
            pressed = true;
            foreach (LED led in craftInterfaceLEDs)
            {
                led.setDefault();
            }
        }
    }
    public void ReleaseButton()
    {
        transform.localPosition = offPosition;
        if (device.operating)
        {
            pressed = false;
            foreach (LED led in craftInterfaceLEDs)
            {
                led.returnToPrevious();
            }
            device.majorAlarmLED.setOff();
            device.minorAlarmLED.setOff();
        }
    }
}
