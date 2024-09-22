using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OpacityBreath : MonoBehaviour
{
	private TextMeshProUGUI text;
	private Color originalColor;
	[SerializeField] private float minAlpha;
	[SerializeField] private float maxAlpha;
	[SerializeField] private float speed;
	void Start()
	{
		if (text == null)
		{
			text = GetComponent<TextMeshProUGUI>();
		}
		originalColor = text.color; // Store the original color
	}

	void Update()
	{

		float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * speed) + 1) / 2);
		Color newColor = new(originalColor.r, originalColor.g, originalColor.b, alpha);
		text.color = newColor;
	}
}
