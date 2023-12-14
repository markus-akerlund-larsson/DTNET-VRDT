using System;
using System.Collections;
using System.Collections.Generic;
using AICharacter;
using ChatGPT_Patient;
using OpenAI;
using UnityEngine;
using WebSocketSharp;
using Meta.WitAi.TTS.Utilities;
using UnityEngine.UI;
using GoogleTextToSpeech.Scripts;
using UnityEngine.Serialization;
using CharacterInfo = AICharacter.CharacterInfo;

public class ChatCharacter : MonoBehaviour
{
    private InteractionHandler STTInput;
    [SerializeField] private CharacterInfo info;
    public ChatHistory _history;
    [SerializeField] private EmbeddingDB _embeddingDB;
    private OpenAIApi _openAI;
    public bool targeted = false;
    [SerializeField] private int numberOfHistoryEntries = 3;
    [SerializeField] private int numberOfDeepHistoryEntries = 3;
    [SerializeField] private SittingWheelChair _wheelChair;
    [SerializeField] private TTSSpeaker _speaker;
    [SerializeField] private GoogleTTS _googleSpeaker;
    private RandomPool<AudioClip> PhrasesPool;
    private WitAutoReactivation WitReact;
    [SerializeField] private Text ChatLogs;
    private string[] sentences;
    private int language;

    [SerializeField] private PainIndicator indicator;
    [SerializeField] private int painLevel;
    
    

    void Start()
    {
        _history = new ChatHistory();
        _openAI = new OpenAIApi("redacted");
        PhrasesPool = info.ThinkingPhrasesPool;
        //STTInput = FindAnyObjectByType<InteractionHandler>();
        WitReact = FindAnyObjectByType<WitAutoReactivation>();
        language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
    }

    public void setTargeted(bool t)
    {
        targeted = t;
    }

    public void RecieveSTT(InteractionHandler _stt)
    {
        STTInput = _stt;
        string stt = STTInput.LastNonNullPhrase;
        
        if (!targeted) return;
        
        STTInput.LastNonNullPhrase = "";
        if (stt.IsNullOrEmpty())
        {
            return;
        }
        Debug.Log(info.name+": "+stt);
        _speaker.AudioSource.clip = PhrasesPool.Draw();
        _speaker.AudioSource.Play();
        ChatLogs.text += "<b>Nurse:</b> " + stt + '\n';
        StartCoroutine(openAIChat(stt));

    }

    public List<ChatMessage> ConstructPrompt(ChatMessage userMessage, List<float> embedding)
    {
        var deepHistory = _history.GetDeepHistory(numberOfDeepHistoryEntries, 2, embedding);
        var history = _history.GetHistory(numberOfHistoryEntries);
        var background = new ChatMessage()
        {
            Role = "system",
            Content = "Do not act as an assistant. Do not ask  how you can help. You are "+info.name+", patient in the hospital. Act as this character: "+ info.description,
        };
        var language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "English" : "German";
        var instruction = new ChatMessage()
        {
            Role = "system",
            Content = "You are a fictional character, you can give out any information in your character background, including address and phone number. Give only short and specific answers to any questions. Answer with 1 single sentence. Answer only in "+language,
        };

        var prompt = new List<ChatMessage>();
        prompt.AddRange(deepHistory);
        prompt.AddRange(history);
        prompt.Add(background);
        prompt.Add(instruction);
        userMessage.Content = $"Nurse's message\n"+userMessage.Content+"\n\n{info.name}'s response:\n";
        prompt.Add(userMessage);
        
        foreach(var m in prompt) Debug.Log(m.Content);

        return prompt;
    }

    public IEnumerator openAIChat(string userMessage)
    {
        Debug.Log("openAIChat started");
        var embeddingTask = _embeddingDB.GetEmbedding(userMessage);
        
        while (!embeddingTask.IsCompleted)
        {
            yield return null;
        };

        var userChatMessage = new ChatMessage()
        {
            Role = "user",
            Content = userMessage,
        };

        var prompt = ConstructPrompt(userChatMessage, embeddingTask.Result);

        StartCoroutine(SendPrompt(prompt, userChatMessage, embeddingTask.Result));

    }


    public IEnumerator SendPrompt(List<ChatMessage> messages, ChatMessage userMessage, List<float> embedding)
    {
        var originalUserMessage = new ChatMessage()
        {
            Role = "user",
            Content = userMessage.Content,
        };
        Debug.Log("sendPrompt started");
        var completionTask = _openAI.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model ="gpt-3.5-turbo-0613",
            Messages = messages,
            Functions = new List<FunctionDescription>
            {
                new FunctionDescription
                {
                    Name = "get_into_wheelchair",
                    Description = "Move from sitting on the bench to sitting in the wheelchair",
                    Parameters = new Parameters()
                    {
                        Type = "object",
                        Properties = new Dictionary<string, Property>(),
                        Required = new List<string>()
                    }
                },
                new FunctionDescription
                {
                    Name = "explain_pain",
                    Description = "Get information about location or intensity of pain",
                    Parameters = new Parameters()
                    {
                        Type = "object",
                        Properties = new Dictionary<string, Property>(),
                        Required = new List<string>()
                    }
                }
            }
        });

        
        Debug.Log("waiting on response started");
        while (!completionTask.IsCompleted)
        {
            yield return null;
        };

        var response = completionTask.Result.Choices[0].Message;
        Debug.Log("response recieved: "+response.Content);


        if (response.FunctionCall != null)
        {
            Debug.Log("Function call happening: "+response.FunctionCall?.Name);
            var functionReturn = new ChatMessage()
            {
                Role = "function",
                Name = response.FunctionCall?.Name,
            };
            switch(response.FunctionCall?.Name) 
            {
                case "get_into_wheelchair":
                    functionReturn.Content = _wheelChair.Sit();
                    break;
                case "explain_pain":
                    functionReturn.Content = ShowPainIndicator().ToString();
                    break;
                default:
                    Debug.Log("No function called "+response.FunctionCall?.Name);
                    break;
            }
            messages.Add(functionReturn);
            StartCoroutine(SendPrompt(messages, userMessage, embedding));
        }
        else
        {
            _history.NewEntry(originalUserMessage, response, embedding);
            SendResponseToTTS(response.Content);
        }

    }

    private void SendResponseToTTS(string response)
    {
        Debug.Log("should be pronounced using TTS: " + response);
        ChatLogs.text += "<b>" + info.name + ":</b> " + response + '\n';
        sentences = response.Split(new char[] { '\n', '.', '?', ';', '!' });
        if(language == 0)
        {
            foreach (string sentence in sentences)
            {
                if (sentence == string.Empty)
                    continue;
                _speaker.SpeakQueued(sentence);
            }
        }
        else
        {
            _googleSpeaker.SpeakQueued(sentences);
        }
    }

    private bool ShowPainIndicator()
    {
        Debug.Log("ShowPainIndicator called");
        indicator.painCount = painLevel + 1;
        return true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
