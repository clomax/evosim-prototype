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

	public Logger lg;
	public Creature crt;
	public Creature other_crt;

	void Start () {
		lg = Logger.getInstance();
		crt = transform.parent.GetComponent<Creature>();
	}
	
	void OnTriggerEnter (Collider col) {
		other_crt = col.gameObject.GetComponent<Creature>();
		if (col.gameObject.name == "Genital") {
			string mesg = "CRTB" + " " + crt.getID() + " " + other_crt.getID() + " " + Time.realtimeSinceStartup; 
			Debug.Log(mesg);
			lg.write(mesg);
		 }
	}
}
