using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class LockModules : MonoBehaviour
{
	// Start is called before the first frame update
	Module[] Modules;
	JuniperLineSlot[] JuniperLineSlotLocks;
	[SerializeField] GameObject[] objectLists;
	[SerializeField] XRInteractionManager xRInteractionManager;
	readonly float seconds = 0.1f;
	List<List<Transform>> moduleList;

	void Awake()
	{
		Modules = FindObjectsOfType<Module>();
		JuniperLineSlotLocks = FindObjectsOfType<JuniperLineSlot>();
		moduleList = new();
		for (int i = 0; i < objectLists.Length; i++)
		{
			var objCategory = objectLists[i];
			moduleList.Add(new List<Transform>());
			foreach (Transform child in objCategory.transform)
			{
				moduleList[i].Add(child);
				child.gameObject.SetActive(false);
			}
		}



	}
	void Start()
	{
		StartCoroutine(StartAfterSeconds(seconds));
	}



	IEnumerator StartAfterSeconds(float seconds)
	{
		yield return new WaitForFixedUpdate();
		yield return new WaitForEndOfFrame(); // Wait until the end of the frame


		foreach (var list in moduleList)
		{

			foreach (Transform child in list)
			{
				Collider rb = child.GetComponent<Collider>();
				if (rb != null) rb.isTrigger = true;
				child.gameObject.SetActive(true);
				yield return new WaitForSeconds(seconds);
			}
		}

		StartCoroutine(ScrewModules());
	}

	IEnumerator ScrewModules()
	{


		foreach (Module module in Modules)
		{

			if (module.slot != null)
			{
				for (int i = 0; i < module.lockingScrews.Count; i++) module.lockingScrews[i].ScrewIt();
				if (module.TryGetComponent(out PSU psuModule))
				{
					if (psuModule.IsConnectedToPower())
					{
						yield return new WaitForSeconds(seconds);
						psuModule.powerSwitch.SetSwitch(true);
					}
				}
			}
		}

		foreach (JuniperLineSlot slotLock in JuniperLineSlotLocks)
		{
			if (slotLock.connectedModule != null)
			{
				yield return new WaitForSeconds(seconds);
				slotLock.rightLock.LockInteract();
				slotLock.leftLock.LockInteract();

				if (slotLock.connectedModule.TryGetComponent(out JuniperRoutingEngine reModule))
				{
					slotLock.controlledBy.SetSwitch(true);
					reModule.onlineButton.SetSwitch(true);

				}

				if (slotLock.connectedModule.TryGetComponent(out JuniperLineCard lineModule))
				{
					slotLock.controlledBy.SetSwitch(true);

				}
			}
		}

		foreach (var list in moduleList)
		{

			foreach (Transform child in list)
			{
				Collider rb = child.GetComponent<Collider>();
				if (rb != null) rb.isTrigger = false;
				yield return new WaitForSeconds(seconds);
			}
		}

		Debug.Log("Modules screwed in place");

	}


}
