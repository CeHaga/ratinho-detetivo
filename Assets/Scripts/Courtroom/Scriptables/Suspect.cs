using UnityEngine;

[CreateAssetMenu(fileName = "Suspect", menuName = "Suspect", order = 0)]
public class Suspect : ScriptableObject {
	public new string name;
	
	public override string ToString() {
		return name;
	}
}
