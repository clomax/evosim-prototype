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
	
	private Logger lg;
	private GameObject aperatus;
	private GameObject ether;
	
	void Start () {
		lg = Logger.getInstance();
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		ether = (GameObject)Instantiate(Resources.Load("Prefabs/Ether"));
	}

	void OnDestroy() {
		lg.close();
	}
	
}
