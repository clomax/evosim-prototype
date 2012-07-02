using UnityEngine;
using System.Collections;

public class CreatureCount : MonoBehaviour {

	public int number_of_creatures = 0;
	
	void Update ()	{
		guiText.text = "Creatures: " + number_of_creatures;
	}
}
