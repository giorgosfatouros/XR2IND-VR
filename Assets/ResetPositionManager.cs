using System.Collections;
using System.Collections.Generic;
using HPhysic;
using Unity.VisualScripting;
using UnityEngine;

public class ResetPositionManager : MonoBehaviour
{
	[SerializeField] List<Transform> objPositions = new();
	[SerializeField] BoxCollider floorCollider;
	[SerializeField] List<PhysicCable> cables;
	Transform parent;
	void Start()
	{
		parent = transform.parent;
		StartCoroutine(TrackItemsRoutine(1.5f));
	}

	IEnumerator TrackItemsRoutine(float delay)
	{
		yield return new WaitForSeconds(delay);

		if (TryGetComponent(out BoxCollider boxCollider))
		{

			if (TryGetComponent(out CompartmentAttacher compartmentAttacher))
			{
				foreach (var psu in compartmentAttacher.pSUs) if (!objPositions.Contains(psu.transform)) objPositions.Add(psu.transform);
				foreach (var fc in compartmentAttacher.fanCompartments) if (!objPositions.Contains(fc.transform)) objPositions.Add(fc.transform);
				foreach (var lc in compartmentAttacher.juniperLineCards) if (!objPositions.Contains(lc.transform)) objPositions.Add(lc.transform);
				foreach (var re in compartmentAttacher.juniperRoutingEngines) if (!objPositions.Contains(re.transform)) objPositions.Add(re.transform);
				foreach (var pluggable in compartmentAttacher.pluggables) if (!objPositions.Contains(pluggable.transform)) objPositions.Add(pluggable.transform);
			}

			foreach (var cable in cables)
			{
				if (cable.GetComponent<ResetCableManager>() != null) Destroy(cable.GetComponent<ResetCableManager>());
				ResetCableManager cableManager = cable.AddComponent<ResetCableManager>();

				cableManager.boxCollider = boxCollider;
				cableManager.floorCollider = floorCollider;
			}

			Vector3 center = boxCollider.bounds.center;
			Vector3 halfExtents = boxCollider.bounds.extents;
			Collider[] colliders = Physics.OverlapBox(center, halfExtents, transform.rotation);

			foreach (Collider collider in colliders)
			{
				if (collider.transform.parent == parent && !objPositions.Contains(collider.transform)) objPositions.Add(collider.transform);
			}

			foreach (var obj in objPositions)
			{
				if (obj.gameObject != gameObject && obj.GetComponent<Rigidbody>() != null)
				{
					obj.gameObject.AddComponent<ResetPosition>();
					obj.GetComponent<ResetPosition>().insideColliders.Add(boxCollider);
					obj.GetComponent<ResetPosition>().outsideColliders.Add(floorCollider);
				}
			}

			Debug.Log("ResetPosition components added");
		}
	}



}
