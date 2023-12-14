using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridController : MonoBehaviour
{
    public TextMeshProUGUI textDescription;
    [HideInInspector]
    public List<RowSentence> rowsSentence = new List<RowSentence>();
    [HideInInspector]
    public List<RowSentence> rowsChoose = new List<RowSentence>();
    public Transform contentSentence;
    public Transform contentChoose;
    [SerializeField] ButtonSentence buttonWord;
    public RowSentence rowSentence;
    public RowSentence rowChoose;

    [SerializeField] float contentWidth;
    SentenceScrambleTab sentenceScrambleTab;
    // Start is called before the first frame update
    void Awake()
    {
        sentenceScrambleTab = GetComponent<SentenceScrambleTab>();
        //rowsSentence.Add(Instantiate(rowSentence, contentSentence));
        //rowsChoose.Add(Instantiate(rowChoose, contentChoose));
    }
   

    float GetRowLenght(RowSentence row)
    {
        float lenght = 0;
        for (int i = 0; i < row.word.Count; i++)
            lenght = lenght + row.word[i].GetComponent<RectTransform>().rect.width;

        lenght = lenght + (2.5f * (row.word.Count - 1));
        return lenght;
    }

    public void InstantiateSentenceWord(string text)
    {
        RowSentence rowSentence = rowsSentence[rowsSentence.Count - 1];
        ButtonSentence word = Instantiate(buttonWord, rowSentence.spawnPoint.transform.parent);
        word.GetComponent<ButtonSentence>().inSenntence = true;
        word.GetComponentInChildren<TextMeshProUGUI>().text = text;
        //rowSentence.word.Add(word.GetComponent<ButtonSentence>());
        //sentenceScrambleTab.UpdateSentence();
    }

    public void InstantiateWordChoose(string text)
    {
        RowSentence rowChoose = rowsChoose[rowsChoose.Count - 1];
        ButtonSentence word = Instantiate(buttonWord, rowChoose.spawnPoint.transform.parent);
        word.GetComponentInChildren<TextMeshProUGUI>().text = text;
        rowChoose.word.Add(word.GetComponent<ButtonSentence>());
    }
    public void Remove(ButtonSentence buttonSentence)
    {
        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].word.Count; j++)
            {
                if (rowsSentence[i].word[j] == buttonSentence)
                {
                    Destroy(buttonSentence.gameObject);
                    rowsSentence[i].word.RemoveAt(j);
                    if (i== rowsSentence.Count-1)
                    {
                        if (rowsSentence[i].word.Count == 0 && i>0)
                        {
                            Destroy(rowsSentence[i].gameObject);
                            rowsSentence.Remove(rowsSentence[i]);
                            return;
                        }
                        if (rowsSentence[i].word.Count > 0)
                        {
                            UpdatePostions();
                        }
                    }
                    if (i < rowsSentence.Count - 1)
                    {
                        if (GetRowLenght(rowsSentence[i]) + rowsSentence[i + 1].word[0].GetComponent<RectTransform>().rect.width < contentWidth)
                        {
                            rowsSentence[i + 1].word[0].transform.parent = rowsSentence[i].transform;
                            rowsSentence[i + 1].word[0].transform.position = rowsSentence[i].word[rowsSentence[i].word.Count - 1].spawnPoint.transform.position;
                            rowsSentence[i].word.Add(rowsSentence[i + 1].word[0]);
                            rowsSentence[i + 1].word.RemoveAt(0);

                            UpdatePostions();
                        }
                        else
                        {
                            UpdatePostions();
                        }
                    }
                }
            }
        }
    }

    public void UpdatePostions() 
    {
        for (int i = 0; i < rowsSentence.Count; i++)
        {
            for (int j = 0; j < rowsSentence[i].word.Count; j++)
            {
                if (j == 0)
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].spawnPoint.transform.position;
                else
                    rowsSentence[i].word[j].transform.position = rowsSentence[i].word[j - 1].spawnPoint.transform.position;
            }
        }
    }
    public void UpdatePostionsWordChoose(List<RowSentence> row) 
    {
        for (int i = 0; i < row.Count; i++)
        {
            if (GetRowLenght(row[i]) < contentWidth)
            {
                for (int j = 0; j < row[i].word.Count; j++)
                {
                    if (j == 0)
                        row[i].word[j].transform.position = row[i].spawnPoint.transform.position;
                    else
                        row[i].word[j].transform.position = row[i].word[j - 1].spawnPoint.transform.position;
                }
                row[i].spawnPoint.transform.parent.transform.localPosition = new Vector3(GetRowLenght(row[i])  / -2, 0, 0);
            }

            if (GetRowLenght(row[i]) >= contentWidth)
            {
                rowsChoose.Add(Instantiate(rowChoose, contentChoose));
                row[i].word[row[i].word.Count - 1].transform.parent = row[i+1].spawnPoint.transform.parent;
                row[i].word[row[i].word.Count - 1].transform.position = row[i+1].spawnPoint.transform.position;

                row[i + 1].word.Add(row[i].word[row[i].word.Count - 1]);
                row[i].word.RemoveAt(row[i].word.Count - 1);
            }
        }

        if (rowsChoose[rowsChoose.Count-1].word.Count==0)
        {
            Destroy(rowsChoose[rowsChoose.Count - 1].gameObject);
            rowsChoose.RemoveAt(rowsChoose.Count - 1);
        }
    }
   

    public void UpdatePostionsNewWordSentence(RowSentence row, ButtonSentence word)
    {
        if (contentWidth > GetRowLenght(row))
        {
            if (rowsSentence[rowsSentence.Count - 1].word.Count==0)
                word.transform.position = rowsSentence[rowsSentence.Count - 1].spawnPoint.transform.position;
            else
                word.transform.position = rowsSentence[rowsSentence.Count - 1].word[rowsSentence[rowsSentence.Count - 1].word.Count - 1].spawnPoint.transform.position;
           
            word.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
            rowsSentence[rowsSentence.Count - 1].word.Add(word.GetComponent<ButtonSentence>());

            sentenceScrambleTab.UpdateSentence();
            return;

        }
        if (contentWidth < GetRowLenght(row))
        {
            rowsSentence.Add(Instantiate(rowSentence, contentSentence));

            word.transform.position = rowsSentence[rowsSentence.Count - 1].spawnPoint.transform.position;
            word.transform.parent = rowsSentence[rowsSentence.Count - 1].transform;
            rowsSentence[rowsSentence.Count - 1].word.Add(word.GetComponent<ButtonSentence>());

            sentenceScrambleTab.UpdateSentence();
            return;
        }
    }


}
