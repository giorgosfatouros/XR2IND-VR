using UnityEngine.XR.Interaction.Toolkit;

public class SFPSocket : DataSocket
{
    protected override void Start()
    {
        base.interactionLayers = InteractionLayerMask.GetMask("SFPSocket");
    }
}
