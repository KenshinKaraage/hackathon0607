using UnityEngine;
using System.Collections.Generic;

public class CharacterDataList : MonoBehaviour
{
    [SerializeField] private CharacterData[] characterDatas;
    public CharacterData[] CharacterDatas => (CharacterData[])characterDatas.Clone();
}
