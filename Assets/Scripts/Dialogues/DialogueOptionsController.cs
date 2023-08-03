using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueOptionsContainer
{
    public GameObject dialogueOption;
    public TextMeshProUGUI dialogueOptionText;
    public GameObject arrowSelection;
}
public class DialogueOptionsController : MonoBehaviour
{
    [SerializeField] private GameObject[] dialogueOptions;
    private DialogueOptionsContainer[] dialogueOptionsContainers;

    private void Start()
    {
        dialogueOptionsContainers = new DialogueOptionsContainer[dialogueOptions.Length];
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            dialogueOptionsContainers[i] = new DialogueOptionsContainer();
            dialogueOptionsContainers[i].dialogueOption = dialogueOptions[i];
            dialogueOptionsContainers[i].dialogueOptionText = dialogueOptions[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            dialogueOptionsContainers[i].arrowSelection = dialogueOptions[i].transform.GetChild(2).gameObject;
        }
        DialogueManager.Instance.onDialogueOptionsSet += SetOptions;
        DialogueManager.Instance.onDialogueOptionsReset += CloseOptions;
        DialogueManager.Instance.onDialogueOptionsChoose += SetSelectedOption;
    }

    public void SetOptions(string[] dialogueTemplates)
    {
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            if (i < dialogueTemplates.Length)
            {
                dialogueOptions[i].SetActive(true);
                dialogueOptionsContainers[i].dialogueOptionText.text = dialogueTemplates[i];
            }
            else
            {
                dialogueOptions[i].SetActive(false);
            }
            dialogueOptionsContainers[i].arrowSelection.SetActive(false);
        }
        dialogueOptionsContainers[0].arrowSelection.SetActive(true);
    }

    public void SetSelectedOption(int index)
    {
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            dialogueOptionsContainers[i].arrowSelection.SetActive(false);
        }
        dialogueOptionsContainers[index].arrowSelection.SetActive(true);
    }

    public void CloseOptions()
    {
        for (int i = 0; i < dialogueOptions.Length; i++)
        {
            dialogueOptions[i].SetActive(false);
        }
    }
}
