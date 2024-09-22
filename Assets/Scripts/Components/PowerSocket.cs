using UnityEngine.XR.Interaction.Toolkit;

public class PowerSocket : Socket
{
	public override void OnPlugConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		connectedPluggable = selectEnterEventArgs.interactableObject.transform.GetComponent<Pluggable>();
		connectedPluggable.isConnectedToDevice = FindObjectOfType<PowerSupplier>();
		isConnected = true;
	}
}
