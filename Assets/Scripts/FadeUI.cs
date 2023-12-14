using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] List<CanvasGroup> canvasGroups;
    [SerializeField] float speedMultiplier = 1;
    public bool fadeOut,fadeIn;

    void Start()
    {
        for (int i = 0; i < canvasGroups.Count; i++)
        {
            GraphicRaycaster _raycaster;
            if(canvasGroups[i].TryGetComponent<GraphicRaycaster>(out _raycaster))
                _raycaster.enabled = false;
            canvasGroups[i].alpha = 0;
        }
    }

    public void FadeOut() 
    {
        fadeOut = true;
    }
    public void FadeIn()
    {
        fadeIn = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            fadeIn = false;
            for (int i = 0; i < canvasGroups.Count; i++)
            {
                if (canvasGroups[i].alpha > 0)
                {
                    canvasGroups[i].alpha -= 1f * Time.deltaTime * speedMultiplier;
                    GraphicRaycaster _raycaster;
                    if (canvasGroups[i].TryGetComponent<GraphicRaycaster>(out _raycaster))
                        _raycaster.enabled = false;
                }
            }
            if (canvasGroups[canvasGroups.Count-1].alpha<=0)
            {
                fadeOut = false;
            }
        }

        if (fadeIn)
        {
            fadeOut = false;
            for (int i = 0; i < canvasGroups.Count; i++)
            {
                if (canvasGroups[i].alpha < 1)
                {
                    canvasGroups[i].alpha += 1f *Time.deltaTime * speedMultiplier;
                    GraphicRaycaster _raycaster;
                    if (canvasGroups[i].TryGetComponent<GraphicRaycaster>(out _raycaster))
                        _raycaster.enabled = true;
                }
            }
            if (canvasGroups[canvasGroups.Count - 1].alpha >= 1)
            {
                fadeIn = false;
            }
        }
    }
}
