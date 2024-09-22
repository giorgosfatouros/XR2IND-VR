using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class screwdriverTip : MonoBehaviour
{

	[SerializeField] List<InputActionReference> llmActions = new();
	[SerializeField] InputActionReference leftChangeScrewMode;
	[SerializeField] InputActionReference rightChangeScrewMode;
	[SerializeField] InputActionReference leftTriggerPressed;
	[SerializeField] InputActionReference rightTriggerPressed;
	[SerializeField] Transform trigger;
	public float screwSpeed = 6f;
	private XRInputSubsystem xrInputSubsystem;
	float rotationSpeed = 0f;
	// Start is called before the first frame update

	private void ButtonPressed(InputAction.CallbackContext ctx)
	{
		screwSpeed *= -1;
	}
	private void TriggerPressed(InputAction.CallbackContext ctx)
	{
		trigger.localPosition = new Vector3(0.000760000024f, -0.000220000002f, 0.0f);
		rotationSpeed = screwSpeed;
	}
	private void TriggerReleased(InputAction.CallbackContext ctx)
	{
		trigger.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
		rotationSpeed = 0.0f;
	}

	public virtual void OnSelected(SelectEnterEventArgs selectEnterEventArgs)
	{
		rotationSpeed = 0.0f;
		trigger.localPosition = new Vector3(0.0f, 0.0f, 0.0f);


		var interactor = selectEnterEventArgs.interactorObject.transform.parent;

		if (interactor.TryGetComponent(out XRInteractionGroup xRInteractionGroup))
		{
			if (xRInteractionGroup.groupName == "Left")
			{
				foreach (var actionRef in llmActions) actionRef.action.Disable();
				leftChangeScrewMode.action.performed += ButtonPressed;
				leftTriggerPressed.action.performed += TriggerPressed;
				leftTriggerPressed.action.canceled += TriggerReleased;
			}
			else if (xRInteractionGroup.groupName == "Right")
			{
				rightChangeScrewMode.action.performed += ButtonPressed;
				rightTriggerPressed.action.performed += TriggerPressed;
				rightTriggerPressed.action.canceled += TriggerReleased;
			}

		}
		else
		{
		}
	}

	public virtual void OnReleased(SelectExitEventArgs selectExitEventArgs)
	{
		rotationSpeed = 0.0f;
		var interactor = selectExitEventArgs.interactorObject.transform.parent;

		if (interactor.TryGetComponent(out XRInteractionGroup xRInteractionGroup))
		{
			if (xRInteractionGroup.groupName == "Left")
			{
				foreach (var actionRef in llmActions) actionRef.action.Enable();
				leftChangeScrewMode.action.performed -= ButtonPressed;
				leftTriggerPressed.action.performed -= TriggerPressed;
				leftTriggerPressed.action.canceled -= TriggerReleased;
			}
			else if (xRInteractionGroup.groupName == "Right")
			{
				rightChangeScrewMode.action.performed -= ButtonPressed;
				rightTriggerPressed.action.performed -= TriggerPressed;
				rightTriggerPressed.action.canceled -= TriggerReleased;
			}

		}
		else
		{
		}
	}


	// Update is called once per frame
	void Update()
	{
		transform.Rotate(rotationSpeed, 0f, 0f);
	}

	void OnTriggerStay(Collider other)
	{
		if (other.TryGetComponent(out Screw screw))
		{
			if (!screw.screwed && rotationSpeed < 0f) screw.ScrewIt();
			else if (screw.screwed && rotationSpeed > 0f) screw.UnscrewIt();

		}
	}
}
