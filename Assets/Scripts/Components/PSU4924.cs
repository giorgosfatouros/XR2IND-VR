using System.Collections;
using UnityEngine;

public class PSU4924 : PSU
{
    [Header("ME4924PSU Attributes")]
    [SerializeField] LED outputOkLED;
    [SerializeField] Fan PSUFan;

    public override void Update()
    {
        base.Update();
    }

    public override IEnumerator BootRoutine()
    {
        yield return new WaitForSeconds(bootWaitingTime);
        if (faulty)
        {
            outputOkLED.setOff();
            PSUFan.setOff();
        }
        else
        {
            outputOkLED.setGreen();
            PSUFan.setOn();
        }

    }

    public override void TurnOff()
    {
        base.TurnOff();
        outputOkLED.setOff();
        PSUFan.setOff();
    }

    public override IEnumerator TurnOffRoutine()
    {
        operating = false;
        yield return new WaitForSeconds(0.01f);
        inputOkLED.setOff();
        outputOkLED.setOff();
        PSUFan.setOff();
        slot.GetComponent<CiscoPSUSlot>().CraftInterfaceLED.setOff();
        operating = false;
    }
}
