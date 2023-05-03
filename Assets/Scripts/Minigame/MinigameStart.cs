using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MinigameStart : MonoBehaviour
{
	[Header("Scenes")]
	[SerializeField] private string minigameScene;
	
	private void Start() {		
		Debug.Log("MinigameResult.happened: " + MinigameResult.happened);
		Debug.Log("MinigameResult.win: " + MinigameResult.win);
		
		MinigameResult.mainGameScene = SceneManager.GetActiveScene().name;
		
	}
	
	public void StartMinigame()
	{
		Debug.Log("StartMinigame");
		SceneManager.LoadScene(minigameScene);
	}
}
