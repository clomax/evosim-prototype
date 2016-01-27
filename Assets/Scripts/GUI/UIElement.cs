using UnityEngine;
using System.Collections;

public class UIElement : MonoBehaviour {

    public bool visible = true;

    public void ToggleVisibility()
    {
        if (!visible)
        {
            make_visible();
            visible = true;
        }
        else
        {
            make_invisible();
            visible = false;
        }
    }

    public void make_invisible()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = false;
        cg.blocksRaycasts = false;
        cg.alpha = 0;
    }

    public void make_visible()
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        cg.interactable = true;
        cg.blocksRaycasts = true;
        cg.alpha = 1;
    }

}
