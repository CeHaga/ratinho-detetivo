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

    [SerializeField] public List<FasesDoJogo> fases;
    [SerializeField] private UnityEvent OnTempoChange;

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
        GameManager.IndexCenaAtual = 0;
        GameManager.TempoTotal = this.fases.Aggregate(0, (acc, faseDoJogo) => acc + faseDoJogo.tempoTotal);
    }

    public void IncreaseTempoAtual(int value)
    {
        GameManager.TempoAtual += value;
        this.OnTempoChange?.Invoke();

        if (GameManager.TempoAtual > this.fases[GameManager.IndexCenaAtual].tempoTotal)
        {
            GameManager.TempoAtual = 1;
            GameManager.IndexCenaAtual++;
            SceneManager.LoadScene(this.fases[GameManager.IndexCenaAtual].cena);
        }
    }

    /*private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene();
    }*/
}
