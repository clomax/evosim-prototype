using UnityEngine;
using System.Collections;

public class Siminfo : MonoBehaviour {

    public UIElement ui_element;

    void Start ()
    {
        ui_element = GetComponent<UIElement>();
    }

	void Update () {
	    if(Input.GetKeyUp(KeyCode.F3))
        {
            ui_element.ToggleVisibility();
        }
	}
}
