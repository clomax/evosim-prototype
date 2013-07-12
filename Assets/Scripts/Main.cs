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
	// Logger lg;
	 Settings settings;
	 Spawner spw;
	 GeneticsMain gm;
	 CollisionMediator co;

	 GameObject aperatus;
	 GameObject cam;
	 GameObject plane;
	 GameObject ether;
	 GameObject _catch;
	 GameObject nrg_ether;
	 GameObject gui_text;
	
	 MeshRenderer p_mr;
	#pragma warning restore 0414
	
	/*
	 * Instantiate all necessary objects, attach and configure
	 * Components as needed.
	 */
	void Start () {		
		//lg = Logger.getInstance();
		settings = Settings.getInstance();
		print( settings.read() );
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		cam = GameObject.Find("Main Camera");
		cam.AddComponent("CameraCtl");
		plane = GameObject.Find("Plane");
		p_mr = (MeshRenderer)plane.AddComponent("MeshRenderer");
		p_mr.material = (Material)Resources.Load("Materials/grid");
		p_mr.material.mainTextureScale = new Vector2(50,50);
		_catch = GameObject.Find("Catch");
		_catch.AddComponent("Catch");
		ether = (GameObject)Instantiate(Resources.Load("Prefabs/Ether"));
		ether.AddComponent("Ether");
		gui_text = (GameObject)Instantiate(Resources.Load("Prefabs/GUItext"));
		
		co = CollisionMediator.getInstance();
		spw = Spawner.getInstance();
		gm = GeneticsMain.getInstance();
	}
	
	/*
	 * Cleanup before exiting
	 */
	void OnDestroy() {
		//lg.close();
	}
	
}
