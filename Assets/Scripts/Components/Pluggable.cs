using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class Pluggable : XRGrabInteractable
{
	[Header("Pluggable Attributes")]
	public Socket isConnectedTo;
	public Module isConnectedToModule;
	public Device isConnectedToDevice;
	public Pluggable otherEnd;
	public bool isCable;
	public bool faulty = false;

	private void Start()
	{
		isCable = GetComponent<DataSocket>() == null;
	}

	public bool IsOtherEndConnected
	{
		get
		{
			return isCable && otherEnd.isConnectedTo != null;
		}
	}
}
