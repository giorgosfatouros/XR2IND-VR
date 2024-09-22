using System.Collections;
using UnityEngine;

public class Screw : MonoBehaviour
{
	[SerializeField] Module module;
	[SerializeField] float animDuration = 0.5f;
	private Vector3 unscrewedPosition = new();
	private Vector3 screwedPosition = new();
	private BoxCollider boxCollider;
	private Coroutine activeCoroutine;
	private double slidingDistance;
	public bool screwed;
	public bool invokeScrew = false;
	public bool invokeUnscrew = false;
	private float slidePercent = .85f;

	private void Update()
	{
		if (invokeScrew && !screwed)
		{
			ScrewIt();
		}

		if (invokeUnscrew && screwed)
		{
			UnscrewIt();
		}
		invokeUnscrew = false;
		invokeScrew = false;
	}

	public void Start()
	{
		boxCollider = GetComponent<BoxCollider>();
		unscrewedPosition = transform.localPosition;
		slidingDistance = transform.localScale.y * boxCollider.size.y * slidePercent;
		screwedPosition = unscrewedPosition + (float)slidingDistance * (transform.localRotation * Vector3.down);

	}
	public void UnscrewIt()
	{
		if (module.slot != null && activeCoroutine == null)
		{
			activeCoroutine = StartCoroutine(ScrewRoutine(screwedPosition, unscrewedPosition, false));
		}
	}
	public void ScrewIt()
	{
		if (module.slot != null && activeCoroutine == null)
		{
			activeCoroutine = StartCoroutine(ScrewRoutine(unscrewedPosition, screwedPosition, true));
		}
	}

	public IEnumerator ScrewRoutine(Vector3 startPosition, Vector3 stopPosition, bool isScrewed)
	{

		float timeStep = animDuration / Time.deltaTime;
		var step = slidingDistance / timeStep;
		var slidingDirection = (stopPosition - startPosition).normalized;
		var screwingDirection = (isScrewed ? 1f : -1f) * 6f;


		while ((transform.localPosition - startPosition).magnitude < slidingDistance)
		{

			var center = GetComponent<MeshRenderer>().bounds.center;
			transform.localPosition += slidingDirection * (float)step;
			transform.RotateAround(center, transform.up, screwingDirection);
			yield return null;
		}

		screwed = isScrewed;
		activeCoroutine = null;
	}

}
