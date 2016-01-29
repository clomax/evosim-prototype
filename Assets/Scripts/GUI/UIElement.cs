using UnityEngine;
using System.Collections;

public class UIElement : MonoBehaviour {

    public bool visible = false;
    public bool passive;

    void Start ()
    {
        if (visible)
            make_visible();
        else
            make_invisible();
    }

    public void ToggleVisibility()
    {
        if (!visible)
        {
            make_visible();
        }
        else
        {
            make_invisible();
        }
    }

    public void make_invisible()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0;
        visible = false;
    }

    public void make_visible()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = true;
        if(!passive)
            cg.blocksRaycasts = true;
        cg.alpha = 1;
        visible = true;
    }
}
