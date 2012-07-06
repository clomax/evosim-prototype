using UnityEngine;
using System.Collections;

public class FoodbitCount : MonoBehaviour {

	public Ether eth;
	
	void Start() {
		name = "FoodbitCount";
		eth = GameObject.Find("Ether").GetComponent<Ether>();
	}

	void Update ()	{
		guiText.text = "Foodbits: " + eth.foodbitCount;
	}
}
