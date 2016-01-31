using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatureCount : MonoBehaviour
{
	public int number_of_creatures = 0;
	public Text text;

    void OnEnable ()
    {
        Creature.CreatureDead += OnDeath;
    }

    void OnDisable()
    {
        Creature.CreatureDead += OnDeath;
    }

    void OnDeath (Creature _x)
    {
        number_of_creatures -= 1;
    }

    void Update ()	{
		text.text = "Creatures: " + number_of_creatures;
	}

}
