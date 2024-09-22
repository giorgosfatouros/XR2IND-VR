using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class RJ45Socket : DataSocket
{
    [Header("RJ45Socket Attributes")]
    public LED statusLed;

    protected override void Start()
    {
        base.interactionLayers = InteractionLayerMask.GetMask("RJ45Socket");
    }
    public override void Update()
    {
        base.Update();

        if (statusLed != null)
        {
            if (healthyConnection)
            {
                if (connectionSpeed > 100.0f) statusLed.setGreen();
                else if (connectionSpeed <= 100f && connectionSpeed > 10f) statusLed.startGreenBlink();
                else statusLed.setOff();
            }
            else
            {
                statusLed.setOff();
            }
        }
    }
}
