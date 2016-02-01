using UnityEngine;
using UnityEngine.UI;
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
	public Text text;
	
	void Update () {
		eth = Ether.getInstance();	
		text.text = "Ether energy: " + eth.getEnergy().ToString("F1");
	}
}
