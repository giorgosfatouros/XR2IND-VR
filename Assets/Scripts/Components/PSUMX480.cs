using System.Collections;
using UnityEngine;

public class PSUMX480 : PSU
{
	[Header("MX480PSU Attributes")]
	[SerializeField] LED pwrOkLED;
	[SerializeField] LED bkrOnLED;

	public override void Update()
	{
		base.Update();

	}
	public override IEnumerator BootRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
		inputOkLED.setGreen();
		if (faulty)
		{
			pwrOkLED.setGreen();
			bkrOnLED.setRed();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setOff();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setRed();
		}
		else
		{
			pwrOkLED.setOff();
			bkrOnLED.setOff();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setGreen();
			slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setOff();

		}
	}

	public override IEnumerator TurnOffRoutine()
	{
		operating = false;
		yield return new WaitForSeconds(0.01f);
		inputOkLED.setOff();
		pwrOkLED.setOff();
		bkrOnLED.setOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setOff();
		operating = false;
	}

	public override void TurnOff()
	{
		base.TurnOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceOk.setOff();
		slot.GetComponent<JuniperPSUSlot>().CraftInterfaceFail.setOff();
	}
}
