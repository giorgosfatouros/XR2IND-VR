using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CompartmentAttacher : MonoBehaviour
{
    public PSU[] pSUs;
    [SerializeField] PSUSlot[] pSUslots;
    public FanCompartment[] fanCompartments;
    [SerializeField] FanCompartmentSlot[] fanCompartmentSlots;
    public JuniperLineCard[] juniperLineCards;
    [SerializeField] JuniperLineSlot[] juniperLineSlots;
    public JuniperRoutingEngine[] juniperRoutingEngines;
    [SerializeField] JuniperLineSlot[] juniperRoutingEngineSlots;
    public Pluggable[] pluggables;
    [SerializeField] Socket[] sockets;
    public XRInteractionManager interactionManager;

    void Start()
    {
        for (int i = 0; i < pSUs.Length; i++)
            FullyConnectPSU(pSUs[i], pSUslots[i]);

        for (int i = 0; i < fanCompartments.Length; i++)
            FullyConnectFC(fanCompartments[i], fanCompartmentSlots[i]);

        for (int i = 0; i < juniperLineCards.Length; i++)
            FullyConnectLineCard(juniperLineCards[i], juniperLineSlots[i]);

        for (int i = 0; i < juniperRoutingEngines.Length; i++)
            FullyConnectRoutingEngine(juniperRoutingEngines[i], juniperRoutingEngineSlots[i]);

        for (int i = 0; i < pluggables.Length; i++)
            Attach(pluggables[i], sockets[i]);

    }

    public void Attach(IXRSelectInteractable interactable, IXRSelectInteractor socket)
    {
        StartCoroutine(AttachCoroutine(interactable, socket));
    }
    public IEnumerator AttachCoroutine(IXRSelectInteractable interactable, IXRSelectInteractor socket)
    {
        yield return null;
        interactionManager.SelectEnter(socket, interactable);
    }

    public IEnumerator LockLineCardSlot(JuniperLineSlot slotLock)
    {
        yield return null;
        slotLock.rightLock.LockInteract();
        slotLock.leftLock.LockInteract();
    }

    public IEnumerator ScrewModule(Module module)
    {
        yield return null;
        foreach (Screw screw in module.GetComponentsInChildren<Screw>())
        {
            screw.ScrewIt();
        }
    }

    public void TurnOnPSU(PSU psuModule)
    {
        psuModule.powerSwitch.SetSwitch(true);
    }

    public void FullyConnectLineCard(JuniperLineCard juniperLineCard, IXRSelectInteractor socket)
    {
        Attach(juniperLineCard, socket);
        StartCoroutine(LockLineCardSlot(socket.transform.GetComponent<JuniperLineSlot>()));
        socket.transform.GetComponent<JuniperLineSlot>().controlledBy.SetSwitch(true);
    }

    public void FullyConnectRoutingEngine(JuniperRoutingEngine juniperRE, IXRSelectInteractor socket)
    {
        Attach(juniperRE, socket);
        StartCoroutine(LockLineCardSlot(socket.transform.GetComponent<JuniperLineSlot>()));
        socket.transform.GetComponent<JuniperLineSlot>().controlledBy.SetSwitch(true);
        juniperRE.onlineButton.SetSwitch(true);
    }

    public void FullyConnectPSU(PSU psu, IXRSelectInteractor socket)
    {
        Attach(psu, socket);
        StartCoroutine(ScrewModule(psu));
        TurnOnPSU(psu);
    }
    public void FullyConnectFC(FanCompartment fc, IXRSelectInteractor socket)
    {
        Attach(fc, socket);
        StartCoroutine(ScrewModule(fc));
    }
}
