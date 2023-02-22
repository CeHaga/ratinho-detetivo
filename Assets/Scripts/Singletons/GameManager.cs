using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
    
    public static int TempoAtual = 1;
    private static int indexCenaAtual = 0;
    private static int tempoTotal = 0;

    [SerializeField]
    private List<FasesDoJogo> fases;
    
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
        GameManager.indexCenaAtual = 0;
        GameManager.tempoTotal = this.fases.Aggregate(0, (acc, faseDoJogo) => acc + faseDoJogo.tempoTotal);
    }

    public void IncreaseTempoAtual(int value)
    {
        GameManager.TempoAtual += value;

        if (GameManager.TempoAtual > this.fases[GameManager.indexCenaAtual].tempoTotal)
        {
            GameManager.TempoAtual = 1;
            GameManager.indexCenaAtual++;
            SceneManager.LoadScene(this.fases[GameManager.indexCenaAtual].cena);
        }
    }

    /*private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene();
    }*/
}
