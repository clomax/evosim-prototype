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
	private int crt_detect_range = 3000;
	private int crt_mate_range = 100;
	private float curr_dist = 1;
	private int id;
#pragma warning restore 0414

	void Start () {
		this._t = transform;
		this.id = GetInstanceID();
		this.gameObject.tag = "Genital";
		this.crt = (Creature)_t.parent.gameObject.GetComponent("Creature");
		lg = Logger.getInstance();
		spw = Spawner.getInstance();
		co = CollisionMediator.getInstance();
		
		_t = transform;
		lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/genital_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
	}
	
	void Update () {
		GameObject cc = closestCreature();
		if(cc) {
			lr.useWorldSpace = true;
			line_end = new Vector3(cc.transform.position.x, cc.transform.position.y, cc.transform.position.z);
			line_start = _t.position;
			lr.SetPosition(1,line_end);
			resetStart();
		} else {
			lr.useWorldSpace = false;
			line_start = new Vector3(0,0,0);
			line_end = new Vector3(0,0,line_length);
			lr.SetPosition(0,line_start);
			lr.SetPosition(1,line_end);
		}
	}
	
	private GameObject closestCreature () {
		GameObject[] crts = GameObject.FindGameObjectsWithTag("Genital");
		GameObject closest = null;
		float dist = crt_detect_range;
		Vector3 pos = transform.position;
		foreach(GameObject c in crts) {
			if (c != this.gameObject) {
				Vector3 diff = c.transform.position - pos;
				curr_dist = diff.sqrMagnitude;
				if (curr_dist < dist) {
					closest = c;
					dist = curr_dist;
				}
				if (curr_dist < crt_mate_range) {
					if (this.crt.state == Creature.State.persuing_mate) {
						Genitalia other_crt = c.transform.gameObject.GetComponent<Genitalia>();
						co.observe(this.gameObject, other_crt.gameObject);
					}
					dist = curr_dist;
				}
			}
		}
		return closest;	
	}
	
	private void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}
	
	public int getID() {
		return this.id;
	}

}
