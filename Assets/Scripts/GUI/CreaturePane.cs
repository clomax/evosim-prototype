using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreaturePane : MonoBehaviour
{
    public Text Name;
    public Text Energy;
    public Text Age;

    public Image Root_Col;
    public Image Limb_Col;

    private UIElement ui_element;

    private Creature crt;

    void OnEnable ()
    {
        Selection.Selected += OnSelected;
    }

    void OnDisable()
    {
        Selection.Selected -= OnSelected;
    }

    void OnSelected (Creature c)
    {
        crt = c;
    }

    void Start ()
    {
        ui_element = GetComponent<UIElement>();
    }

    void Update ()
    {
        set_data(crt);
    }

    private void set_data(Creature c)
    {
        if (c)
        {
            Name.text = c.name;
            Energy.text = c.energy.ToString();
            Age.text = c.age.ToString();

            Root_Col.color = c.chromosome.colour;
            Limb_Col.color = c.chromosome.limb_colour;

            ui_element.make_visible();
        }
        else
        {
            ui_element.make_invisible();
        }
    }
}
