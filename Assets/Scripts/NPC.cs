using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private DialogueTemplate dialogue;
    [SerializeField] private int tempoNecessario;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            DialogueManager.Instance.currentDialogue = this.dialogue;
            DialogueManager.Instance.onDialogueFinish += this.HandleDialogueFinish;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            DialogueManager.Instance.currentDialogue = null;
            DialogueManager.Instance.onDialogueFinish -= this.HandleDialogueFinish;
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
}
