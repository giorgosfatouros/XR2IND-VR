using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Socket : XRSocketInteractor
{
	[Header("Socket Attributes")]
	public Module module;
	public bool faulty;
	public bool healthyConnection = false;
	public bool isConnected = false;
	public Pluggable connectedPluggable;


	public virtual void OnPlugConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		connectedPluggable = selectEnterEventArgs.interactableObject.transform.GetComponent<Pluggable>();
		connectedPluggable.isConnectedTo = this;
		if (module != null)
		{
			connectedPluggable.isConnectedToDevice = module.device;
			connectedPluggable.isConnectedToModule = module;
		}
		isConnected = true;
	}

	public virtual void OnPlugDisconnection()
	{
		connectedPluggable.isConnectedTo = null;
		connectedPluggable.isConnectedToModule = null;
		connectedPluggable.isConnectedToDevice = null;
		connectedPluggable = null;
		isConnected = false;
	}

	public bool IsTerminalSocket
	{
		get
		{
			if (GetComponentInParent<Module>() != null)
			{
				return true;
			}
			return false;
		}

	}

	public virtual void Update() { }
}
