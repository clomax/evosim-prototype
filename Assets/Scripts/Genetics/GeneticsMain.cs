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
	private Spawner spw;
	public static GameObject container;
	public static GeneticsMain instance;
	
	void Start () {
		spw = Spawner.getInstance();
		spw.spawn(new Vector3(0,5,0), new Vector3(0,0,0));
		spw.spawn(new Vector3(50,5,0), new Vector3(0,180,0));
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
