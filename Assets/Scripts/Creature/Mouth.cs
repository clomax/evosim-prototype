using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		crl9@aber.ac.uk
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
	private int fb_detect_range = 2000;
	//private int fb_eat_range = ;
	
	void Start () {
		_t = transform;
		lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		lr.material = (Material)Resources.Load("Materials/mouth_vector");
		lr.SetWidth(line_width, line_width);
		lr.SetVertexCount(2);
		lr.castShadows = false;
		lr.receiveShadows = false;
		lr.renderer.enabled = true;
	}
	
	void Update () {
		GameObject cf = closestFoodbit();
		if(cf) {
			lr.useWorldSpace = true;
			line_end = new Vector3(cf.transform.position.x, Foodbit.foodbitHeight/2, cf.transform.position.z);
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
	
	private GameObject closestFoodbit () {
		GameObject[] fbits = GameObject.FindGameObjectsWithTag("Foodbit");
		GameObject closest = null;
		float dist = fb_detect_range;
		Vector3 pos = transform.position;
		foreach(GameObject fb in fbits) {
			Vector3 diff = fb.transform.position - pos;
			float curr_dist = diff.sqrMagnitude;
			if (curr_dist < dist) {
				closest = fb;
				dist = curr_dist;
			}
		}
		return closest;	
	}
	
	private void resetStart () {
		line_start = new Vector3(_t.position.x,_t.position.y,_t.position.z);
		lr.SetPosition(0,line_start);
	}
	
	/*
	 * If a foobit enters the mouth; Omnomnom.
	void OnTriggerEnter (Collider col) {
		if (col.gameObject.tag == "Foodbit") {
			fb = col.GetComponent<Foodbit>();
			crt.eat(fb.getEnergy());
			fb.destroy();
		}
	}
	*/

}
