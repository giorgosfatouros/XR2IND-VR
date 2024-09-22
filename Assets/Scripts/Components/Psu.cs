using System.Collections;
using UnityEngine;

public class PSU : Module
{
	[Header("PSU Attributes")]
	public Socket powerSocket;
	public PushableButton powerSwitch;
	public LED inputOkLED;
	public IEnumerator activeRoutine = null;
	public override IEnumerator BootRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
		operating = true;
	}

	public bool IsConnectedToPower()
	{
		if (powerSocket != null)
		{
			if (powerSocket.connectedPluggable != null)
			{
				if (powerSocket.connectedPluggable.otherEnd.isConnectedToDevice is PowerSupplier)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsLockedInSlot()
	{
		return lockedInPlace;
	}
	public bool IsTurnedOn()
	{
		return powerSwitch.on;
	}

	public bool IsReady()
	{
		return IsConnectedToPower() && IsLockedInSlot();
	}

	public bool ShouldBeOperating()
	{
		return IsReady() && IsTurnedOn();
	}

	public override void Update()
	{
		base.Update();
		if (IsConnectedToPower()) inputOkLED.setGreen();
		else inputOkLED.setOff();

		if (ShouldBeOperating() && !operating)
		{
			operating = true;
			activeRoutine = BootRoutine();
			StartCoroutine(activeRoutine);
		}
		else if (!ShouldBeOperating() && operating)
		{
			operating = false;
			if (activeRoutine != null) StopCoroutine(activeRoutine);
			TurnOff();
		}

	}

	public virtual void TurnOff() { }
}