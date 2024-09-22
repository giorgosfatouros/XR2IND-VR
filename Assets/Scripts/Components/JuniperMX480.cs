using System.Collections;
public class JuniperMX480 : JuniperSwitch
{
	public override void Start()
	{
		base.Start();
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

	public override IEnumerator BootRoutine()
	{
		return base.BootRoutine();
	}

	public override void TurnOff()
	{
		base.TurnOff();
		minorAlarmLED.setOff();
		majorAlarmLED.setOff();
	}


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
		foreach (JuniperPSUSlot psuSlot in PSUSlots)
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


}
