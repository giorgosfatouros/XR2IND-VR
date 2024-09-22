using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class LineSlot : Slot
{
    [Header("LineSlot Attributes")]
    public bool locked;
    [SerializeField] InteractionLayerMask activeSlotMask;
    [SerializeField] InteractionLayerMask inactiveSlotMask;
    public float bootWaitingTime = 6.0f;
    public bool slotActive = true;

    protected override void Start()
    {
        base.Start();
        activeSlotMask = base.interactionLayers;
    }

    public void ActivateSlot()
    {
        base.interactionLayers = activeSlotMask;
        slotActive = true;
    }
    public void DeactivateSlot()
    {
        base.interactionLayers = inactiveSlotMask;
        slotActive = false;
    }
}
