using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[Serializable]
public struct FasesDoJogo
{
	public string cena;
	public int tempoTotal;
}
public sealed class TimeManager : MonoBehaviour
{
	public static TimeManager Instance
	{
		get; private set;
	}

	[Header("Fases do Jogo")]
	[SerializeField] public List<FasesDoJogo> fases;
	[SerializeField] private string finalScene;

	[Header("Clock")]
	[SerializeField] private GameObject timePanel;
	[SerializeField] private RectTransform clockCenter;
	[SerializeField] private float clockRadius;
	[SerializeField] private Transform timeCounterPanel;
	[SerializeField] private RectTransform timeCounterPrefab;

	private int currentTime;
	private int currentSceneIndex;

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
		currentTime = 0;

		currentSceneIndex = this.fases.FindIndex(faseDoJogo => faseDoJogo.cena == SceneManager.GetActiveScene().name);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

		ToggleTimeBar(SceneManager.GetActiveScene().name);
		SetTimeCountAroundClock(this.fases[currentSceneIndex].tempoTotal);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Debug.Log("Carregando cena: " + scene.name);
		ToggleTimeBar(scene.name);
	}

	private void ToggleTimeBar(string sceneName)
	{
		if (this.fases.Any(faseDoJogo => faseDoJogo.cena == sceneName))
		{
			timePanel.SetActive(true);
		}
		else
		{
			timePanel.SetActive(false);
		}
	}

	public void IncreaseTempoAtual(int value)
	{
		Debug.Log("IncreaseTempoAtual");
		currentTime += value;

		if (currentTime >= this.fases[currentSceneIndex].tempoTotal)
		{
			currentTime = 0;
			currentSceneIndex++;
			if (currentSceneIndex >= this.fases.Count)
			{
				CourtroomData.entries = DiaryManager.Instance.entries;
				SceneManager.LoadScene(this.finalScene);
			}
			else
				SceneManager.LoadScene(this.fases[currentSceneIndex].cena);
		}
	}

	private void SetTimeCountAroundClock(int totalTime)
	{
		foreach (Transform child in timeCounterPanel)
		{
			Destroy(child.gameObject);
		}

		float offset = 90f / (totalTime - 1);
		for (int i = 0; i < totalTime; i++)
		{
			RectTransform timeCounter = Instantiate(timeCounterPrefab, timeCounterPanel);
			float x = -Mathf.Sin(i * offset * Mathf.Deg2Rad) * clockRadius;
			float y = Mathf.Cos(i * offset * Mathf.Deg2Rad) * clockRadius;
			// timeCounter.anchoredPosition = new Vector2(x, y);
			timeCounter.position = new Vector2(x, y) + (Vector2)clockCenter.position;
			Debug.Log("x: " + x + " y: " + y);
			timeCounter.anchorMin = timeCounter.anchorMax = timeCounter.pivot = new Vector2(0.5f, 0.5f);
		}
	}
}
