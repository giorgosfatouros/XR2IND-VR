using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
	public float rotationSpeed = 0f;
	public float offSpeed = 0f;
	public float onSpeed = 8f;
	public float faultSpeed = 3f;

	public bool isFaulty = false;
	// Start is called before the first frame update
	void Start()
	{

	}

	public void setOn()
	{
		if (isFaulty)
		{
			rotationSpeed = faultSpeed;
		}
		else
		{
			rotationSpeed = onSpeed;
		}

	}

	public void setFaulty()
	{
		rotationSpeed = faultSpeed;
	}

	public void setOff()
	{
		rotationSpeed = offSpeed;
	}

	// Update is called once per frame
	void Update()
	{
		transform.Rotate(0f, 0f, rotationSpeed);
	}
}
