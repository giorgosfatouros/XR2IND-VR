using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FanCompartmentSlot : Slot
{
	[Header("FanCompartment Slot Attributes")]
	public FanCompartment connectedFC;

	public override void OnModuleConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		base.OnModuleConnection(selectEnterEventArgs);
		connectedFC = connectedModule.GetComponent<FanCompartment>();
	}

	public override void OnModuleDisconnection(SelectExitEventArgs selectEnterEventArgs)
	{
		base.OnModuleDisconnection(selectEnterEventArgs);
		connectedFC = null;
	}

	public virtual void RunOKLEDS() { }

	public virtual void RunFaultyLEDS() { }

	public virtual void StopLEDS() { }
}
