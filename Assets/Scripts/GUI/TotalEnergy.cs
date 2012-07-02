using UnityEngine;
using System.Collections;

public class TotalEnergy : MonoBehaviour {
	
	public int total_energy;
	public Ether eth;
	
	void Update () {
		eth = GameObject.Find("Ether").GetComponent("Ether") as Ether;
		
		total_energy = eth.getEnergy();
			// + total foodbit energy + total creature energy
		guiText.text = "Total energy: " + total_energy;
	}
}
