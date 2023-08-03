using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterTemplate", menuName = "Characters/CharacterTemplate", order = 0)]
public class CharacterTemplate : ScriptableObject
{
  [SerializeField] public string Nome;
  [SerializeField] public Sprite Avatar;
  [TextArea(8, 4)][SerializeField] public string Bio;
}
