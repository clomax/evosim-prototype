using UnityEngine;
using System.Collections;

public class TotalEnergy : MonoBehaviour {
	
	public double total_energy;
	public Ether eth;
	
	void Start () {
		eth = Ether.getInstance();
	}
	
	void Update () {
		total_energy = eth.total_energy;
		guiText.text = "Total energy: " + total_energy;
	}
}
