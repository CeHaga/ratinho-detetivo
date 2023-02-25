using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProgressaoTempoHandler : MonoBehaviour
{
    public static ProgressaoTempoHandler Instance {
        get; private set;
    }
    
    [SerializeField] private Image mask;
    
    private int maximum;
    
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
        this.mask.fillAmount = percentage;
    }
}
