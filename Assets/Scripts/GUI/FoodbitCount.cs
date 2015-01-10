using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FoodbitCount : MonoBehaviour {
	public int fbit_count;
	public Text text;
	public Ether eth;
	
	void Start() {
		name = "FoodbitCount";
		eth = Ether.getInstance();
	}

	void Update ()	{
		fbit_count = eth.getFoodbitCount();
		text.text = "Foodbits: " + fbit_count;
	}
}
