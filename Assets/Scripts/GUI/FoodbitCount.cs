using UnityEngine;
using System.Collections;

public class FoodbitCount : MonoBehaviour {

	public Ether eth;
	
	void Start() {
		name = "FoodbitCount";
		eth = Ether.getInstance();
	}

	void Update ()	{
		guiText.text = "Foodbits: " + eth.foodbitCount;
	}
}
