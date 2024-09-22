using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

// Mostly unsused for the moment
public class PushableButton : XRSimpleInteractable
{
	[Header("PushableButton Attributes")]
	public Vector3 offPosition;
	public Vector3 onPosition;
	[SerializeField] Vector3 offRotation;
	[SerializeField] Vector3 onRotation;
	[SerializeField] UnityEvent invokeMethod;
	public bool on = false;

	public virtual void Start()
	{
		transform.localPosition = on ? onPosition : offPosition;
		transform.localRotation = on ? Quaternion.Euler(onRotation) : Quaternion.Euler(offRotation);
	}

	public virtual void FlipSwitch()
	{
		on = !on;
		transform.localPosition = on ? onPosition : offPosition;
		transform.localRotation = on ? Quaternion.Euler(onRotation) : Quaternion.Euler(offRotation);
		invokeMethod?.Invoke();
	}

	public virtual void SetSwitch(bool _on)
	{
		if (_on != on) FlipSwitch();
	}

}
