using UnityEngine;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class CollEvent {
	private GameObject a;
	private GameObject b;
	
	public CollEvent (GameObject a, GameObject b) {
		this.a = a;
		this.b = b;
	}
	
	public GameObject[] getColliders() {
		GameObject[] evt = new GameObject[2];
		evt[0] = this.a;
		evt[1] = this.b;
		return evt;
	}
}
