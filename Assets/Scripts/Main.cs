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

	#pragma warning disable 0414
	private Logger lg;
	private Spawner spw;
	private GeneticsMain gm;
	private CollisionMediator co;

	private GameObject aperatus;
	private GameObject cam;
	private GameObject plane;
	private GameObject ether;
	private GameObject _catch;
	private GameObject nrg_ether;
	
	private MeshRenderer p_mr;
	#pragma warning restore 0414
	
	/*
	 * Instantiate all necessary objects, attach and configure
	 * Components as needed.
	 */
	void Start () {
		lg = Logger.getInstance();
		aperatus = (GameObject)Instantiate(Resources.Load("Prefabs/Aperatus"));
		cam = GameObject.Find("Main Camera");
		cam.AddComponent("CameraCtl");
		cam.AddComponent("PauseMenu");
		plane = GameObject.Find("Plane");
		p_mr = (MeshRenderer)plane.AddComponent("MeshRenderer");
		p_mr.material = (Material)Resources.Load("Materials/grid");
		_catch = GameObject.Find("Catch");
		_catch.AddComponent("Catch");
		ether = (GameObject)Instantiate(Resources.Load("Prefabs/Ether"));
		ether.AddComponent("Ether");
		nrg_ether = (GameObject)Instantiate(Resources.Load("Prefabs/Energy"));
		nrg_ether.AddComponent("EtherEnergy");
		
		co = CollisionMediator.getInstance();
		spw = Spawner.getInstance();
		gm = GeneticsMain.getInstance();
	}
	
	/*
	 * Cleanup before exiting
	 */
	void OnDestroy() {
		lg.close();
	}
	
}
