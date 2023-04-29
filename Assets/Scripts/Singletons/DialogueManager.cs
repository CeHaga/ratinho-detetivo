using UnityEngine.UI;
using TMPro;
using UnityEngine;

public sealed class DialogueManager : MonoBehaviour {
	public DialogueTemplate currentDialogue;

	public delegate void OnDialogueFinish();

	public event OnDialogueFinish onDialogueFinish;

	[SerializeField] private Canvas UIContainer;
	[SerializeField] private TextMeshProUGUI characterNameComponent;
	[SerializeField] private TextMeshProUGUI messageComponent;
	[SerializeField] private Image avatarComponent;
	[SerializeField] private DiaryEntryEvent OnDiaryEntryAdded;

	public static DialogueManager Instance {
		get; private set;
	}
	public bool hasStarted {
		get; private set;
	}
	
	private void Awake() {
		if (Instance != null && Instance != this) { 
			Destroy(this.gameObject);
		} else {
			Instance = this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start() {
		HideUI();
		hasStarted = false;
	}

	public void StartDialogue() {
		if(this.currentDialogue == null)
			return;
		
		hasStarted = true;
		setUIInfo();
		showDialogueUI();
		if(currentDialogue.opcoes.Count > 0) {
			ShowOpcoes();
		}
	}

	public void NextLine() {
		if(currentDialogue.hasDiaryEntry) {
			DiaryEntry entry = new DiaryEntry(
				currentDialogue.diaryEntryName,
				currentDialogue.texto,
				currentDialogue.diaryEntryHint,
				DiaryEntryType.DIALOG
			);
			OnDiaryEntryAdded.Invoke(entry);
		}
		if(currentDialogue.opcoes.Count > 0) {
			currentDialogue = currentDialogue.opcoes[0].Dialogue;
		} else if (currentDialogue.proximoDialogo != null) {
			currentDialogue = currentDialogue.proximoDialogo;
		} else {
			Debug.Log("Dialogo acabou");
			HideUI();
			hasStarted = false;

			if (this.onDialogueFinish != null)
			{
				this.onDialogueFinish.Invoke();
			}
			
			return;
		}

		setUIInfo();
		if(currentDialogue.opcoes.Count > 0)
			ShowOpcoes();
	}

	private void ShowOpcoes() {
		Debug.Log("===Mostrar Opções===");
		int i = 0;
		foreach (Opcoes opcao in currentDialogue.opcoes) {
			Debug.Log(i + " - " + opcao.Texto);
			i++;
		}
	}

	private void setUIInfo() {
		characterNameComponent.SetText(currentDialogue.personagem.Nome);
		avatarComponent.sprite = currentDialogue.personagem.Avatar;
		messageComponent.SetText(currentDialogue.texto);
	}

	private void showDialogueUI() {
		UIContainer.enabled = true;
	}

	private void HideUI() {
		UIContainer.enabled = false;
	}
}
