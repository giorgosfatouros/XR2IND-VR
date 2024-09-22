using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using HuggingFace.API;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;


public class LLMScreen : MonoBehaviour
{
	[SerializeField] List<InputActionReference> toggleCanvas;
	[SerializeField] List<InputActionReference> toggleRecording;
	[SerializeField] InputActionReference controllerScroll;
	[SerializeField] InputActionReference mouseScroll;
	[SerializeField] List<InputActionReference> move;
	[SerializeField] Canvas canvas;
	[SerializeField] RectTransform textRect;
	[SerializeField] RectTransform panel;
	[SerializeField] RectTransform loadingRect;
	public float moveSpeed = 10f;
	private TextMeshProUGUI text;
	private AudioClip clip;
	private byte[] bytes;
	private List<RectTransform> panelElements;
	public ARPanel[] canvases;
	int micStartPos, micStopPos;

	void Start()
	{
		foreach (var actionReference in toggleCanvas) actionReference.action.performed += ToggleCanvas;
		foreach (var actionReference in toggleRecording)
		{
			actionReference.action.performed += StartRecording;
			actionReference.action.canceled += StopRecording;
		}




		text = textRect.GetComponent<TextMeshProUGUI>();
		clip = Microphone.Start(null, false, 3599, 44100);
		panelElements = new List<RectTransform> { textRect, loadingRect };
		ShowCanvas(false);


	}

	private void StartRecording(InputAction.CallbackContext ctx)
	{
		micStartPos = Microphone.GetPosition(null);
		ShowElement(loadingRect);
	}

	private void StopRecording(InputAction.CallbackContext ctx)
	{
		micStopPos = Microphone.GetPosition(null);
		var samples = new float[(micStopPos - micStartPos) * clip.channels];
		clip.GetData(samples, micStartPos);
		bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
		SendRecording(bytes);
		UpdateText("You: ");
		ShowElement(textRect);
	}

	private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
	{
		using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
		{
			using (var writer = new BinaryWriter(memoryStream))
			{
				writer.Write("RIFF".ToCharArray());
				writer.Write(36 + samples.Length * 2);
				writer.Write("WAVE".ToCharArray());
				writer.Write("fmt ".ToCharArray());
				writer.Write(16);
				writer.Write((ushort)1);
				writer.Write((ushort)channels);
				writer.Write(frequency);
				writer.Write(frequency * channels * 2);
				writer.Write((ushort)(channels * 2));
				writer.Write((ushort)16);
				writer.Write("data".ToCharArray());
				writer.Write(samples.Length * 2);

				foreach (var sample in samples)
				{
					writer.Write((short)(sample * short.MaxValue));
				}
			}
			return memoryStream.ToArray();
		}
	}

	private void SendRecording(byte[] bytes)
	{
		HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
		{
			Debug.Log("The user asks: " + response);
			AskTheLLM(response);
			InlineTextAppend(response);
		}, error =>
		{
			// text_field.GetComponent<TMP_InputField>().text.color = Color.red;
			// text_field.GetComponent<TMP_InputField>().text = error;
		});
	}

	public async void AskTheLLM(string prompt)
	{
		HttpClient client = new HttpClient();
		HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Environment.GetEnvironmentVariable("LLM_ASSISTANT"));
		request.Content = new StringContent("{ \"messages\": [{ \"role\": \"user\", \"content\": \"" + prompt + "\" }] }");
		request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
		HttpResponseMessage response = await client.SendAsync(request);
		response.EnsureSuccessStatusCode();
		string responseBody = await response.Content.ReadAsStringAsync();
		Debug.Log("LLM answers: " + responseBody);
		NewLineTextAppend("Answer: " + responseBody);
	}

	void Update()
	{
		Scroll();
	}

	public void OpenCanvas()
	{
		canvas.enabled = true;
		foreach (var actionReference in move) actionReference.action.Disable();
		textRect.anchoredPosition3D = new Vector3(0, 0, 0);
	}

	public void CloseCanvas()
	{
		canvas.enabled = false;
		foreach (var actionReference in move) actionReference.action.Enable();
	}

	public void ToggleCanvas(InputAction.CallbackContext ctx)
	{
		ShowCanvas(!canvas.enabled);
	}

	public void ShowCanvas(bool show)
	{
		if (show) OpenCanvas();
		else CloseCanvas();
	}

	public void UpdateText(string newText)
	{
		text.text = newText;
		textRect.anchoredPosition3D = new Vector3(0, 0, 0);
		ShowElement(textRect);
	}

	public void Scroll()
	{
		Vector2 scrollValue = new(0, 0);
		if (controllerScroll.action.phase != InputActionPhase.Waiting)
		{
			scrollValue = controllerScroll.action.ReadValue<Vector2>();
		}
		else if (mouseScroll.action.phase != InputActionPhase.Waiting)
		{
			scrollValue = mouseScroll.action.ReadValue<Vector2>();
		}

		float offset = Time.deltaTime * moveSpeed * scrollValue.y;
		offset = mouseScroll.action.phase != InputActionPhase.Waiting ? offset / 10f : offset;

		float newYpos = textRect.anchoredPosition3D.y + offset;

		if (newYpos <= textRect.sizeDelta.y - panel.rect.height && newYpos >= 0)
		{
			textRect.anchoredPosition3D += new Vector3(0, offset, 0);
		}


	}

	public void ShowElement(RectTransform element)
	{
		foreach (var panelElement in panelElements)
		{
			Debug.Log(panelElement.name, panelElement);
			panelElement.gameObject.SetActive(panelElement == element);
		}

		ShowCanvas(true);
	}

	public void InlineTextAppend(string extraText)
	{
		text.text += extraText;
	}

	public void NewLineTextAppend(string extraText)
	{
		text.text = text.text + "\n" + extraText;
	}
}
