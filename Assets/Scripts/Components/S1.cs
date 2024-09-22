using UnityEngine;

public class S1 : MonoBehaviour
{
	public Module[] SceneModules;
	public DataPluggable[] ScenePluggables;
	[SerializeField] bool dontPlaceFaulty = false;


	void Start()
	{
		if (dontPlaceFaulty) return;
		System.Random random = new();

		int randomModule = random.Next(0, SceneModules.Length);
		SceneModules[randomModule].faulty = true;


		int randomPluggable = random.Next(0, ScenePluggables.Length);
		ScenePluggables[randomPluggable].faulty = true;


	}

}
