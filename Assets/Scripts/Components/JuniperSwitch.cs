using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JuniperSwitch : Switch
{
    [Header("Juniper Switch Attributes")]
    public List<JuniperLineSlotActivationButton> LineSlotActivationButtons;
    public List<JuniperLineSlot> RoutingEngineSlots;
    public List<JuniperFanCompartmentSlot> FanCompartmentSlots;
    public JuniperLineSlot masterRE;
    public ACOLT acoltButton;
    public LED majorAlarmLED, minorAlarmLED;

    public override void Update()
    {
        base.Update();
        if (!operating)
        {
            masterRE = null;
        }
        else
        {
            if (masterRE == null)
            {
                foreach (JuniperLineSlot RE in RoutingEngineSlots)
                {
                    if (RE.connectedModule && RE.connectedModule.operating && !acoltButton.pressed)
                    {
                        RE.connectedModule.GetComponent<JuniperRoutingEngine>().isMaster = true;
                        RE.connectedModule.GetComponent<JuniperRoutingEngine>().masterLED.setDefault();
                        masterRE = RE;
                        masterRE.gameObject.GetComponent<RoutingEngineLineSlot>().masterLED.setDefault();
                        break;
                    }
                }
            }
            else
            {
                if (masterRE.connectedModule == null || !masterRE.connectedModule.operating)
                {
                    masterRE = null;
                }
            }
        }
    }

    public override IEnumerator BootRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        foreach (JuniperLineSlotActivationButton lineSlotActivationButton in LineSlotActivationButtons)
        {
            if (lineSlotActivationButton.on)
            {
                lineSlotActivationButton.controllingSlot.on = true;
                StartCoroutine(lineSlotActivationButton.controllingSlot.BootRoutine());
            }
        }
        operating = true;
    }
}
