using UnityEngine;
using System.Collections;

public class Eye : MonoBehaviour {

	static int MAX_SIZE = 50;
	GameObject[] objects;
	public Creature crt;
	public CollisionMediator co;
	public LineRenderer lr;
	public Vector3 line_start;
	public float line_length = 0.5F;
	public Vector3 line_end;
	public float line_width = 0.5F;
	public int crt_detect_range = 40;
	public int crt_mate_range = 10;
	public float curr_dist = 1;
	public double timeCreated;
	public double timeToEnableMating = 3.0f;
	public Transform _t;
	public Creature mate;
	public GameObject closestFoodbit;

	
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
		crt = _t.parent.gameObject.GetComponent<Creature>();
	}
	
	void Update () {
		if (this.crt.state == Creature.State.mating && Time.time > (timeCreated + timeToEnableMating)) {
			this.crt.state = Creature.State.persuing_mate;
			timeCreated = Time.time;
		}
		
		Creature cc = closestCrt();
		if(cc) {
			lr.material = (Material)Resources.Load("Materials/genital_vector");
			lr.useWorldSpace = true;
			line_end = new Vector3(cc.genital.transform.position.x,
			                       cc.genital.transform.position.y,
			                       cc.genital.transform.position.z
			                      );
			line_start = crt.genital.transform.position;
			lr.SetPosition(1,line_end);
			resetStart();
		} else {
			lr.useWorldSpace = false;
			line_start = crt.genital.transform.position;
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
	
	public Creature closestCrt () {
		Creature closest = null;
		float dist = crt_detect_range;
		Vector3 pos = transform.position;
		foreach(GameObject c in objects) {
			if (c && c.tag == "Creature" && c != _t.parent.gameObject) {
				Vector3 diff = c.transform.position - pos;
				curr_dist = diff.magnitude;
				if (curr_dist < dist) {
					closest = c.GetComponent<Creature>();
					dist = curr_dist;
				}
				if (curr_dist < crt_mate_range) {
					Creature other_crt = c.gameObject.GetComponent<Creature>();
					if (this.crt.state == Creature.State.persuing_mate || other_crt.state == Creature.State.persuing_mate) {
						co.observe(this.gameObject, other_crt.genital);
						other_crt.state = Creature.State.mating;
						this.crt.state = Creature.State.mating;
					}
					dist = curr_dist;
				}
			}
		}
		return closest;	
	}
	
	public void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}
}
