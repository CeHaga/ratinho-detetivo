using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryButtonsCreator : MonoBehaviour
{
	[SerializeField] private GameObject buttonPrefab;
	[SerializeField] private Judge judge;
	
	private int chosenButton = -1;
	
	public void CreateButtons(int n)
	{
		for (int i = 0; i < n; i++)
		{
			GameObject button = Instantiate(this.buttonPrefab, this.transform);
			
			DiaryButton buttonScript = button.GetComponent<DiaryButton>();
			buttonScript.SetButtonIndex(i);
			
			Button buttonBtn = button.GetComponent<Button>();
			buttonBtn.onClick.AddListener(() => this.judge.SetEvidenceIndex(buttonScript.GetButtonIndex()));
		}
	}
	
	public void ChooseButton(int index)
	{
		this.chosenButton = index;
	}
	
	public int ConfirmChoice()
	{
		return this.chosenButton;
	}
}
