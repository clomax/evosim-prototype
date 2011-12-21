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
	private GameObject cam;
	private GameObject plane;
	private GameObject ether;
	private GameObject _catch;
	private GameObject nrg_ether;
	
	private MeshRenderer p_mr;
	
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
		nrg_ether = (GameObject)Instantiate(Resources.Load("Prefabs/Energy"));
		nrg_ether.AddComponent("EtherEnergy");
	}

	void OnDestroy() {
		lg.close();
	}
	
}
