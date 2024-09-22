using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniperLineCard : Module
{
	[Header("JuniperLineCard Attributes")]
	public LED statusLed;
	public LED micLed;
	[SerializeField] List<DataSocket> dataSockets;


	public override void Start()
	{
		base.Start();
	}


	public override IEnumerator BootRoutine()
	{
		statusLed.startGreenBlink();
		if (micLed) micLed.startGreenBlink();
		yield return new WaitForSeconds(bootWaitingTime);
		if (operating) SetOn();
		else SetOff();
		operating = true;
	}

	public override IEnumerator TurnOffRoutine()
	{
		statusLed.startGreenBlink();
		if (micLed) micLed.startGreenBlink();
		yield return new WaitForSeconds(bootWaitingTime);
		SetOff();
		operating = false;
	}

	public void SetOn()
	{
		if (!faulty)
		{
			statusLed.setGreen();
			if (micLed) micLed.setGreen();
		}
		else
		{
			statusLed.setRed();
			if (micLed) micLed.setRed();
		}
		foreach (DataSocket dataSocket in dataSockets)
		{

			dataSocket.UpdateRelevantSockets();
			dataSocket.UpdateMe();
		}

	}

	public void SetOff()
	{
		statusLed.setOff();
		if (micLed) micLed.setOff();
		operating = false;
		foreach (DataSocket dataSocket in dataSockets)
		{
			dataSocket.UpdateRelevantSockets();
			dataSocket.UpdateMe();
		}
	}


	public override void Update()
	{
		base.Update();
		if (lockedInPlace && slot != null && slot.on && !operating)
		{
			StartCoroutine(BootRoutine());
		}
		if (!lockedInPlace || slot == null || !slot.on)
		{
			SetOff();
		}
	}

}
