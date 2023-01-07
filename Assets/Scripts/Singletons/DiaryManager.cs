using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryManager : MonoBehaviour
{
	// [SerializeField] private Canvas UIContainer;
	[SerializeField] private List<DiaryEntry> entries;
	
	public static DiaryManager Instance {
		get; private set;
	}
	public bool inDiary {
		get; private set;
	}
	
	private void Awake() {
		if (Instance != null && Instance != this) { 
			Destroy(this.gameObject);
		} else {
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}
	
	void Start()
	{
		HideUI();
		inDiary = false;
		entries = new List<DiaryEntry>();
	}
	
	public void ToogleDiary() {
		// if(inDiary) {
		// 	HideUI();
		// 	inDiary = false;
		// } else {
		// 	showDiaryUI();
		// 	inDiary = true;
		// }
		Debug.Log("Entries: ");
		foreach (DiaryEntry entry in entries) {
			Debug.Log(entry);
		}
	}
	
	public void AddEntry(DiaryEntry entry) {
		// Check if entry already exists
		if(entries.Contains(entry)) {
			return;
		}	
		entries.Add(entry);
	}
	
	private void showDiaryUI() {
		// UIContainer.enabled = true;
	}

	private void HideUI() {
		// UIContainer.enabled = false;
	}
}
