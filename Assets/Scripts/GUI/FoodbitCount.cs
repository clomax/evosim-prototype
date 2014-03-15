using UnityEngine;
using System.Collections;

public class FoodbitCount : MonoBehaviour {
	public int fbit_count;

	public Ether eth;
	
	void Start() {
		name = "FoodbitCount";
		eth = Ether.getInstance();
	}

	void Update ()	{
		fbit_count = eth.getFoodbitCount();
		guiText.text = "Foodbits: " + fbit_count;
	}
}
