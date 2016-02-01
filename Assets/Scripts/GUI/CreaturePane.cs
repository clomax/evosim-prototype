using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class CreaturePane : MonoBehaviour
{
    public InputField Name;
    public Text Energy;
    public Text Age;
    public Text Offspring;
    public Text FoodEaten;
    public Text State;

    public Image Root_Col;
    public Image Limb_Col;

    private UIElement ui_element;

    private Button[] buttons;
    private bool crt_dead;
    public Creature crt;

    void OnEnable ()
    {
        Selection.Selected += OnSelected;
        Creature.CreatureDead += OnCreatureDeath;
    }

    void OnDisable()
    {
        Selection.Selected -= OnSelected;
        Creature.CreatureDead -= OnCreatureDeath;
    }

    void OnSelected (Creature c)
    {
        crt_dead = false;
        if (!c)
        {
            crt = null;
            ui_element.make_invisible();
            return;
        }
        crt = c;
        Name.text = c.name;
        foreach (var b in buttons)
        {
            b.interactable = true;
        }
    }

    void OnCreatureDeath (Creature c)
    {
        if (c == crt)
        {
            set_data(c);
            crt_dead = true;
            foreach (var b in buttons)
            {
                b.interactable = false;
            }
        }
    }

    void Start ()
    {
        ui_element = GetComponent<UIElement>();
        buttons = GetComponentsInChildren<Button>();
    }

    void Update ()
    {
        set_data(crt);
    }

    private void set_data(Creature c)
    {
        if (c)
        {
            Energy.text = c.energy.ToString("0.0");
            Age.text = c.age.ToString("0");
            Offspring.text = c.offspring.ToString();
            FoodEaten.text = c.food_eaten.ToString();

            StringBuilder sb = new StringBuilder(c.state.ToString());
            string state = 
                sb
                  .Replace("_"," ")
                  .ToString();

            state = char.ToUpper(state[0]) + state.Substring(1);
            State.text = state;

            Root_Col.color = c.chromosome.colour;
            Limb_Col.color = c.chromosome.limb_colour;

            ui_element.make_visible();
        }
    }

    public void kill ()
    {
        crt.kill();
        crt = null;
        ui_element.make_invisible();
    }
}
