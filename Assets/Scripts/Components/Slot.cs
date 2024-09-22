using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Slot : XRSocketInteractor
{
	[Header("Slot Attributes")]
	public Transform attachmentPoint;
	public Switch device;
	public Module connectedModule;
	public PushableButton controlledBy;
	public Transform start;
	public Transform stop;
	private float slideDuration = 0.2f;
	public bool faulty;
	public bool on;

	protected override void Start()
	{
		base.Start();
		selectEntered.AddListener(OnModuleConnection);
		selectExited.AddListener(OnModuleDisconnection);
		device = GetComponentInParent<Switch>();
		if (attachmentPoint == null) attachmentPoint = transform.Find("AttachmentPoint");
		if (start == null) start = transform.Find("Start");
		if (stop == null) stop = transform.Find("Stop");
	}

	public virtual void OnModuleConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		attachmentPoint.localPosition = start.localPosition;
		connectedModule = selectEnterEventArgs.interactableObject.transform.GetComponent<Module>();
		connectedModule.slot = this;
		connectedModule.device = device;
		StartCoroutine(SlideInRoutine(connectedModule));
	}

	public virtual void OnModuleDisconnection(SelectExitEventArgs selectEnterEventArgs)
	{
		connectedModule.slot = null;
		connectedModule.device = null;
		connectedModule = null;
	}

	public IEnumerator SlideInRoutine(Module module)
	{
		module.GetComponent<Collider>().enabled = false;
		float slidingDistance = (stop.localPosition - start.localPosition).magnitude;
		var slidingDirection = (stop.localPosition - start.localPosition).normalized;
		float step = slidingDistance * Time.deltaTime / slideDuration;
		attachmentPoint.localPosition = start.localPosition;

		while ((attachmentPoint.localPosition - stop.localPosition).magnitude > 1.1 * step)
		{
			attachmentPoint.localPosition += slidingDirection * step;
			yield return null;
		}

		attachmentPoint.localPosition = stop.localPosition;
		module.GetComponent<Collider>().enabled = true;

	}

}
