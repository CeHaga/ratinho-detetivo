using UnityEngine;

public class NPC : MonoBehaviour, Interactable
{
	[SerializeField] private DialogueTemplate dialogue;
	[SerializeField] private int tempoNecessario;
	[SerializeField] private GameObject visualCue;
	private bool playerInRange;
	
	private void Awake() {
		playerInRange = false;
		visualCue.SetActive(false);
	}
	
	private void Update() {
		if (playerInRange) {
			visualCue.SetActive(true);
		} else {
			visualCue.SetActive(false);
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Player"))
		{
			DialogueManager.Instance.currentDialogue = this.dialogue;
			DialogueManager.Instance.onDialogueFinish += this.HandleDialogueFinish;
			playerInRange = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			DialogueManager.Instance.currentDialogue = null;
			DialogueManager.Instance.onDialogueFinish -= this.HandleDialogueFinish;
			playerInRange = false;
		}
	}

	private void OnDisable()
	{
		DialogueManager.Instance.currentDialogue = null;
		DialogueManager.Instance.onDialogueFinish -= this.HandleDialogueFinish;
	}

	private void HandleDialogueFinish()
	{
		GameManager.Instance.IncreaseTempoAtual(this.tempoNecessario);
	}
	
	public void Interact() {
		DialogueManager.Instance.StartDialogue(this.dialogue);
	}
}
