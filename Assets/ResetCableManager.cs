using System.Collections;
using System.Collections.Generic;
using HPhysic;
using Unity.VisualScripting;
using UnityEngine;

public class ResetCableManager : MonoBehaviour
{
	[SerializeField] List<Rigidbody> childrenRb = new();
	[SerializeField] List<ResetPosition> childrenReset = new();
	public Collider boxCollider;
	public Collider floorCollider;
	void Awake()
	{
		StartCoroutine(InitializationCoroutine());
	}

	IEnumerator InitializationCoroutine()
	{
		while (boxCollider == null || floorCollider == null)
		{
			Debug.Log("Waiting for parameters to be initialized");
			yield return null;
		}
		var childrenRbArray = GetComponentsInChildren<Rigidbody>();
		childrenRb.AddRange(childrenRbArray);
		foreach (Rigidbody jointRb in childrenRb)
		{

			if (jointRb.GetComponent<ResetPosition>() != null) Destroy(jointRb.GetComponent<ResetPosition>());
			ResetPosition resetPosition = jointRb.AddComponent<ResetPosition>();
			resetPosition.ignoreCollisions = resetPosition.GetComponent<Pluggable>() == null;
			childrenReset.Add(resetPosition);
			resetPosition.cableManager = this;
			if (!resetPosition.insideColliders.Contains(boxCollider)) resetPosition.insideColliders.Add(boxCollider);
			if (!resetPosition.outsideColliders.Contains(floorCollider)) resetPosition.outsideColliders.Add(floorCollider);
		}

		PhysicCable physicCable = GetComponent<PhysicCable>();


		if (physicCable.start.TryGetComponent(out Pluggable startPluggable))
		{
			if (startPluggable.GetComponent<ResetPosition>() != null) Destroy(startPluggable.GetComponent<ResetPosition>());
			ResetPosition startResetPosition = startPluggable.AddComponent<ResetPosition>();
			startResetPosition.cableManager = this;
			if (!childrenReset.Contains(startResetPosition)) childrenReset.Add(startResetPosition);
			if (startResetPosition.TryGetComponent(out Rigidbody startRb) && !childrenRb.Contains(startRb)) childrenRb.Add(startRb);
			if (!startResetPosition.insideColliders.Contains(boxCollider)) startResetPosition.insideColliders.Add(boxCollider);
			if (!startResetPosition.outsideColliders.Contains(floorCollider)) startResetPosition.outsideColliders.Add(floorCollider);
		}
		if (physicCable.end.TryGetComponent(out Pluggable stopPluggable))
		{
			if (stopPluggable.GetComponent<ResetPosition>() != null) Destroy(stopPluggable.GetComponent<ResetPosition>());
			ResetPosition stopResetPosition = stopPluggable.AddComponent<ResetPosition>();
			if (!childrenReset.Contains(stopResetPosition)) childrenReset.Add(stopResetPosition);
			stopResetPosition.cableManager = this;
			if (stopResetPosition.TryGetComponent(out Rigidbody stopRb) && !childrenRb.Contains(stopRb)) childrenRb.Add(stopRb);
			if (!stopResetPosition.insideColliders.Contains(boxCollider)) stopResetPosition.insideColliders.Add(boxCollider);
			if (!stopResetPosition.outsideColliders.Contains(floorCollider)) stopResetPosition.outsideColliders.Add(floorCollider);
		}
	}

	public void ResetJoints()
	{
		foreach (var child in childrenReset) if (child.isGrabbed) return;

		foreach (var child in childrenReset)
		{
			child.ResetTransform();
		}
	}

}
