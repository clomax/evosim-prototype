using UnityEngine;
using System.Collections;

/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Genitalia : MonoBehaviour {

#pragma warning disable 0414
	private Creature crt;
	private Logger lg;
	private Spawner spw;
	private CollisionMediator co;
	private Transform _t;
	private LineRenderer lr;
	private Vector3 line_start;
	private float line_length = 0.5F;
	private Vector3 line_end;
	private float line_width = 0.5F;
	private double timeCreated;
	public double timeToEnableMating = 1.0f;
	public Eye eye;
	public int id;
#pragma warning restore 0414

	void Start () {
		_t = transform;
		id = GetInstanceID();
		gameObject.tag = "Genital";
		crt = (Creature)_t.parent.gameObject.GetComponent("Creature");
		lg = Logger.getInstance();
		co = CollisionMediator.getInstance();
		eye = crt.eye.gameObject.GetComponent<Eye>();
		
		_t = transform;
		lr = (LineRenderer)gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/genital_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
		timeCreated = Time.time;
	}
	
	void Update () {
		if (crt.state == Creature.State.mating && Time.time > (timeCreated + timeToEnableMating)) {
			crt.state = Creature.State.persuing_mate;
			timeCreated = Time.time;
		}
		
		Creature cc = eye.closestCrt;
		if(cc) {
			lr.useWorldSpace = true;
			line_end = new Vector3(cc.genital.transform.position.x,
			                       cc.genital.transform.position.y,
			                       cc.genital.transform.position.z
			                      );
			line_start = _t.position;
			lr.SetPosition(1,line_end);
			resetStart();
		} else {
			lr.useWorldSpace = false;
			line_start = new Vector3(0,0,-.5f);
			line_end = new Vector3(0,0,line_length);
			lr.SetPosition(0,line_start);
			lr.SetPosition(1,line_end);
		}
	}
	
	
	
	private void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}

}
