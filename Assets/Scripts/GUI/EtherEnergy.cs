using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		31.08.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

 
/*
 * Energy - Updates a GUIText object with the total amount of energy in the environment
 * 	for testing purposes. Attatch to a GUIText object.
 *	
 *		notes:
 *				> energy should be options read from a settings file
 */
 
public class EtherEnergy : MonoBehaviour {
	
	public Ether eth;
	
	void Update () {
		eth = GameObject.Find("Ether").GetComponent("Ether") as Ether;	
		guiText.text = "Ether energy: " + eth.getEnergy().ToString();
	}
}
