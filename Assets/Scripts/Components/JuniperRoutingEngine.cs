using System.Collections;
using UnityEngine;

public class JuniperRoutingEngine : Module
{
	[SerializeField] LED sfOkFailLED, onlineLED, reOkFailLED;
	public LED masterLED;
	public PushableButton onlineButton;
	public PushableButton resetButton;
	public bool isMaster;
	public bool isOnline;
	float reBootWaitingTime = 5.0f;

	public override IEnumerator BootRoutine()
	{
		onlineLED.setOff();
		sfOkFailLED.startGreenBlink();
		reOkFailLED.startGreenBlink();
		yield return new WaitForSeconds(reBootWaitingTime);
		SetOn();
		operating = true;
	}

	public override IEnumerator TurnOffRoutine()
	{
		sfOkFailLED.startGreenBlink();
		reOkFailLED.startGreenBlink();
		yield return new WaitForSeconds(reBootWaitingTime);
		SetOff();
	}

	public void SetOn()
	{
		if (!faulty)
		{
			sfOkFailLED.setGreen();
			reOkFailLED.setGreen();
		}
		else
		{
			sfOkFailLED.setRed();
			reOkFailLED.setRed();
		}
		if (isOnline)
		{
			onlineLED.setDefault();
		}
	}

	public void SetOff()
	{
		sfOkFailLED.setOff();
		reOkFailLED.setOff();
		masterLED.setOff();
		onlineLED.setOff();
		operating = false;
	}

	public void OnlineButtonPressed()
	{
		StartCoroutine(OnOffRE());
	}

	public void ResetButtonPressed()
	{
		StartCoroutine(ResetRE());
	}

	public IEnumerator ResetRE()
	{
		onlineLED.startGreenBlink();
		yield return new WaitForSeconds(reBootWaitingTime);
		if (isOnline)
		{
			onlineLED.setDefault();
		}
		else
		{
			onlineLED.setOff();
		}
	}

	public IEnumerator OnOffRE()
	{
		onlineLED.startGreenBlink();
		yield return new WaitForSeconds(reBootWaitingTime);
		isOnline = !isOnline;
		if (isOnline)
		{
			onlineLED.setDefault();
		}
		else
		{
			onlineLED.setOff();
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
