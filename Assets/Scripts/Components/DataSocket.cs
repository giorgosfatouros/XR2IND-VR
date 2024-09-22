using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DataSocket : Socket
{
	[Header("DataSocket Attributes")]
	public float connectionSpeed;
	public float speed = 1000;
	public Device connectedDevice;
	public Module connectedDeviceModule;
	public LED activityLed;
	public List<DataSocket> socketsInConnection = new();
	public List<Pluggable> pluggablesInConnection = new();


	public override void OnPlugConnection(SelectEnterEventArgs selectEnterEventArgs)
	{
		base.OnPlugConnection(selectEnterEventArgs);
		if (CompleteConnection())
		{
			UpdateRelevantSockets();
			UpdateMe();
		}
	}

	public override void OnPlugDisconnection()
	{
		base.OnPlugDisconnection();
		UpdateRelevantSockets();
		CompleteConnection();
		UpdateMe();
		connectedDeviceModule = null;
		connectedDevice = null;
	}



	public DataSocket GetNextSocket(DataSocket socket)
	{

		// returns the socket the other end of the cable is attached to
		if (socket.connectedPluggable != null && socket.connectedPluggable.IsOtherEndConnected && !socketsInConnection.Contains(socket.connectedPluggable.otherEnd.isConnectedTo.GetComponent<DataSocket>()))
		{
			pluggablesInConnection.Add(socket.connectedPluggable);
			pluggablesInConnection.Add(socket.connectedPluggable.otherEnd);
			return socket.connectedPluggable.otherEnd.isConnectedTo.GetComponent<DataSocket>();
		}
		// returns the socket of the adapter attached to it
		else if (socket.connectedPluggable != null && !socket.connectedPluggable.isCable && !socketsInConnection.Contains(socket.connectedPluggable.GetComponent<DataSocket>()))
		{

			pluggablesInConnection.Add(socket.connectedPluggable);
			return socket.connectedPluggable.GetComponent<DataSocket>();
		}
		// returns the socket that the adapter is attached to
		else if (socket.IsConnectedAdapter && !socketsInConnection.Contains(socket.GetComponent<DataPluggable>().isConnectedTo.GetComponent<DataSocket>()))
		{

			pluggablesInConnection.Add(socket.GetComponent<DataPluggable>());
			return socket.GetComponent<DataPluggable>().isConnectedTo.GetComponent<DataSocket>();
		}

		return null;
	}

	public DataSocket GetNeighbourSocket()
	{
		return GetNextSocket(this);
	}

	public bool IterateSockets(DataSocket startSocket)
	{
		DataSocket testSocket = startSocket;
		while (testSocket != null && testSocket.connectedPluggable != null)
		{
			if (!socketsInConnection.Contains(testSocket)) socketsInConnection.Add(testSocket);
			if (testSocket.IsTerminalSocket) return true;
			testSocket = GetNextSocket(testSocket);
		}

		return false;
	}

	public bool CompleteConnection()
	{
		socketsInConnection.Clear();
		pluggablesInConnection.Clear();

		var roundOne = IterateSockets(this);
		var neighbour = GetNeighbourSocket();
		var roundTwo = IterateSockets(neighbour);

		return roundOne && roundTwo;

	}

	public void UpdateMe()
	{
		if (IsTerminalSocket)
		{
			foreach (DataSocket socket in socketsInConnection)
			{
				if (socket.IsTerminalSocket && socket != this)
				{
					connectedDeviceModule = socket.module;
					connectedDevice = socket.module.device;
					break;
				}
				else
				{
					connectedDeviceModule = null;
					connectedDevice = null;
				}
			}
		}

		bool arePartsHealthy = true;
		bool areComponentsOperating = true;

		if (isConnected) areComponentsOperating = connectedDeviceModule && module && connectedDeviceModule.operating && module.operating;
		if (isConnected && areComponentsOperating)
		{
			foreach (var socket in socketsInConnection) if (socket.faulty) { arePartsHealthy = false; break; }
			if (arePartsHealthy) foreach (var plug in pluggablesInConnection) if (plug.faulty) { arePartsHealthy = false; break; }
		}
		healthyConnection = isConnected && arePartsHealthy && areComponentsOperating;

		if (activityLed != null)
		{
			if (healthyConnection)
			{
				activityLed.startGreenStrobe();
			}
			else
			{
				activityLed.setOff();
			}
		}
	}

	public void UpdateRelevantSockets()
	{
		foreach (DataSocket socket in socketsInConnection)
		{
			if (this != socket)
			{
				socket.CompleteConnection();
				socket.UpdateMe();
			}
		}
	}

	public bool IsConnectedAdapter
	{
		get
		{
			return GetComponent<DataPluggable>() != null && GetComponent<DataPluggable>().isConnectedTo != null;
		}
	}
}
