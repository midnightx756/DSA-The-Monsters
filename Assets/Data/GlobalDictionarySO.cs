using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalDictionary", menuName = "Data/Global Dictionary ")]
public class GlobalDictionarySO : ScriptableObject
{
    [SerializeField]
    private GameSession<string, AudioClip> myDictionaryData = new GameSession<string, AudioClip>();
      //private GameSession<string, Duragon> myDictionaryData = new GameSession<string, Duragon>();
    public Dictionary<string, AudioClip> Runtimezdict => myDictionaryData.Dictionary;
     //public Dictionary<string, Duragon> Runtimezdict => myDictionaryData.Dictionary;
}
