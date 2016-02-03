using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatureCount : MonoBehaviour
{
	public int number_of_creatures;
	Text text;

    void Start ()
    {
        number_of_creatures = 0;
        text = GetComponent<Text>();
    }

    void OnEnable ()
    {
        Spawner.CreatureSpawned += OnSpawn;
        Creature.CreatureDead += OnDeath;
    }

    void OnDisable()
    {
        Spawner.CreatureSpawned -= OnSpawn;
        Creature.CreatureDead += OnDeath;
    }

    void OnSpawn (Creature _x)
    {
        number_of_creatures += 1;
    }

    void OnDeath (Creature _x)
    {
        number_of_creatures -= 1;
    }

    void Update ()	{
		text.text = "Creatures: " + number_of_creatures;
	}

}
