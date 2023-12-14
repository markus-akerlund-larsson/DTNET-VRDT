using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowcaseSign : MonoBehaviour
{
    [SerializeField] Text text;
    [field: SerializeField] public string Tag { get; private set; }

    public void Fill(string _text)
    {
        text.text = _text;
    }
}
