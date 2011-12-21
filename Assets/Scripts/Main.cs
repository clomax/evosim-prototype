using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class Main : MonoBehaviour {

	public Transform ether;
	public static Logger lg;
	
	void Start () {
		lg = Logger.getInstance();
		Instantiate(ether.gameObject);
	}

	void OnDestroy() {
		lg.close();
	}
	
}
