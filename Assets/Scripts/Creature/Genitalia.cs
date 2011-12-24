using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class Genitalia : MonoBehaviour {
#pragma warning disable 0414
	private Logger lg;
	private Creature crt;
	private Creature other_crt;
	private Spawner spw;
#pragma warning restore 0414

	void Start () {
		lg = Logger.getInstance();
		crt = transform.parent.GetComponent<Creature>();
		spw = Spawner.getInstance();
	}
	
	/*
	 * Determine the GameObject colliding with the genital
	 * radius. If it's the genitalia of another creature
	 * log event and pass genes of both creatues to
	 * a function yet undetermined.
	 *
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.name == "Genital") {
			other_crt = col.transform.parent.gameObject.GetComponent<Creature>();
			spw.spawn(col.transform.position, col.transform.localEulerAngles);
			string mesg = "CRTB" + " " + crt.getID() + " " +
				other_crt.getID() + " " + Time.realtimeSinceStartup; 
			Debug.Log(mesg);
			lg.write(mesg);
		 }
	}*/
}
