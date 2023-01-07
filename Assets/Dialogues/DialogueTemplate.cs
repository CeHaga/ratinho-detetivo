using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Opcoes {
	public string Texto;
	public DialogueTemplate Dialogue;
}

[CreateAssetMenu(fileName = "DialogueTemplate", menuName = "Dialogues/DialogueTemplate", order = 0)]
public class DialogueTemplate : ScriptableObject {
	[SerializeField] public CharacterTemplate personagem;
	[TextArea(8, 6)] [SerializeField] public string texto;
	[SerializeField] public DialogueTemplate proximoDialogo;
	[SerializeField] public List<Opcoes> opcoes;
	[Header("Diary Entry")]
	[SerializeField] public bool hasDiaryEntry;
	[SerializeField] public string diaryEntryName;
	[SerializeField] public string diaryEntryHint;
}
