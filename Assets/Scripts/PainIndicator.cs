using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainIndicator : MonoBehaviour
{
    [Range(0, 11)] 
    public int painCount;
    int _painCount;
    [SerializeField] Color [] colorPain;
    Material material;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<Renderer>().material;
        material.color = colorPain[0];
    }
    public void SetPain(int paintCount) 
    {
        material.color = colorPain[painCount];
    }
    // Update is called once per frame
    void Update()
    {
        if (painCount!=_painCount)
        {
            SetPain(painCount);
            _painCount = painCount;
        }
 
        //if (material.color. <= 0.2f)
        //{
        //    material.color = new Vector4(material.color.r, 0.5f + (painCount * 1.25f), material.color.b, material.color.a);
        //    //material.color = new Vector4(painCount, material.color.g, material.color.b, material.color.a);
        //}
        //if (painCount > 20 && painCount <= 40)
        //{
        //    material.color = new Vector4(0+((painCount * 8)-20), 200 + ((painCount * 2.75f)-20), material.color.b, material.color.a);
        //}

    }
}
