using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class Mouth : MonoBehaviour {

	public Foodbit fb;
	public Creature crt;
	private Transform _t;
	private LineRenderer lr;
	private Vector3 line_start;
	private float line_length = 0.5f;
	private Vector3 line_end;
	private float line_width = 0.5F;
	private int fb_detect_range = 40;
	private Object[] fbits;
	private GameObject cf;
	public Eye eye;

	void Start () {
		this._t = transform;
		this.crt = (Creature)_t.parent.gameObject.GetComponent("Creature");
		this.lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		this.lr.material = (Material)Resources.Load("Materials/mouth_vector");
		this.lr.SetWidth(line_width, line_width);
		this.lr.SetVertexCount(2);
		this.lr.renderer.enabled = true;
		eye = crt.eye.gameObject.GetComponent<Eye>();
	}

	void Update () {
		this.cf = eye.closestFbit;
		if(cf) {
			this.lr.useWorldSpace = true;
			this.line_end = new Vector3(cf.transform.position.x, cf.transform.position.y, cf.transform.position.z);
			this.line_start = _t.position;
			this.lr.SetPosition(1,line_end);
			this.resetStart();
		} else {
			this.lr.useWorldSpace = false;
			this.line_start = Vector3.zero;
			this.line_end = new Vector3(0,0,line_length);
			this.lr.SetPosition(0,line_start);
			this.lr.SetPosition(1,line_end);
		}
	}
	
	private void resetStart () {
		this.line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		this.lr.SetPosition(0,line_start);
	}
	
	public float getDetectRadius() {
		return this.fb_detect_range;
	}

}
