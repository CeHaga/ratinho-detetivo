using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProgressaoTempoHandler : MonoBehaviour
{
	public static ProgressaoTempoHandler Instance
	{
		get; private set;
	}

	[SerializeField] private Image mask;
	[SerializeField] private GameObject timeBar;

	private int maximum;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		this.maximum = GameManager.Instance.fases.Aggregate(0, (acc, faseDoJogo) => acc + faseDoJogo.tempoTotal);

		this.UpdateUI();
	}

	public void UpdateUI()
	{
		int offset = 0;

		for (int i = 0; i < GameManager.IndexCenaAtual; i++)
		{
			offset += GameManager.Instance.fases[i].tempoTotal;
		}
		float percentage = ((float)GameManager.TempoAtual + offset) / this.maximum;
		Debug.Log("Percentage: " + percentage);
		this.mask.fillAmount = percentage;
	}

	public void Hide()
	{
		Debug.Log("Hide");
		timeBar.SetActive(false);
	}

	public void Show()
	{
		Debug.Log("Show");
		timeBar.SetActive(true);
	}
}
