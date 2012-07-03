using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

	static int MAX_SIZE = 50;
	GameObject[] objects;
	private Creature crt;
	private CollisionMediator co;
	private LineRenderer lr;
	private Vector3 line_start;
	private float line_length = 0.5F;
	private Vector3 line_end;
	private float line_width = 0.5F;
	private int crt_detect_range = 40;
	private int crt_mate_range = 10;
	private float curr_dist = 1;
	public double timeCreated;
	public double timeToEnableMating = 3.0f;
	private Transform _t;

	
	void Start () {
		this.objects = new GameObject[MAX_SIZE];
		co = CollisionMediator.getInstance();
		_t = transform;
		lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/genital_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
		timeCreated = Time.time;
	}
	
	void Update () {		
		Creature cc = closestCreature();
		if(cc) {
			lr.useWorldSpace = true;
			line_end = new Vector3(cc.genital.transform.position.x, cc.genital.transform.position.y, cc.genital.transform.position.z);
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
	
	void OnTriggerEnter (Collider c) {
		if(c.tag == "Foodbit" || c.tag == "Creature")
		for(int i=0; i<this.objects.Length; i++) {
			if (null == this.objects[i]) {
				this.objects[i] = c.gameObject;
				break;
			}
		}
	}
	
	void OnTriggerExit (Collider c) {
		for(int i=0; i<this.objects.Length; i++) {
			if (c.gameObject == this.objects[i]) {
				this.objects[i] = null;
				break;
			}
		}
	}
	
	private Creature closestCreature () {
		Creature closest = null;
		float dist = crt_detect_range;
		Vector3 pos = transform.position;
		foreach(GameObject c in objects) {
			if (c && c.tag == "Creature" && c != transform.parent.gameObject) {
				Vector3 diff = c.transform.position - pos;
				curr_dist = diff.sqrMagnitude;
				if (curr_dist < dist) {
					closest = c.GetComponent<Creature>();
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
}
