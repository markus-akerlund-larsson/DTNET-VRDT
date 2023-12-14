using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NaughtyAttributes;
using OpenAI;
using UnityEngine;

namespace AICharacter
{
    public class EmbeddingDB : MonoBehaviour
    {
        [SerializeField] private string prompt;
        [SerializeField] private string key;
        [SerializeField] private int chunkLength;
        [SerializeField] private Dictionary<string, List<float>> _embeddings;
        public int returnTop;



        private String model = "text-embedding-ada-002";
        private OpenAIApi openai;
        void Start()
        {
            openai = new OpenAIApi("redacted");
        }

        void Update()
        {

        }

        public static List<string> window(string s, int winsize)
        {
            string[] _sentences = s.Split(".", StringSplitOptions.RemoveEmptyEntries);
            int _sentenceIter = 0;
            int _sentencesTotal = _sentences.Length;
            var res = new List<string>();
            while (_sentenceIter < _sentencesTotal)
            {
                int _currentWin = winsize;
                int _currentSentence = _sentenceIter;
                string _currentString = "";
                while ((_currentWin > 0) && (_currentSentence < _sentencesTotal))
                {
                    _currentWin -= _sentences[_currentSentence].Trim().Length + 2;
                    _currentString += _sentences[_currentSentence].Trim() + ". ";
                    _currentSentence++;
                }
                if (((_currentSentence - _sentenceIter) == 1) || _currentSentence == _sentencesTotal)
                {
                    _currentSentence += 1;
                }
                _sentenceIter = _currentSentence - 1;

                res.Add(_currentString);
            }

            foreach(string _currentString in res)
            {
                Debug.Log(_currentString);
            }
            
            return res;
        }

        public static double Dot(List<float> a, List<float> b)
        {
            if (a.Count != b.Count)
            {
                Debug.Log("Dot product can only be calculated for vector of the same length");
                return float.NaN;
            }
            double sum = 0;
            for (var i = 0; i < a.Count; ++i)
            {
                sum += a[i] * b[i];
            }
            return sum;
        }
    
        public static double VectorSimilarity(List<float> a, List<float> b)
        {
            return Math.Abs(Dot(a, b));
        }

        [Button("Generate DB")]
        void GenerateDB()
        {
            var wins = window(prompt, chunkLength);
            if (_embeddings == null) _embeddings = new Dictionary<string, List<float>>();
            if (openai == null) openai = new OpenAIApi(key);
            _embeddings.Clear();
            int rescount = 0;
            Debug.Log(wins.Count +" embeddings creations scheduled");
            foreach (string s in wins)
            {
                openai.CreateEmbeddings(new CreateEmbeddingsRequest()
                {
                    Input = s,
                    Model = model
                }).ContinueWith(r =>
                {
                    _embeddings.Add(s, r.Result.Data[0].Embedding);
                    Debug.Log("Window "+(rescount++)+": Embeddings creation done");
                });
            }
        

        }

        public Task<List<float>> GetEmbedding(string message)
        {
            if (openai == null) openai = new OpenAIApi(key);

            return openai.CreateEmbeddings(new CreateEmbeddingsRequest()
            {
                Input = message,
                Model = model
            }).ContinueWith(r => r.Result.Data[0].Embedding);
        }
    
        List<string> Search(List<float> embedding, int topN)
        {
            if (_embeddings == null) return null;

            var searchResult = _embeddings.OrderBy(kv => -VectorSimilarity(kv.Value, embedding)).ToList();
            var res = new List<string>();
            for (int i = 0; i < topN; ++i)
            {
                res.Add(searchResult[i].Key);
            }

            return res;
        }
    }
}
