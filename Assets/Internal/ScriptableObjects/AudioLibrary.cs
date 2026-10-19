using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine.Events;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/AudioLibrary", order = 1)]

    public class AudioLibrary : ScriptableObject
    {

        [Serializable]
        public class AudioDictionaryEntry
        {
            public string key;
            public AudioClip value;
        }

        [SerializeField]
        private List<AudioDictionaryEntry> inspectorDictionary;

        private Dictionary<string, AudioClip> myDictionary;

        public UnityEvent<string,Vector3> AudioEvent;


        private void Awake()
        {
            myDictionary = new Dictionary<string, AudioClip>();
            foreach (AudioDictionaryEntry entry in inspectorDictionary)
            {
                myDictionary.Add(entry.key, entry.value);
            }
            AudioEvent = new UnityEvent<string, Vector3>();
        }


        public AudioDictionaryEntry GetAtIndex(int index)
        {
            return inspectorDictionary[index];

        }

        public void PlayAudioEvent(string name, Vector3 pos)
        { 
            AudioEvent.Invoke(name, pos);
        }

        public bool TryGetClip(string key, out AudioClip reference)
        {
            reference = null;
            if (myDictionary == null)
            {
                myDictionary = new Dictionary<string, AudioClip>();
                foreach (AudioDictionaryEntry entry in inspectorDictionary)
                {
                    myDictionary.Add(entry.key, entry.value);
                }
            }
            if (myDictionary.ContainsKey(key))
            {
                reference = myDictionary[key];
                return true;
            }
            return false;
        }

    }
}