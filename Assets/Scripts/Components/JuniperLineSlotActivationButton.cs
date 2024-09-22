using UnityEngine;


public class JuniperLineSlotActivationButton : PushableButton
{
	[Header("LineSlotActivationButton Attributes")]
	public JuniperLineSlot controllingSlot;
	public Switch device;

	public override void Start()
	{
		device = GetComponentInParent<Switch>();
		controllingSlot.controlledBy = this;
	}
	public virtual void Update()
	{
		if (!device.operating)
		{
			controllingSlot.on = false;
		}
		else if (!on && controllingSlot.on)
		{
			StartCoroutine(controllingSlot.TurnOffRoutine());
		}
		else if (on && device.operating && !controllingSlot.on)
		{
			StartCoroutine(controllingSlot.BootRoutine());
		}
	}
	public override void FlipSwitch()
	{
		base.FlipSwitch();
		if (on)
		{
			if (device.operating)
			{
				StartCoroutine(controllingSlot.BootRoutine());
			}
		}
		else
		{
			controllingSlot.on = false;
			if (device.operating)
			{
				StartCoroutine(controllingSlot.TurnOffRoutine());
			}
		}
	}

}
