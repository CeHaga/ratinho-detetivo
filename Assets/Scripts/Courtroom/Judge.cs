using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
	[SerializeField] private DiaryButtonsCreator buttonsCreator;
	[SerializeField] private GameObject suspectsPanel;
	[SerializeField] private GameObject veredictPanel;
	[SerializeField] private TMPro.TextMeshProUGUI suspectText;
	[SerializeField] private TMPro.TextMeshProUGUI veredictText;

	private List<DiaryEntry> entries;

	private Suspect suspect;
	private DiaryEntry evidence;

	private void Start()
	{
		this.entries = CourtroomData.entries;
		int count = this.entries != null ? this.entries.Count : 0;
		this.buttonsCreator.CreateButtons(count);

		this.suspectsPanel.SetActive(true);
		this.veredictPanel.SetActive(false);

		this.suspect = null;
		this.evidence = null;
	}

	public void SetSuspect(Suspect suspect)
	{
		this.suspect = suspect;
		Debug.Log("Suspect set to " + suspect.ToString());
	}

	public void SetEvidenceIndex(int index)
	{
		Debug.Log("Evidence index set to " + index);
		this.evidence = this.entries[index];
		Debug.Log("Evidence set to " + this.evidence.ToString());
	}

	public void ConfirmChoice()
	{
		if (evidence == null || suspect == null)
		{
			Debug.LogError("Suspect or evidence is null");
			return;
		}

		suspectsPanel.SetActive(false);
		veredictPanel.SetActive(true);

		suspectText.text = suspect.ToString();
		veredictText.text = evidence.ToString();
	}
}
