using System.Collections;
using System.Collections.Generic;
using ChatGPT_Patient;
using UnityEngine;
using OpenAI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AICharacter
{
    public class CharacterConversation : MonoBehaviour
    {
        [SerializeField] private WitAutoReactivation WitReact;
        [SerializeField] private InteractionHandler InterHandler;
        [SerializeField] private Character character;
        //private ChatHistory ChatHistory; 

        private List<ChatMessage> chatHistory;

        private string userInput;

        void Start()
        {
            character.Init();
            chatHistory = new List<ChatMessage>();
        }

        public string GetUserInput()
        {
            userInput = InterHandler.LastNonNullPhrase;
            InterHandler.LastNonNullPhrase = "";
            return userInput;
        }

        public List<ChatMessage> GetChathistory()
        {
            return chatHistory;
        }

        public int GetChatHistoryLenght()
        {
            return chatHistory.Count;
        }

        public bool HasChatHistory()
        {
            return chatHistory.Count > 0;
        }

        public string GetFullPromt()
        {
            string _finalString = "";
            foreach (ChatMessage message in chatHistory)
            {
                Debug.Log(message.Content);
                _finalString += message.Content + '\n';
            }
            return _finalString;
        }

        public string GetCharacterContent()
        {
            return character.Content;
        }

        public List<FunctionDescription> GetCharacterFunctionDescriptions()
        {
            return character.functionDescriptions;
        }

        public string GetCharacterFunctionContent()
        {
            return character.contentPain;
        }

        public void SetWitReactTemporarilyIgnore(bool state)
        {
            WitReact.temporarilyIgnore = state;
        }

        public void AddToChatHistory(ChatMessage message)
        {
            chatHistory.Add(message);
        }
    }
}
