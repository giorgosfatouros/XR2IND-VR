using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Module : XRGrabInteractable
{
	[Header("Module Attributes")]
	public Switch device;
	public Slot slot;
	public float bootWaitingTime;
	public bool faulty;
	public bool operating;
	public bool poweredOn;
	public bool lockedInPlace;
	public bool grabbable = true;
	public List<Screw> lockingScrews;


	public virtual void Start() { }

	public virtual IEnumerator BootRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
	}
	public virtual IEnumerator TurnOffRoutine()
	{
		yield return new WaitForSeconds(bootWaitingTime);
	}

	public void DeactivateGrabbable()
	{
		base.interactionLayers ^= InteractionLayerMask.GetMask("Default");
		grabbable = false;
		GetComponent<Rigidbody>().isKinematic = true;
		device.CheckForAlarms();
		device.RunDiagnostics();
	}

	public void ActivateGrabbable()
	{
		base.interactionLayers |= InteractionLayerMask.GetMask("Default");
		grabbable = true;
		device.CheckForAlarms();
		device.RunDiagnostics();
		GetComponent<Rigidbody>().isKinematic = false;
	}

	public virtual void Update()
	{
		if (lockingScrews.Count > 0)
		{
			lockedInPlace = false;
			if (slot != null)
			{
				foreach (Screw lockingScrew in lockingScrews)
				{
					if (lockingScrew.screwed) lockedInPlace = true;
				}
			}
		}

		if (lockedInPlace && grabbable) DeactivateGrabbable();
		else if (!lockedInPlace && !grabbable) ActivateGrabbable();

	}
}
