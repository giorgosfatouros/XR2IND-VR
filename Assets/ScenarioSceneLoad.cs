using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioSceneLoad : MonoBehaviour
{
	public void LoadScenario01()
	{
		SceneManager.LoadScene("S1_new");
	}
	public void LoadScenario02()
	{
		SceneManager.LoadScene("S2_new");
	}
	public void LoadTutorial()
	{
		SceneManager.LoadScene("ScenarioSelect");
	}
}
