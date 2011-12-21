using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class Mouth : MonoBehaviour {
	
	public Foodbit fb;
	public Creature crt;
	
	void Start () {
		crt = transform.parent.GetComponent<Creature>();
	}
	
	/*
	 * If a foobit enters the mouth; Omnomnom.
	 */
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Foodbit") {
			fb = col.GetComponent<Foodbit>();
			crt.eat(fb.getEnergy());
			fb.destroy();
		}
	}
}

