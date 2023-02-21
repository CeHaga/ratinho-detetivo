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
    public static int TempoAtual = 0;

    public static GameManager Instance {
        get; private set;
    }

    [SerializeField]
    private List<FasesDoJogo> fases;

    private int indexCenaAtual = 0;
    private int tempoTotal = 0;
    
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
        this.tempoTotal = this.fases.Aggregate(0, (acc, faseDoJogo) => acc + faseDoJogo.tempoTotal);
    }

    public void IncreaseTempoAtual(int value)
    {
        GameManager.TempoAtual += value;
        Debug.Log(GameManager.TempoAtual);

        if (GameManager.TempoAtual >= this.fases[this.indexCenaAtual].tempoTotal)
        {
            this.indexCenaAtual++;
            SceneManager.LoadScene(this.fases[this.indexCenaAtual].cena);
        }
    }

    /*private void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene();
    }*/
}
