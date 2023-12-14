using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AICharacter
{
    public class CharacterInfo : MonoBehaviour
    {
        public string name;
        public string description;
        public RandomPool<AudioClip> ThinkingPhrasesPool;
        [SerializeField] private AudioClip[] thinkingPhrases;

        private void Awake()
        {
            ThinkingPhrasesPool = new RandomPool<AudioClip>(thinkingPhrases);
        }
    }
}
