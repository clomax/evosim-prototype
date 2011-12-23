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
	private LineRenderer lr;
	private Vector3 line_start;
	private Vector3 line_end;
	private int line_length = 5;
	private Vector3 pos_crt;
	
	void Start () {
		crt = transform.parent.GetComponent<Creature>();
		lr = (LineRenderer)this.gameObject.AddComponent("LineRenderer");
		lr.useWorldSpace = false;
		lr.material = (Material)Resources.Load("Materials/line");
		lr.SetWidth(0.1F,0.1F);
		lr.SetVertexCount(2);
		lr.renderer.enabled = true;
	}
	
	public Mouth () {
		
	}
	
	void Update () {
		if(closestFoodbit() != null)
			Debug.Log(closestFoodbit().transform.position.x + ", " + closestFoodbit().transform.position.z);
	}
	
	private GameObject closestFoodbit () {
		GameObject[] fbits = GameObject.FindGameObjectsWithTag("Foodbit");
		GameObject closest = null;
		float dist = line_length;
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

