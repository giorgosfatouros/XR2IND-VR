using UnityEngine;

public class MX480PowerSwitchButton : PushableButton
{
	[SerializeField] Material onMat, offMat;
	[SerializeField] MeshRenderer label;

	public override void Start()
	{
		base.Start();
		var materialsCopy = label.materials;
		materialsCopy[0] = on ? onMat : offMat;
		label.materials = materialsCopy;

	}
	public override void FlipSwitch()
	{
		base.FlipSwitch();

		var materialsCopy = label.materials;
		materialsCopy[0] = on ? onMat : offMat;
		label.materials = materialsCopy;
	}
}
