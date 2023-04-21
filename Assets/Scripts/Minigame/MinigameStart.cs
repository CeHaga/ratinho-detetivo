using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameStart : MonoBehaviour
{
	[Header("Scenes")]
	[SerializeField] private string minigameScene;
	
	public void StartMinigame()
	{
		SceneManager.LoadScene(minigameScene);
	}
	
	private void Start() {		
		Debug.Log(MinigameResult.happened);
		Debug.Log(MinigameResult.win);
	}
}
