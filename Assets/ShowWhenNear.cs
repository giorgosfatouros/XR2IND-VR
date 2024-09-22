using System.Collections.Generic;
using UnityEngine;

public class ShowWhenNear : MonoBehaviour
{
	[SerializeField] Collider trigger;
	[SerializeField] List<GameObject> itemsToShow;
	[SerializeField] Camera playerCamera;
	[SerializeField] float distance;
	private void Start()
	{
		if (playerCamera == null) playerCamera = FindFirstObjectByType<Camera>();
		if (playerCamera == null) Debug.LogError("Could not find XROrigin in scene");

	}
	private void Update()
	{
		bool SetActive;
		var distance = (playerCamera.transform.position - transform.position).magnitude;
		SetActive = distance < 3;
		foreach (var item in itemsToShow)
		{
			if (item.activeInHierarchy != SetActive)
			{
				item.SetActive(SetActive);
			}
		}
	}
}
