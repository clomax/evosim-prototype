using UnityEngine;
using System.Collections;

public class MouthRadius : MonoBehaviour {
	
	private static int MAX_SIZE = 50;	// It's ridiculously unlikely that a creature will need to consider more than this many foodbits
	private GameObject[] fbits;
	
	void Start () {
		this.fbits = new GameObject[MAX_SIZE];
	}
	
	void OnTriggerEnter (Collider c) {
		for(int i=0; i<this.fbits.Length; i++) {
			if (null == this.fbits[i]) {
				this.fbits[i] = c.gameObject;
				break;
			}
		}
	}
	
	void OnTriggerExit (Collider c) {
		for(int i=0; i<this.fbits.Length; i++) {
			if (c.gameObject == this.fbits[i]) {
				this.fbits[i] = null;
				break;
			}
		}
	}
	
	public GameObject[] getFoodbits() {
		return this.fbits;
	}
}
