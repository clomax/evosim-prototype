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

    public void set_data(Creature c)
    {
        Name.text = c.name;
        Energy.text = c.energy.ToString();
        Age.text = c.age.ToString();

        Root_Col.color = c.chromosome.colour;
        Limb_Col.color = c.chromosome.limb_colour;
    }
}
