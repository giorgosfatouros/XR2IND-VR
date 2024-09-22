using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ResetPosition : MonoBehaviour
{
	public List<Collider> outsideColliders = new();
	public List<Collider> insideColliders = new();
	Collider lastExitCollider;
	Vector3 initialPosition;
	Quaternion initialRotation;
	Rigidbody rb;
	public ResetCableManager cableManager;
	public bool ignoreCollisions = false;
	public bool isGrabbed = false;
	bool shouldResetPosition = false;
	void Awake()
	{
		initialPosition = transform.position;
		initialRotation = transform.rotation;
		if (TryGetComponent(out XRGrabInteractable grabInteractable))
		{
			grabInteractable.selectEntered.AddListener(OnGrab);
			grabInteractable.selectExited.AddListener(OnRelease);
		}
		rb = GetComponent<Rigidbody>();
	}

	private void OnGrab(SelectEnterEventArgs args)
	{
		isGrabbed = true;
	}

	private void OnRelease(SelectExitEventArgs args)
	{
		isGrabbed = false;
		if (gameObject.activeSelf) StartCoroutine(ReleaseCoroutine());
	}

	IEnumerator ReleaseCoroutine()
	{
		yield return new WaitForFixedUpdate();
		if (shouldResetPosition)
		{
			Reset();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (insideColliders.Contains(other) && !ignoreCollisions)
		{
			shouldResetPosition = true;
			lastExitCollider = other;
		}
	}
	void OnTriggerEnter(Collider other)
	{
		if (lastExitCollider == other && !ignoreCollisions)
		{
			shouldResetPosition = false;
		}
	}

	private void Reset()
	{
		lastExitCollider = null;
		if (cableManager != null) cableManager.ResetJoints();
		else ResetTransform();
	}

	public void ResetTransform()
	{
		transform.position = initialPosition;
		transform.rotation = initialRotation;
		rb.velocity = new(0, 0, 0);
		shouldResetPosition = false;
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (outsideColliders.Contains(collision.collider) && !ignoreCollisions) shouldResetPosition = true;
	}

	private void Update()
	{
		if (shouldResetPosition && !isGrabbed) Reset();
	}
}
