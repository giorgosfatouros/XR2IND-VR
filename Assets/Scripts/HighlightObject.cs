using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HighlightObject : MonoBehaviour
{
	// Color highlightColor = new(0.694117665f, 0.694117665f, 1f, 255);
	Color highlightColor = new(0f, 0f, 1f, 255);
	float speed = 1f;
	Coroutine hl;
	List<Material> materials = new List<Material>();
	List<Color> startColors = new List<Color>();
	public bool isHighlighted;


	void Start()
	{
		isHighlighted = false;
		foreach (Renderer rend in this.GetComponentsInChildren<Renderer>())
		{
			foreach (Material mat in rend.materials)
			{
				materials.Add(mat);
				startColors.Add(mat.color);
			}
		}
	}

	public void Highlight()
	{
		hl ??= StartCoroutine(HighlightCoroutine());
	}

	IEnumerator HighlightCoroutine()
	{
		isHighlighted = true;
		float tick = 0f;
		while (true)
		{
			tick += Time.deltaTime * speed;
			for (int i = 0; i < materials.Count; i++)
			{
				materials[i].color = Color.Lerp(startColors[i], highlightColor, Mathf.PingPong(tick, speed));
			}
			yield return null;
		}
	}

	public void StopHighlighting()
	{
		if (hl == null) return;
		StopCoroutine(hl);
		hl = null;
		isHighlighted = false;
		for (int i = 0; i < materials.Count; i++)
		{
			materials[i].color = startColors[i];
		}
	}
}
