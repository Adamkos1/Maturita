using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AH
{
    [System.Serializable]
    public class SerializbleDictionary<Tkey, TValue> : Dictionary<Tkey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<Tkey> keys = new List<Tkey>();
        [SerializeField] private List<TValue> values = new List<TValue>();


        //zavola ho pre tym ako ho sreialajzne
        //ulozi dictionary do listu
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();

            foreach(KeyValuePair<Tkey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        //zavolany hned po serializacii
        //loadni dictionary z listu
        public void OnAfterDeserialize()
        {
            Clear();

            if(keys.Count != values.Count)
            {
                Debug.LogError("bratu skusili sme desrializjnut ten slovnik ale pocet klucov nie je rovny hodnote tych hodnot");
            }

            for(int i = 0; i < keys.Count; i++)
            {
                Add(keys[i], values[i]);
            }
        }
    }
}
