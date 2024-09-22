using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;


public class LineSlotLock : XRSimpleInteractable
{
	[Header("LineSlotLock Attributes")]
	[SerializeField] Vector3 unlockRotation;
	[SerializeField] Vector3 lockRotation;
	public bool locked = false;
	public bool unlocking, locking;
	float rotationX;

	public void Start()
	{
		if (locked)
		{
			locking = true;
			unlocking = false;
		}
		else
		{
			unlocking = true;
			locking = false;
		}
	}
	public virtual void LockInteract()
	{
		if (locked)
		{
			unlocking = true;
			locking = false;
		}
		else
		{
			unlocking = false;
			locking = true;
		}
	}

	public void Update()
	{
		rotationX = transform.rotation.eulerAngles.x % 360;

		if (unlocking)
		{
			if (rotationX < unlockRotation.x - 0.1f || rotationX > unlockRotation.x + 0.1f)
			{
				transform.Rotate(0f, -1.0f, 0f);
			}
			else
			{
				unlocking = false;
				locked = false;
			}
		}
		else if (locking)
		{
			if (rotationX < lockRotation.x - 0.1f || rotationX > lockRotation.x + 0.1f)
			{
				transform.Rotate(0f, 1.0f, 0f);
			}
			else
			{
				locking = false;
				locked = true;
			}
		}
	}

}
