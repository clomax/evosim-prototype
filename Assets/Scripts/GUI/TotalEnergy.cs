using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TotalEnergy : MonoBehaviour {
	
	public double total_energy;
	public Ether eth;
	public Text text;
	
	void Start () {
		eth = Ether.getInstance();
	}
	
	void Update () {
		total_energy = eth.total_energy;
		text.text = "Total energy: " + total_energy;
	}
}
