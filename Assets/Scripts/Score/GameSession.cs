using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public class GameSession<TKey, TValue> : ISerializationCallbackReceiver
{
   [SerializeField]
   private List<TKey> keys= new List<TKey>();

   [SerializeField]
   private List<TValue> values = new List<TValue>();

   public Dictionary<TKey, TValue> Dictionary = new Dictionary<TKey, TValue>();

     public void OnBeforeSerialize()
     {
          keys.Clear();
          values.Clear();
          foreach(KeyValuePair<TKey, TValue> pair in Dictionary)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
     }

     public void OnAfterDeserialize()
     {
          Dictionary.Clear();
          for(int i = 0; i< keys.Count; i++)
        {
            if(i < values.Count)
            {
                Dictionary[keys[i]] = values[i];
            }
        }
     }
}
