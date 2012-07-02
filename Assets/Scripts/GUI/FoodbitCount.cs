using UnityEngine;
using System.Collections;

public class FoodbitCount : MonoBehaviour {

	public Ether eth;
	
	void Start() {
		eth = GameObject.Find("Ether").GetComponent<Ether>();
	}

	void Update ()	{
		guiText.text = "Foodbits: " + eth.getFoodbitCount();
	}
}
