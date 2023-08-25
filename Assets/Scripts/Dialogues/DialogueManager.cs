using UnityEngine.UI;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public enum MoveDialogueOptions
{
	UP,
	DOWN
}

public sealed class DialogueManager : MonoBehaviour
{
	public DialogueTemplate currentDialogue;
	public DialogueTemplate nextDialogue;
	private int dialogueIndex;
	private int countOptions;

	public delegate void OnDialogueStart();
	public delegate void OnDialogueFinish();
	public delegate void OnDialogueOptionsSet(string[] options);
	public delegate void OnDialogueOptionsReset();
	public delegate void OnDialogueOptionsChoose(int index);

	public event OnDialogueStart onDialogueStart;
	public event OnDialogueFinish onDialogueFinish;
	public event OnDialogueOptionsSet onDialogueOptionsSet;
	public event OnDialogueOptionsReset onDialogueOptionsReset;
	public event OnDialogueOptionsChoose onDialogueOptionsChoose;

	[SerializeField] private Canvas UIContainer;
	[SerializeField] private TextMeshProUGUI characterNameComponent;
	[SerializeField] private TextMeshProUGUI messageComponent;
	[SerializeField] private Image avatarComponent;
	[SerializeField] private DiaryEntryEvent OnDiaryEntryAdded;

	public static DialogueManager Instance
	{
		get; private set;
	}
	public bool hasStarted
	{
		get; private set;
	}

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
		HideUI();
		hasStarted = false;
	}

	private void SetDialogue(DialogueTemplate dialogue)
	{
		currentDialogue = dialogue;
		dialogueIndex = 0;
		countOptions = currentDialogue.opcoes.Count;
		ShowOpcoes(currentDialogue.opcoes);
		if (currentDialogue.hasDiaryEntry)
		{
			DiaryEntry entry = new DiaryEntry(
				currentDialogue.diaryEntryName,
				currentDialogue.texto,
				currentDialogue.diaryEntryHint,
				currentDialogue.diaryEntryType
			);
			OnDiaryEntryAdded.Invoke(entry);
		}
		nextDialogue = currentDialogue.proximoDialogo;
		if (currentDialogue.opcoes.Count > 0)
		{
			nextDialogue = currentDialogue.opcoes[0].Dialogue;
		}
		setUIInfo();
	}

	public void StartDialogue(DialogueTemplate dialogue)
	{
		if (onDialogueStart != null)
		{
			onDialogueStart.Invoke();
		}
		SetDialogue(dialogue);
		hasStarted = true;
		showDialogueUI();
	}

	public void Continue()
	{
		if (nextDialogue != null)
		{
			SetDialogue(nextDialogue);
			return;
		}

		Debug.Log("Dialogo acabou");
		HideUI();
		hasStarted = false;

		if (this.onDialogueFinish != null)
		{
			this.onDialogueFinish.Invoke();
		}
	}

	public void MoveOption(MoveDialogueOptions option)
	{
		if (currentDialogue.opcoes.Count > 0)
		{
			switch (option)
			{
				case MoveDialogueOptions.UP:
					dialogueIndex--;
					if (dialogueIndex < 0)
					{
						dialogueIndex = countOptions - 1;
					}
					break;
				case MoveDialogueOptions.DOWN:
					dialogueIndex++;
					if (dialogueIndex >= countOptions)
					{
						dialogueIndex = 0;
					}
					break;
			}
			nextDialogue = currentDialogue.opcoes[dialogueIndex].Dialogue;
			onDialogueOptionsChoose?.Invoke(dialogueIndex);
		}
		if (nextDialogue != null) Debug.Log("Opção selecionada " + dialogueIndex + ": " + nextDialogue.texto);
		else Debug.Log("Diálogo vai acabar");
	}

	private void ShowOpcoes(List<Opcoes> opcoes)
	{
		if (opcoes.Count == 0)
		{
			onDialogueOptionsReset?.Invoke();
			return;
		}
		Debug.Log("===Mostrar Opções===");
		int i = 0;
		foreach (Opcoes opcao in currentDialogue.opcoes)
		{
			Debug.Log(i + " - " + opcao.Texto);
			i++;
		}
		string[] options = new string[opcoes.Count];
		for (i = 0; i < opcoes.Count; i++)
		{
			options[i] = opcoes[i].Texto;
		}
		onDialogueOptionsSet?.Invoke(options);
	}


	private void setUIInfo()
	{
		characterNameComponent.SetText(currentDialogue.personagem.Nome);
		avatarComponent.sprite = currentDialogue.personagem.Avatar;
		messageComponent.SetText(currentDialogue.texto);
	}

	private void showDialogueUI()
	{
		UIContainer.enabled = true;
	}

	private void HideUI()
	{
		UIContainer.enabled = false;
	}
}
