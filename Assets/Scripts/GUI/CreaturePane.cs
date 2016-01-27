using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreaturePane : MonoBehaviour
{
    public Text Name;
    public Text Energy;
    public Text Age;
    public Text Offspring;
    public Text FoodEaten;
    public Text State;

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
            Energy.text = c.energy.ToString("#.0");
            Age.text = c.age.ToString("#.0");
            Offspring.text = c.offspring.ToString();
            FoodEaten.text = c.food_eaten.ToString();
            State.text = c.state.ToString();

            Root_Col.color = c.chromosome.colour;
            Limb_Col.color = c.chromosome.limb_colour;

            ui_element.make_visible();
        }
        else
        {
            ui_element.make_invisible();
        }
    }

    public void kill ()
    {
        crt.kill();
    }
}
