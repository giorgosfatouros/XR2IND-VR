public class RoutingEngineLineSlot : JuniperLineSlot
{

	public LED masterLED, onlineLED, offlineLED;

	public override void Update()
	{
		base.Update();
		if (!device.GetComponent<JuniperSwitch>().acoltButton.pressed)
		{
			if (connectedModule && connectedModule.operating)
			{
				if (connectedModule.GetComponent<JuniperRoutingEngine>().isOnline)
				{
					onlineLED.setDefault();
					offlineLED.setOff();
				}
				else
				{
					onlineLED.setOff();
					offlineLED.setDefault();
				}
			}
			else
			{
				masterLED.setOff();
				onlineLED.setOff();
				offlineLED.setOff();
			}
		}
	}
}
