using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PSUSlot : Slot
{
	[Header("PSU Slot Attributes")]
	public PSU connectedPSU;

	public override void OnModuleConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		base.OnModuleConnection(selectEnterEventArgs);
		connectedPSU = connectedModule.GetComponent<PSU>();
	}

	public override void OnModuleDisconnection(SelectExitEventArgs selectEnterEventArgs)
	{
		base.OnModuleDisconnection(selectEnterEventArgs);
		connectedPSU = null;
	}
}
