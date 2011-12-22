using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
 *
 */


public class GeneticsMain : MonoBehaviour {

	private Ether eth;	
	public static GameObject container;
	public static GeneticsMain instance;
	
	void Start () {
		//10 random genes
	}
	
	public GeneticsMain () {
		
	}
	
	public static GeneticsMain getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent(typeof(GeneticsMain)) as GeneticsMain;
		}
		return instance;
	}
}
