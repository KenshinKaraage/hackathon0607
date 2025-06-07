using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;
    [TextArea(1, 3)] public string description;
    public Sprite imageSprite;
    public PromptCharacter characterPrompt;
}
