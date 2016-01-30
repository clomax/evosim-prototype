using UnityEngine;
using LitJson;
using System.Collections;

public class ToolsButton : MonoBehaviour
{
    public UIElement ui_element;

    public void OnClick ()
    {
        ui_element.ToggleVisibility();
    }
}
