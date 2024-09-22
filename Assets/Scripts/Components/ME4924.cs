using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ME4924 : Switch
{
    [SerializeField] LED statusLED;
    public List<LED> PSULEDs;
    public FanCompartmentSlot FCSlot;
    public bool faultyPSU;
    public bool faultyFan;

    public override IEnumerator BootRoutine()
    {
        statusLED.startAmberBlink();
        yield return new WaitForSeconds(bootWaitingTime);
        SetLEDS();
        activeRoutine = null;
    }

    public override void RunDiagnostics()
    {
        base.RunDiagnostics();
        if (operating)
        {
            activeRoutine = BootRoutine();
            StartCoroutine(activeRoutine);

        }
    }

    public override void Update()
    {
        base.Update();

        bool _faultyPSU = false;
        bool _faultyFan = false;

        for (int i = 0; i < PSUSlots.Count; i++)
        {
            PSU psu = PSUSlots[i].connectedPSU;
            LED psuLED = PSULEDs[i];

            if (psu == null) psuLED.setOff();
            else
            {

                bool poweredButNotActivated = psu.IsReady() && !psu.operating;
                bool faultyAndRunning = psu.faulty && psu.operating;

                if (poweredButNotActivated || faultyAndRunning)
                {
                    psuLED.setRed();
                    if (faultyAndRunning) _faultyPSU = true;
                }
                else if (psu.operating && !psu.faulty) psuLED.setGreen();
                else psuLED.setOff();
            }
        }

        if (FCSlot.connectedFC == null || FCSlot.connectedFC.faulty || !FCSlot.connectedFC.operating) _faultyFan = true;

        faultyPSU = _faultyPSU;
        faultyFan = _faultyFan;

    }

    public void SetLEDS()
    {
        if (faultyFan || faulty) statusLED.setRed();
        else if (faultyPSU) statusLED.setAmber();
        else statusLED.setGreen();
    }

    public override void TurnOff()
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        statusLED.setOff();
    }


}
