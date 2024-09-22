using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : Device
{
	public IEnumerator activeRoutine = null;
	public List<PSUSlot> PSUSlots;
	public bool operating;
	public bool connectedToPower = false;
	public float bootWaitingTime = 5.0f;


	public virtual IEnumerator BootRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
		operating = true;
	}

	public virtual void Update()
	{
		bool shouldBeOperating = false;
		foreach (PSUSlot psuSlot in PSUSlots)
		{
			if (psuSlot.connectedPSU != null && psuSlot.connectedPSU.operating)
			{
				shouldBeOperating = true;
			}
		}

		connectedToPower = shouldBeOperating;

		if (!connectedToPower && operating)
		{
			operating = false;
			if (activeRoutine != null)
			{
				StopCoroutine(activeRoutine);
				activeRoutine = null;

			}
			TurnOff();

		}
		else if (connectedToPower && !operating)
		{
			operating = true;
			activeRoutine = BootRoutine();
			StartCoroutine(activeRoutine);
		}
	}

	public virtual void Start() { }
	public virtual void RunDiagnostics() { }
	public virtual void TurnOff() { }
	public virtual void CheckForAlarms() { }
}
