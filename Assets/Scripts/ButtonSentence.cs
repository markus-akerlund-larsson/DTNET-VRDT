using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSentence : MonoBehaviour
{
    public TextMeshProUGUI text;
    public bool inSenntence;
    SentenceScrambleTab sentenceScrambleTab;
    public string variant;
    public GameObject spawnPoint;
    GridController gridController;
    float buttonWidth;
    ButtonSentence thisbButton;


    // Start is called before the first frame update
    void Start()
    {
        sentenceScrambleTab = FindObjectOfType<SentenceScrambleTab>();
        variant = GetComponentInChildren<TextMeshProUGUI>().text;
        gridController = FindObjectOfType<GridController>();
        buttonWidth = GetComponent<RectTransform>().rect.width;
        GetComponent<Button>().onClick.AddListener(OnClick);
  
        //if (inSenntence)
        //    gridController.UpdatePostionsNewWordSentence(gridController.rowsSentence[gridController.rowsSentence.Count - 1], this);
        //if (!inSenntence)
        //    gridController.UpdatePostionsWordChoose(gridController.rowsChoose);
    }

    void OnClick() 
    {
        if (inSenntence)
        {
            gridController.Remove(this);
            for (int i = 0; i < gridController.rowsChoose.Count; i++)
            {
                for (int j = 0; j < gridController.rowsChoose[i].word.Count; j++)
                {
                    if (GetComponentInChildren<TextMeshProUGUI>().text == gridController.rowsChoose[i].word[j].GetComponentInChildren<TextMeshProUGUI>(true).text)
                    {
                        gridController.rowsChoose[i].word[j].gameObject.SetActive(true);
                        gridController.rowsChoose[i].word[j].inSenntence = false;
                        sentenceScrambleTab.UpdateSentence();
                    }
                }
            }
        }

        if (!inSenntence)
        {
            //sentenceScrambleTab.SetVariant(variant);
            gridController.InstantiateSentenceWord(variant);
            GetComponent<Image>().color = Color.white;
            gameObject.SetActive(false);
            return;
        }

    }
    //public void pointerEnter()
    //{
    //    if (sentenceScrambleTab.buttonChoose != null && inConstructor)
    //    {
    //        sentenceScrambleTab.buttonChoose.variant = text.text;
    //        variant = sentenceScrambleTab.buttonChoose.text.text;


    //        //text.text = sentenceScrambleTab.buttonChoose.variant;
    //        //sentenceScrambleTab.buttonChoose.text.text = variant;

    //        //variant = text.text;
    //        //sentenceScrambleTab.buttonChoose.variant = sentenceScrambleTab.buttonChoose.text.text;

    //        GetComponent<Image>().color = Color.green;
    //    }
    //}
    //public void pointerExit()
    //{
    //    if (inConstructor )
    //    {

    //        if (sentenceScrambleTab.buttonChoose != null)
    //        {

    //            variant = text.text;
    //            sentenceScrambleTab.buttonChoose.variant = sentenceScrambleTab.buttonChoose.text.text;

    //            //variant = sentenceScrambleTab.buttonChoose.text.text;
    //            //sentenceScrambleTab.buttonChoose.variant = text.text;

    //            //text.text = variant;
    //            //sentenceScrambleTab.buttonChoose.text.text = sentenceScrambleTab.buttonChoose.variant;

    //            if (sentenceScrambleTab.buttonChoose != GetComponent<ButtonSentence>())
    //                GetComponent<Image>().color = Color.white;
    //        }
    //    }

    //    if (thisbButton!=null)
    //    {
    //        if (!sentenceScrambleTab.buttonChoose)
    //            sentenceScrambleTab.buttonChoose = GetComponent<ButtonSentence>();
    //    }
    //    thisbButton = null;

    //}
    //public void pointerUp()
    //{
    //    if (!inConstructor)
    //    {
    //        sentenceScrambleTab.SetVariant(variant);
    //        GetComponent<Image>().color = Color.white;
    //        gameObject.SetActive(false);
    //        //Destroy(gameObject);
    //        return;
    //    }

    //    if (inConstructor && !sentenceScrambleTab.buttonChoose && thisbButton)
    //    {
    //        gridController.Remove(this);
    //        //sentenceScrambleTab.ReturnVariant(variant);

    //        for (int i = 0; i < gridController.rowsChoose.Count; i++)
    //        {
    //            for (int j = 0; j < gridController.rowsChoose[i].word.Count; j++)
    //            {
    //                if (GetComponentInChildren<TextMeshProUGUI>().text == gridController.rowsChoose[i].word[j].GetComponentInChildren<TextMeshProUGUI>(true).text) 
    //                {
    //                    gridController.rowsChoose[i].word[j].gameObject.SetActive(true);
    //                    gridController.rowsChoose[i].word[j].inConstructor = false;
    //                    Debug.Log("Find");
    //                }

    //            }

    //        }

    //        Destroy(gameObject);

    //        return;
    //    }

    //    if (inConstructor && sentenceScrambleTab.buttonChoose)
    //    {
    //        var buttonsSentence = FindObjectsOfType<ButtonSentence>();
    //        for (int i = 0; i < buttonsSentence.Length; i++) 
    //        {
    //            buttonsSentence[i].GetComponent<Image>().color = Color.white;

    //            if (buttonsSentence[i].text.text != buttonsSentence[i].variant)
    //                buttonsSentence[i].text.text = buttonsSentence[i].variant;
    //        }

    //        sentenceScrambleTab.buttonChoose = null;
    //        return;
    //    }
    //}
    //public void pointerDown()
    //{
    //    GetComponent<Image>().color = Color.green;
    //    thisbButton = this;
    //}



    private void Update()
    {

        //if (buttonWidth != GetComponent<RectTransform>().rect.width)
        //{
        //    if (gridController.WordsLenghts(this) > gridController.contentWidth)
        //    {
        //        Debug.Log(3);
        //    }
        //    gridController.UpdatePostions();
        //    buttonWidth = GetComponent<RectTransform>().rect.width;
        //}
        if (buttonWidth != GetComponent<RectTransform>().rect.width)
        {
            if (inSenntence)
                gridController.UpdatePostionsNewWordSentence(gridController.rowsSentence[gridController.rowsSentence.Count-1],this);
            if (!inSenntence)
                gridController.UpdatePostionsWordChoose(gridController.rowsChoose);

            buttonWidth = GetComponent<RectTransform>().rect.width;
        }
    }
}

