using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtUser : MonoBehaviour
{
	private Camera userCamera;
	void Start()
	{
		userCamera = FindFirstObjectByType<Camera>();
	}
	void Update()
	{
		transform.LookAt(userCamera.transform, Vector3.up);
		transform.Rotate(0, 180f, 0);
	}
}
