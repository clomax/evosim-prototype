using UnityEngine;
using System.Collections;

public class TotalEnergy : MonoBehaviour {
	
	public int total_energy;
	public Ether eth;
	
	void Update () {
		total_energy = 1000;
		guiText.text = "Total energy: " + total_energy;
	}
}
