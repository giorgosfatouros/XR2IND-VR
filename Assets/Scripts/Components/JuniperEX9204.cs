using System.Linq;

public class JuniperEX9204 : JuniperSwitch
{
    bool MajorAlarm()
    {
        if (masterRE == null || masterRE.faulty)
        {
            return true;
        }
        foreach (FanCompartmentSlot fcs in FanCompartmentSlots)
        {
            if (fcs.connectedFC == null || !fcs.connectedFC.lockedInPlace)
            {
                return true;
            }
        }
        return false;
    }

    bool MinorAlarm()
    {
        foreach (FanCompartmentSlot fcs in FanCompartmentSlots)
        {
            if (fcs.connectedFC != null && fcs.connectedFC.lockedInPlace && fcs.connectedFC.faulty)
            {
                return true;
            }
        }

        foreach (JuniperPSUSlot psuSlot in PSUSlots.Cast<JuniperPSUSlot>())
        {
            if (psuSlot.connectedPSU != null && psuSlot.connectedPSU.faulty)
            {
                return true;
            }
        }

        return false;
    }

    public override void CheckForAlarms()
    {
        if (operating)
        {
            if (MajorAlarm())
            {
                majorAlarmLED.setDefault();
            }
            else
            {
                majorAlarmLED.setOff();
            }
            if (MinorAlarm())
            {
                minorAlarmLED.setDefault();
            }
            else
            {
                minorAlarmLED.setOff();
            }
        }
    }

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
}
