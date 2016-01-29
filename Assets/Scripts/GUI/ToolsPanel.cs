using UnityEngine;
using System.Collections;

public class ToolsPanel : MonoBehaviour
{
    public UIElement ui_element;

    void Start()
    {
        ui_element = GetComponent<UIElement>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F2))
        {
            ui_element.ToggleVisibility();
        }
    }
}
