using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum ObjectiveFunction
{
	alwaysTrue, alwaysFalse, trueAfter7Seconds,
	S2PSUInstalled,
	S2FanCompartmentInstalled,
	S2RoutingEngineInstalled,
	S2LineCardInstalled,
	S2DevicePoweredOn,
	S2PortsConnected,
	S1BrokenPartRaired,
	TutorialDesk1GrabModule,
	TutorialDesk1ConnectModule,
	TutorialDesk1LockedModule,
	TutorialDesk2ConnectSfpModule,
	TutorialDesk2LinkRouters,
	TutorialDesk3UnscrewPSU,
	TutorialDesk3GrabScrewdriver,
	TutorialDesk3RemovePSU,
	TutorialDesk4ConnectPSU,
	TutorialDesk4ChangeScrewingMode,
	TutorialDesk4ScrewPsu,
	TutorialDesk4ConnectPowerCable,
	TutorialDesk4PowerPsu
}

public class ScenarioObjectives : MonoBehaviour
{
	[SerializeField] JuniperEX9204 scenario2JuniperEX9204;
	[SerializeField] FanCompartment scenario2FanCompartment;
	[SerializeField] PSU9204 scenario2PSU;
	[SerializeField] JuniperRoutingEngine scenario2RE;
	[SerializeField] JuniperLineCard scenario2LC;
	[SerializeField] SFPSocket scenario2PortA, scenario2PortB;
	[SerializeField] Slot[] moduleSlots;
	[SerializeField] DataSocket[] Cicso1Connection;
	[SerializeField] DataSocket[] Cicso2Connection;
	[SerializeField] DataSocket[] Cicso3Connection;
	[SerializeField] DataSocket[] Cicso4Connection;
	[SerializeField] DataSocket[] Cicso5Connection;

	[Header("Tutorial Desk 1")]
	[SerializeField] JuniperLineCard desk1RoutingEngine;
	[SerializeField] JuniperLineSlot desk1LineSlot;
	[SerializeField] LineSlotLock[] desk1LineSlotLocks;

	[Header("Tutorial Desk 2")]
	[SerializeField] DataPluggable desk2SfpModule;
	[SerializeField] ME4924 desk2Router1;
	[SerializeField] ME4924 desk2Router2;
	[SerializeField] DataPluggable desk2CableEnd;

	[Header("Tutorial Desk 3")]
	[SerializeField] PSU9204 desk3Psu;
	[SerializeField] XRGrabInteractable desk3ScrewDriver;

	[Header("Tutorial Desk 4")]

	[SerializeField] JuniperEX9204 desk4Router;
	[SerializeField] PSU9204 desk4Psu;
	[SerializeField] Transform desk4PowerCable;
	[SerializeField] screwdriverTip desk4ScrewdriverTip;

	public bool CheckObjective(ObjectiveFunction i)
	{
		if (i == ObjectiveFunction.S2PSUInstalled)
		{
			return S2PSUInstalled();
		}
		else if (i == ObjectiveFunction.S2FanCompartmentInstalled)
		{
			return S2FanCompartmentInstalled();
		}
		else if (i == ObjectiveFunction.S2RoutingEngineInstalled)
		{
			return S2RoutingEngineInstalled();
		}
		else if (i == ObjectiveFunction.S2LineCardInstalled)
		{
			return S2LineCardInstalled();
		}
		else if (i == ObjectiveFunction.S2DevicePoweredOn)
		{
			return S2DevicePoweredOn();
		}
		else if (i == ObjectiveFunction.S2PortsConnected)
		{
			return S2PortsConnected();
		}
		else if (i == ObjectiveFunction.alwaysTrue)
		{
			return alwaysTrue();
		}
		else if (i == ObjectiveFunction.alwaysFalse)
		{
			return alwaysFalse();
		}
		else if (i == ObjectiveFunction.trueAfter7Seconds)
		{
			return trueAfter7Seconds();
		}
		else if (i == ObjectiveFunction.S1BrokenPartRaired)
		{
			return S1BrokenPartRaired();
		}
		else if (i == ObjectiveFunction.TutorialDesk1GrabModule)
		{
			return TutorialDesk1CardGrabbed();
		}
		else if (i == ObjectiveFunction.TutorialDesk1ConnectModule)
		{
			return TutorialDesk1CardConnected();
		}
		else if (i == ObjectiveFunction.TutorialDesk1ConnectModule)
		{
			return TutorialDesk1CardConnected();
		}
		else if (i == ObjectiveFunction.TutorialDesk1LockedModule)
		{
			return TutorialDesk1ModuleLocked();
		}
		else if (i == ObjectiveFunction.TutorialDesk2ConnectSfpModule)
		{
			return TutorialDesk2SFPConnected();
		}
		else if (i == ObjectiveFunction.TutorialDesk2LinkRouters)
		{
			return TutorialDesk2LinedRouters();
		}
		else if (i == ObjectiveFunction.TutorialDesk3GrabScrewdriver)
		{
			return TutorialDesk3GrabScrewdriver();
		}
		else if (i == ObjectiveFunction.TutorialDesk3UnscrewPSU)
		{
			return TutorialDesk3ScrewPsu();
		}
		else if (i == ObjectiveFunction.TutorialDesk3RemovePSU)
		{
			return TutorialDesk3RemovePsu();
		}
		else if (i == ObjectiveFunction.TutorialDesk4ConnectPSU)
		{
			return TutorialDesk4ConnectPsu();
		}
		else if (i == ObjectiveFunction.TutorialDesk4ChangeScrewingMode)
		{
			return TutorialDesk4ChangeScrewingMode();
		}
		else if (i == ObjectiveFunction.TutorialDesk4ScrewPsu)
		{
			return TutorialDesk4ScrewPsu();
		}
		else if (i == ObjectiveFunction.TutorialDesk4ConnectPowerCable)
		{
			return TutorialDesk4ConnectToPower();
		}
		else if (i == ObjectiveFunction.TutorialDesk4PowerPsu)
		{
			return TutorialDesk4PowerOnPsu();
		}
		return false;
	}


	bool S1BrokenPartRaired()
	{
		bool healthyModules = true;
		bool healthyConnections = true;
		List<DataSocket[]> ciscoConnections = new() { Cicso1Connection, Cicso2Connection, Cicso3Connection, Cicso4Connection, Cicso5Connection };
		foreach (Slot slot in moduleSlots)
		{
			if (slot.connectedModule == null || slot.connectedModule.faulty)
			{
				Debug.Log("Unhealthy module detected", slot);
				healthyModules = false;
				break;
			}
		}

		foreach (DataSocket[] connections in ciscoConnections)
		{
			DataSocket ciscoSocket = connections[0];
			DataSocket juniperSocket = connections[1];

			foreach (DataSocket socket in ciscoSocket.socketsInConnection)
			{
				if (socket.faulty || socket.connectedPluggable.faulty)
				{
					healthyConnections = false;
					break;
				}
			}
			if (healthyConnections)
			{
				foreach (DataSocket socket in ciscoSocket.socketsInConnection)
				{
					if (socket.faulty || socket.connectedPluggable.faulty)
					{
						healthyConnections = false;
						break;
					}
				}
			}

			if (!healthyConnections || !ciscoSocket.socketsInConnection.Contains(juniperSocket))
			{
				Debug.Log("Unhealthy connection detected in switch: /n " + ciscoSocket.module.device.name + " in port: /n " + ciscoSocket.name, ciscoSocket);
				healthyConnections = false;
				break;
			}
		}
		return healthyModules && healthyConnections;
	}
	bool S2PSUInstalled()
	{
		if (scenario2PSU.device != null)
		{
			if (scenario2PSU.device.gameObject == scenario2JuniperEX9204.gameObject && scenario2PSU.lockedInPlace)
			{
				return true;
			}
		}
		return false;
	}

	bool S2FanCompartmentInstalled()
	{
		if (scenario2FanCompartment.device != null)
		{
			if (scenario2FanCompartment.device.gameObject == scenario2JuniperEX9204.gameObject && scenario2FanCompartment.lockedInPlace)
			{
				return true;
			}
		}
		return false;
	}

	bool S2RoutingEngineInstalled()
	{
		if (scenario2RE.device != null)
		{
			if (scenario2RE.device.gameObject == scenario2JuniperEX9204.gameObject && scenario2RE.lockedInPlace)
			{
				if (scenario2RE.slot.GetComponent<JuniperLineSlot>().activationButton.on)
				{
					return true;
				}
			}
		}
		return false;
	}

	bool S2PortsConnected()
	{
		if (scenario2PortA.socketsInConnection.Contains(scenario2PortB.GetComponent<DataSocket>()))
		{
			return true;
		}
		return false;
	}

	bool S2LineCardInstalled()
	{
		if (scenario2LC.device != null)
		{
			if (scenario2LC.device.gameObject == scenario2JuniperEX9204.gameObject && scenario2LC.lockedInPlace)
			{
				if (scenario2LC.slot.GetComponent<JuniperLineSlot>().activationButton.on)
				{
					return true;
				}
			}
		}
		return false;
	}


	bool S2DevicePoweredOn()
	{
		if (scenario2JuniperEX9204.operating)
		{
			return true;
		}
		return false;
	}

	bool TutorialDesk1CardGrabbed()
	{
		bool isGrabbed = desk1RoutingEngine.isSelected || desk1LineSlot.connectedModule == desk1RoutingEngine;
		if (desk1RoutingEngine.TryGetComponent(out HighlightObject hightlightRE))
		{
			if (!isGrabbed) hightlightRE.Highlight();
			else hightlightRE.StopHighlighting();
		}
		return isGrabbed;
	}
	bool TutorialDesk1CardConnected()
	{
		bool isConnected = desk1LineSlot.connectedModule == desk1RoutingEngine;
		return isConnected;
	}

	bool TutorialDesk1ModuleLocked()
	{
		bool isLocked = true;
		bool prevStepDone = desk1LineSlot.connectedModule == desk1RoutingEngine;
		foreach (var linelock in desk1LineSlotLocks)
		{
			if (linelock.TryGetComponent(out HighlightObject hightlightLock))
			{
				if (prevStepDone && !linelock.locked) hightlightLock.Highlight();
				else hightlightLock.StopHighlighting();
			}

			isLocked = isLocked && linelock.locked;
		}
		return isLocked;
	}

	bool TutorialDesk2SFPConnected()
	{
		bool isConnected = desk2SfpModule.isConnectedToDevice != null && desk2SfpModule.isConnectedToDevice.GetComponent<ME4924>() == desk2Router1;

		if (desk2SfpModule.TryGetComponent(out HighlightObject highlightSFP))
		{
			if (!isConnected) highlightSFP.Highlight();
			else highlightSFP.StopHighlighting();
		}
		return isConnected;
	}

	bool TutorialDesk2LinedRouters()
	{
		bool prevStepDone = desk2SfpModule.isConnectedToDevice != null && desk2SfpModule.isConnectedToDevice.GetComponent<ME4924>() == desk2Router1;

		List<ME4924> devicesInConnection = new();
		if (desk2CableEnd.isConnectedTo != null && desk2CableEnd.isConnectedTo.TryGetComponent<DataSocket>(out var connectionsSocket))
		{
			foreach (DataSocket socket in connectionsSocket.socketsInConnection)
			{
				if (socket.connectedDevice != null) devicesInConnection.Add(socket.connectedDevice.GetComponent<ME4924>());
			}
		}

		if (desk2CableEnd.isConnectedToDevice != null) devicesInConnection.Add(desk2CableEnd.isConnectedToDevice.GetComponent<ME4924>());
		if (desk2CableEnd.otherEnd.isConnectedToDevice != null) devicesInConnection.Add(desk2CableEnd.otherEnd.isConnectedToDevice.GetComponent<ME4924>());

		bool areConnected = devicesInConnection.Contains(desk2Router1) && devicesInConnection.Contains(desk2Router2);

		if (desk2Router1.TryGetComponent(out HighlightObject highlightR1))
		{
			if (prevStepDone && !areConnected) highlightR1.Highlight();
			else highlightR1.StopHighlighting();
		}

		if (desk2Router2.TryGetComponent(out HighlightObject highlightR2))
		{
			if (prevStepDone && !areConnected) highlightR2.Highlight();
			else highlightR2.StopHighlighting();
		}
		return areConnected;
	}

	bool TutorialDesk3GrabScrewdriver()
	{


		bool nextStepDone = true;
		foreach (Screw screw in desk3Psu.lockingScrews) nextStepDone = nextStepDone && !screw.screwed;
		bool isGrabbed = desk3ScrewDriver.isSelected || nextStepDone;

		if (desk3ScrewDriver.TryGetComponent(out HighlightObject highlightScrewdriver))
		{
			if (isGrabbed) highlightScrewdriver.StopHighlighting();
			else highlightScrewdriver.Highlight();
		}
		return isGrabbed;
	}

	bool TutorialDesk3ScrewPsu()
	{
		bool areScrewed = true;
		foreach (Screw screw in desk3Psu.lockingScrews)
		{
			if (screw.TryGetComponent(out HighlightObject hightlightScrew))
			{
				if (desk3ScrewDriver.isSelected && screw.screwed) hightlightScrew.Highlight();
				else hightlightScrew.StopHighlighting();
			}

			areScrewed = areScrewed && !screw.screwed;
		}

		return areScrewed;
	}
	bool TutorialDesk3RemovePsu()
	{
		bool isRemoved = desk3Psu.device == null;
		bool prevStepDone = true;

		foreach (Screw screw in desk3Psu.lockingScrews) prevStepDone = prevStepDone && !screw.screwed;

		if (desk3Psu.TryGetComponent(out HighlightObject highlightObject))
		{
			if (prevStepDone && !isRemoved) highlightObject.Highlight();
			else highlightObject.StopHighlighting();
		}

		return desk3Psu.device == null;
	}
	bool TutorialDesk4ConnectPsu()
	{
		bool isConnected = desk4Psu.device != null && desk4Psu.device.TryGetComponent<JuniperEX9204>(out var router) && router == desk4Router;
		if (desk4Psu.TryGetComponent(out HighlightObject highlightObject))
		{
			if (isConnected) highlightObject.StopHighlighting();
			else highlightObject.Highlight();
		}
		return isConnected;
	}

	bool TutorialDesk4ChangeScrewingMode()
	{
		bool prevStepDone = desk4Psu.device != null && desk4Psu.device.TryGetComponent<JuniperEX9204>(out var router) && router == desk4Router;
		bool isInScrewingMode = desk4ScrewdriverTip.screwSpeed < 0;
		if (desk4ScrewdriverTip.transform.parent.TryGetComponent(out HighlightObject highlightObject))
		{
			if (prevStepDone && !isInScrewingMode) highlightObject.Highlight();
			else highlightObject.StopHighlighting();
		}
		return isInScrewingMode;
	}

	bool TutorialDesk4ScrewPsu()
	{
		bool prevStepDone = desk4ScrewdriverTip.screwSpeed < 0;
		bool isScrewed = true;

		foreach (var screw in desk4Psu.lockingScrews)
		{

			if (screw.TryGetComponent(out HighlightObject highlightObject))
			{
				if (prevStepDone && !screw.screwed) highlightObject.Highlight();
				else highlightObject.StopHighlighting();
			}
			isScrewed = isScrewed && screw.screwed;
		}

		return isScrewed;
	}

	bool TutorialDesk4ConnectToPower()
	{
		bool prevStepDone = true;
		bool isConnectedToPower = desk4Psu.IsConnectedToPower();

		foreach (var screw in desk4Psu.lockingScrews) prevStepDone = prevStepDone && screw.screwed;



		if (desk4PowerCable.TryGetComponent(out HighlightObject highlightCableEnd))
		{
			if (prevStepDone && !isConnectedToPower) highlightCableEnd.Highlight();
			else highlightCableEnd.StopHighlighting();
		}
		if (desk4Psu.powerSocket.TryGetComponent(out HighlightObject highlightPowerSocket))
		{
			if (prevStepDone && !isConnectedToPower) highlightPowerSocket.Highlight();
			else highlightPowerSocket.StopHighlighting();
		}
		return isConnectedToPower;
	}

	bool TutorialDesk4PowerOnPsu()
	{
		bool prevStepDone = desk4Psu.IsConnectedToPower();
		bool isPoweredOn = desk4Psu.operating;
		if (desk4Psu.powerSwitch.TryGetComponent(out HighlightObject highlightObject))
		{
			if (prevStepDone && !isPoweredOn) highlightObject.Highlight();
			else highlightObject.StopHighlighting();
		}
		return isPoweredOn;
	}

	bool alwaysTrue()
	{
		return true;
	}

	bool alwaysFalse()
	{
		return false;
	}

	bool trueAfter7Seconds()
	{
		if (Time.timeSinceLevelLoad > 37)
		{
			return true;
		}
		return false;
	}
}
