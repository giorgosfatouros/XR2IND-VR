using System.Collections.Generic;
using UnityEngine;

public class FanCompartment : Module
{
	public List<Fan> Fans;
	public override void Start()
	{
		base.Start();
		foreach (Fan fan in Fans)
		{
			if (fan.isFaulty)
			{
				faulty = true;
			}
		}
	}

	public void Begin()
	{
		Debug.Log("Fan Compartment Begin");
		if (faulty)
		{
			foreach (Fan fan in Fans) fan.isFaulty = true;
		}
		bool allOk = true;
		foreach (Fan fan in Fans)
		{
			if (fan.isFaulty)
			{
				allOk = false;
			}
			fan.setOn();
		}
		if (allOk) slot.GetComponent<FanCompartmentSlot>().RunOKLEDS();
		else slot.GetComponent<FanCompartmentSlot>().RunFaultyLEDS();
	}
	public void Stop()
	{
		Debug.Log("Fan Compartment Stop");
		slot.GetComponent<FanCompartmentSlot>().StopLEDS();
		foreach (Fan fan in Fans)
		{
			fan.setOff();
		}
	}

	public override void Update()
	{
		base.Update();
		if (lockedInPlace && slot.device.connectedToPower)
		{

			if (!operating)
			{
				operating = true;

				Begin();
			}
		}
		else
		{
			if (operating)
			{
				operating = false;
				Stop();
			}
		}
	}

}