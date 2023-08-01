using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiaryButton : MonoBehaviour
{
	[SerializeField] private TMPro.TextMeshProUGUI text;
	private int buttonIndex;
	
	public void SetButtonIndex(int index)
	{
		this.buttonIndex = index;
		this.text.text = (index + 1).ToString();
	}
	
	public int GetButtonIndex()
	{
		return this.buttonIndex;
	}
}
