using System.Collections;
using UnityEngine;

public class JuniperLineSlot : LineSlot
{
	public LineSlotLock leftLock;
	public LineSlotLock rightLock;
	[SerializeField] LED okLED;
	[SerializeField] LED failLED;
	public JuniperLineSlotActivationButton activationButton;

	void CheckIfLocked()
	{
		if (!leftLock.locked && !rightLock.locked)
		{
			locked = false;
		}
		else
		{
			locked = true;
		}
	}


	public virtual IEnumerator BootRoutine()
	{
		if (connectedModule != null && connectedModule.lockedInPlace)
		{
			StartCoroutine(connectedModule.BootRoutine());
		}
		failLED.setOff();
		okLED.startGreenBlink();
		yield return new WaitForSeconds(bootWaitingTime);
		okLED.setOff();
		if (connectedModule != null)
		{
			if (connectedModule.lockedInPlace)
			{
				if (!connectedModule.faulty)
				{
					okLED.setGreen();
					failLED.setOff();
				}
				else
				{
					okLED.setOff();
					failLED.setRed();
				}
			}
		}
		on = true;
		device.CheckForAlarms();
	}

	public virtual IEnumerator TurnOffRoutine()
	{
		if (connectedModule != null && connectedModule.lockedInPlace)
		{
			StartCoroutine(connectedModule.TurnOffRoutine());
		}
		failLED.setOff();
		okLED.startGreenBlink();
		yield return new WaitForSeconds(bootWaitingTime);
		okLED.setOff();
		on = false;
		device.CheckForAlarms();
	}

	public virtual void Update()
	{
		if ((!device.operating || !activationButton.on) && on)
		{
			on = false;
		}
		else if (device.operating && activationButton.on && !on)
		{
			on = true;
		}
		CheckIfLocked();
		if (connectedModule != null)
		{
			if (locked && !connectedModule.lockedInPlace)
			{
				connectedModule.lockedInPlace = true;
				if (activationButton.on && device.operating)
				{
					StartCoroutine(BootRoutine());
				}
			}
			else if (!locked && connectedModule.lockedInPlace)
			{
				connectedModule.lockedInPlace = false;
				okLED.setOff();
				failLED.setOff();
			}
			if (!on && connectedModule.operating)
			{
				// connectedModule.operating = false;
				StartCoroutine(connectedModule.TurnOffRoutine());
				okLED.setOff();
				failLED.setOff();
			}
		}
		else
		{
			if (locked && slotActive)
			{
				DeactivateSlot();
			}
			else if (!locked && !slotActive)
			{
				ActivateSlot();
			}
		}
	}


}
