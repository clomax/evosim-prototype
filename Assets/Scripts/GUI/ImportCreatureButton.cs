using UnityEngine;
using LitJson;
using System.Collections;

public class ImportCreatureButton : MonoBehaviour
{
    public UIElement import_creature_panel;

    public void OnClick ()
    {
        import_creature_panel.ToggleVisibility();
    }
}
