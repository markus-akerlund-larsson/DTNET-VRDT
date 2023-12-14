using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Meta.WitAi.TTS.Utilities;
using System.Linq;
using ChatGPT_Patient;
using OpenAI;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

/*
From Mondat-Task: "ChatCharacterComponent"
Pull together the prompt form all the other objects

A MonoBehviour
initializes the ChatHistory and EmbeddingDB objects in start method

has a private method "ProducePrompt(string message) with return type List<ChatMessage> and input parameter string
*/
namespace AICharacter
{
    public class ChatGPTController : MonoBehaviour
    {
        
        //[SerializeField] private InputField inputField;
        [SerializeField] private Text playerUtterance;
        [SerializeField] private Text textArea;
        [SerializeField] private Toggle pauseOnCommas;

        [SerializeField] private TTSSpeaker _speaker;
        //[SerializeField] private string Instruction;

        [SerializeField] private WitAutoReactivation WitReact;
        [SerializeField] private InteractionHandler InterHandler;


        [SerializeField] private AudioClip[] WaitingPhrases;

        private RandomPool<AudioClip> PhrasesPool; 

        private string[] sentences;
        private List<string> ChatHistory;
        [SerializeField] private int PhrasesToSend = 4;
        int instrLen;
        [SerializeField] private bool JustTestingDontSend = false;

        private int secret = 0;
        string[] errorReplies = { "Chat GPT is down (or we didn't pay for it) so have this:",
                    "We're no strangers to love",
                    "You know the rules and so do I (do I)",
                    "A full commitment's what I'm thinking of",
                    "You wouldn't get this from any other guy" };
        List<string> forbiddenPhrases = new List<string>
        {
            "Press activation to talk...",
            "Processing...", "", " "
        };

        private OpenAIApi openai = new OpenAIApi("redacted");

        private string userInput;
        private string finalInstruction;

        [SerializeField] private Character character;

        private List<ChatMessage> messages = new List<ChatMessage>();

        private void Awake()
        {
            character = new Character();
            //character.InitCharacter();
            string c = JsonConvert.SerializeObject(character);
            Debug.Log("=====v======");
            Debug.Log(c);
            Debug.Log("=====^======");

            ChatHistory = new List<string>();
            PhrasesPool = new RandomPool<AudioClip>(WaitingPhrases); 
        }

        void Start()
        {
            //finalInstruction = $"{Instruction}\nQ: ";
            //instrLen = finalInstruction.Length - 4;
        }

        private string ComposeFinalString(string charDesc)
        {
            string _finalString = charDesc; //Instruction;
            foreach (string _str in ChatHistory.Skip(Mathf.Max(0, ChatHistory.Count() - PhrasesToSend)))
            {
                _finalString += "\n" + _str;
            }
            _finalString += "\nElisa: ";
            return _finalString;
        }

        private string FullHistory()
        {
            string _finalString = "";
            foreach (string _str in ChatHistory)
            {
                Debug.Log(_str);
                _finalString += _str + '\n';
            }
            return _finalString;
        }

        private ChatMessage HandleFunctionCalling(FunctionCall? functionCall)
        {
            string functionName = functionCall?.Name;
            string msgContent = "";
            Debug.Log("functionName :: " + functionName);
            if (functionName == "report_pain_level") {
                Debug.Log("Add pain level to message");
                msgContent = character.contentPain;
            }
            Debug.Log("=================================");
            return new ChatMessage()
            {
                Role = "function",
                Name = functionName,
                Content = msgContent
            };
        }

        public async void SendReply()
        {
            userInput = InterHandler.LastNonNullPhrase;
            InterHandler.LastNonNullPhrase = "";
            if (forbiddenPhrases.Contains(userInput))
            {
                Debug.Log("Heard nothing / did not have enough time to process the speech");
                return;
            }

            WitReact.temporarilyIgnore = true;

            Debug.Log("userInput:: "+userInput);
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = userInput
            };

            //finalInstruction += $"{userInput}\nA: ";
            //ChatHistory.Add("Nurse: " + userInput);
            if (messages.Count == 0) newMessage.Content = character.Content + "\n" + userInput;
            messages.Add(newMessage);

            textArea.text = FullHistory() + "\n...";
            Debug.Log("Full chat history: " + FullHistory());
            playerUtterance.text = "";
            finalInstruction = ComposeFinalString(character.Content);

            if (!JustTestingDontSend)
            {
                // Step 1: send the conversation and available functions to GPT
                // Complete the instruction
                var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
                {
                    Model = "gpt-3.5-turbo-0613",
                    Messages = messages,
                    Functions = character.functionDescriptions
                });

                
                bool responseHasChoices = (completionResponse.Choices != null && completionResponse.Choices.Count > 0);
                if (responseHasChoices)
                {
                    var message = completionResponse.Choices[0].Message;
                    message.Content = message.Content.Trim();
                    
                    messages.Add(message);

                    string mgs = JsonConvert.SerializeObject(messages);
                    Debug.Log("::::  mgs  :::::::");
                    Debug.Log(mgs);
                    Debug.Log(":::: _mgs_ :::::::");

                    ChatHistory.Add("Elisa: " + message.Content);
                    textArea.text = FullHistory();
                    
                    StartCoroutine(PlayAndWait());
                    //WitReact.temporarilyIgnore = false;
                }
                else
                {
                    textArea.text = FullHistory();
                    Debug.LogWarning("No text was generated from this prompt.");
                    //finalInstruction += $"{errorReplies[secret]}\nQ: ";
                    _speaker.Speak(errorReplies[secret]);
                    secret++;
                    if (secret >= errorReplies.Length)
                        secret = 0;
                    //WitReact.temporarilyIgnore = false;
                }
                //inputField.enabled = true;
            }
            else
            {
                Debug.LogWarning("We're just testing, but if I was to send this to GPT, that would be: " + finalInstruction);
                sentences = new string[2] { "This is very cool", "tell me more"};
                StartCoroutine(PlayAndWait());
            }
        }
        IEnumerator PlayAndWait()
        {
            foreach (string sentence in sentences)
            {
                if (sentence == string.Empty)
                    continue;
                _speaker.AudioSource.clip = PhrasesPool.Draw();
                _speaker.AudioSource.Play();
                _speaker.Speak(sentence);
                
                while (!_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                while (_speaker.IsSpeaking)
                {
                    yield return 0;
                }
                WitReact.temporarilyIgnore = true;
            }
            WitReact.temporarilyIgnore = false;
            Debug.Log("Giving control back to the stt");
        }
    }
}
