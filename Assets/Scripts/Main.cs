using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Main : MonoBehaviour {

	#pragma warning disable 0414
	 Logger lg;
	 Settings settings;
	 Selection selectionManager;
	 Spawner spw;
	 GeneticsMain gm;
	 CollisionMediator co;

	 GameObject aperatus;
	 GameObject cam;
	 Ether ether;
#pragma warning restore 0414

	/*
	 * Instantiate all necessary objects, attach and configure
	 * Components as needed.
	 */
	void Start () {		
		lg = Logger.getInstance();
		settings = Settings.getInstance();
		selectionManager = Selection.getInstance();
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		cam = GameObject.Find("Main Camera");
		cam.AddComponent<CameraCtl>();
		ether = Ether.getInstance();
		co = CollisionMediator.getInstance();
		spw = Spawner.getInstance();
		gm = GeneticsMain.getInstance();
	}
	
}
