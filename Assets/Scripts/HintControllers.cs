using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using UnityEngine;

public class HintControllers : MonoBehaviour
{
    public GameObject modelHand;
    bool loadOnStart;
    public bool testPress;
    // Start is called before the first frame update
    void Start()
    {
      XRControllerEvent xRControllerEvent = GetComponent<XRControllerEvent>();
      //xRControllerEvent.link = FindObjectOfType<AutoHandPlayer>().handLeft.GetComponent<XRHandControllerLink>();
      //xRControllerEvent.button = CommonButton.menuButton;
      xRControllerEvent.Pressed.AddListener(PressButton);

        //if (!loadOnStart)
        //{
        //    transform.GetChild(0).gameObject.SetActive(true);
        //    modelHand.SetActive(false);
        //    loadOnStart = true;
        //}
    }
    public void EnableHint() 
    {
        transform.GetChild(0).gameObject.SetActive(true);
        modelHand.SetActive(false);
    }
    public void EnableHand()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        modelHand.SetActive(true);
    }

    void PressButton() 
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        modelHand.SetActive(!modelHand.activeSelf);
    }

    private void Update()
    {
        if (testPress)
        {
            PressButton();
            testPress = false;
        }
    }
}
