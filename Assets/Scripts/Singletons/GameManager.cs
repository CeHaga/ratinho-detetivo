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
public sealed class GameManager : MonoBehaviour
{
	public static GameManager Instance {
		get; private set;
	}
	
	public static int TempoTotal = 0;
	public static int TempoAtual = 1;
	public static int IndexCenaAtual = 0;

	[Header("Fases do Jogo")]
	[SerializeField] public List<FasesDoJogo> fases;
	
	[Header("Time Events")]
	[SerializeField] private UnityEvent OnTempoChange;
	[SerializeField] private UnityEvent ShowTimeBar;
	[SerializeField] private UnityEvent HideTimeBar;

	private void Awake() {
		if (Instance != null && Instance != this) { 
			Destroy(this.gameObject);
		} else {
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		GameManager.TempoAtual = 1;
		GameManager.IndexCenaAtual = this.fases.FindIndex(faseDoJogo => faseDoJogo.cena == SceneManager.GetActiveScene().name);
		GameManager.TempoTotal = this.fases.Aggregate(0, (acc, faseDoJogo) => acc + faseDoJogo.tempoTotal);
		UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
		
		this.OnTempoChange?.Invoke();
		ToggleTimeBar(SceneManager.GetActiveScene().name);
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
			ShowTimeBar?.Invoke();
		}
		else
		{
			HideTimeBar?.Invoke();
		}
	}

	public void IncreaseTempoAtual(int value)
	{
		Debug.Log("IncreaseTempoAtual");
		GameManager.TempoAtual += value;
		this.OnTempoChange?.Invoke();

		if (GameManager.TempoAtual > this.fases[GameManager.IndexCenaAtual].tempoTotal)
		{
			GameManager.TempoAtual = 1;
			GameManager.IndexCenaAtual++;
			SceneManager.LoadScene(this.fases[GameManager.IndexCenaAtual].cena);
		}
	}
}
