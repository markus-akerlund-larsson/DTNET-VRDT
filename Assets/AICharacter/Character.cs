using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi.TTS.Utilities;
using OpenAI;


/*
# From Monday task:
name
description
voice settings?
male/female
age?

"Thinking phrases" List of "hmmm"s and "let me think"s
*/
namespace AICharacter
{
    [System.Serializable]
    public class FunctionContent
    {
        public string key;
        public string value;
    }

    [System.Serializable]
    public class Character
    {
        public string Name = "Elisa";
        public string role = "assistant";
        public string Content = "Act as a random person named Elisa.";
        //public int Age;
        //public string Gender;
        public TTSSpeaker VoiceSpeaker;
        public AudioClip[] ThinkingPhrases;
        
        // !!! Change this into something more Dynamic.. !!!
        public List<FunctionDescription> functionDescriptions;
        public string contentPain = "{\"pain\":\"9\"}";
        //[SerializeField] public FunctionContent functionContent;


        public void Init()
        {
            Debug.Log("Init Character :: "+Name);
            InitTestFunctionDescription(); // For demon. (Make this more dynamic)
        }

        public void AddFunctionDescription(FunctionDescription functionDescription) 
        {
            functionDescriptions.Add(functionDescription);
        }

        public void Speak(string sentance)
        {
            VoiceSpeaker.Speak(sentance);
        }

        private void InitTestFunctionDescription() 
        {
            functionDescriptions = new List<FunctionDescription>
            {
                new FunctionDescription
                {
                    Name = "report_pain_level",
                    Description = "Patient's pain as a number between 1 and 10.",
                    Parameters = new Parameters
                    {
                        Type = "object",
                        Properties = new Dictionary<string, Property>
                        {
                            {
                                "Pain", new Property
                                {
                                    Type = "number",
                                    Description = "A number from one to ten"
                                }
                            },
                            {
                                "Reponse", new Property
                                {
                                    Type = "string",
                                    Description = "The in-character response. How the patient responds to the question."
                                }
                            }
                        },
                        Required = new List<string> { "pain", "reponse" }
                    }
                }
            };
        }
    }

}
