using System.Collections;
using System.Collections.Generic;
using AICharacter;
using OpenAI;
using UnityEngine;
using UnityEngine.UI;

public class GrammarFeedbackCalc : MonoBehaviour
{
    public ChatCharacter cchar;

    private OpenAIApi _openAI;
    public Text text;
    private bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        _openAI = new OpenAIApi("redacted");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Activate()
    {
        Debug.Log("button click sent");
        
        StartCoroutine("RequestFeedback");
    }
    
    public IEnumerator RequestFeedback()
    {
        Debug.Log("request sent");
        var messages = cchar._history.GetHistory(20);
        foreach (var chatMessage in messages)
        {
            Debug.Log(chatMessage.Content);
        }
        var instruction = new ChatMessage()
        {
            Role = "user",
            Content = "Pick out the top 3 grammar errors I have made during our conversation, excluding this last message. If there are not three major ones you can give advice on 2, 1 or even none if you find no big issues. Focus on word choices rather than things like if there are commas missing or in the wrong place. Give examples of how to correct them. Format the response nicely, with lines breaks between the three corrections. Start with \"Here are some points where your grammar could be improved:\"",
        };

        messages.Add(instruction);
        var completionTask = _openAI.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model ="gpt-3.5-turbo-0613",
            Messages = messages,
        });

        
        Debug.Log("waiting on response started");
        while (!completionTask.IsCompleted)
        {
            yield return null;
        };

        var response = completionTask.Result.Choices[0].Message;
        text.text = response.Content;
        Debug.Log("response recieved: "+response.Content);
        running = false;


    }
}
