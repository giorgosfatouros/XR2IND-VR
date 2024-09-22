using System.Collections;
using UnityEngine;

public class PSU9204 : PSU
{
	[Header("EX9204PSU Attributes")]
	[SerializeField] LED outputOkLED;
	[SerializeField] LED failLED;
	[SerializeField] Fan PSUFan;

	public override void Update()
	{
		base.Update();
	}

	public override void TurnOff()
	{
		base.TurnOff();
		outputOkLED.setOff();
		PSUFan.setOff();
		failLED.setOff();
	}
	public override IEnumerator BootRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
		inputOkLED.setGreen();
		if (faulty)
		{
			outputOkLED.setOff();
			failLED.setRed();
			PSUFan.setOff();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setOff();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setRed();
		}
		else
		{
			outputOkLED.setGreen();
			failLED.setOff();
			PSUFan.setOn();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setGreen();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setOff();
			if (!device.operating)
			{
				StartCoroutine(device.BootRoutine());
			}
		}
		operating = true;
	}

	public override IEnumerator TurnOffRoutine()
	{
		operating = false;
		yield return new WaitForSeconds(0.01f);
		inputOkLED.setOff();
		outputOkLED.setOff();
		failLED.setOff();
		PSUFan.setOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setOff();
		operating = false;
	}
}
