using UnityEngine;
using System.Collections;

public class Genitalia : MonoBehaviour {
	
	public LineRenderer lr;
	public Vector3 line_start;
	public float line_length = 0.5F;
	public Vector3 line_end;
	public float line_width = 0.5F;
	public Creature crt;
	public Creature cc;
	public Eye eye;
	Transform _t;

	
	void Start () {
		_t = transform;
		lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/genital_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
		crt = transform.parent.gameObject.GetComponent<Creature>();
		eye = crt.GetComponent<Eye>();
	}
	
	void Update () {		
		if(eye.cc) {
			lr.material = (Material)Resources.Load("Materials/genital_vector");
			lr.useWorldSpace = true;
			line_start = crt.genital.transform.position;
			line_end = eye.cc.genital.transform.position;
			lr.SetPosition(1,line_end);
			resetStart();
		} else {
			lr.material = (Material)Resources.Load("Materials/genital_vector");
			lr.useWorldSpace = false;
			line_start = crt.genital.transform.position;
			line_end = new Vector3(0,0,line_length);
			lr.SetPosition(0,line_start);
			lr.SetPosition(1,line_end);
		}
	}

	public void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}
}
