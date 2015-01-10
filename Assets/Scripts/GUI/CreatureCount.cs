using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreatureCount : MonoBehaviour {

	public int number_of_creatures = 0;
	public Text text;

	void Update ()	{
		text.text = "Creatures: " + number_of_creatures;
	}

}
