using UnityEngine;
using System.Collections;

public class CollEvent {
	private GameObject a;
	private GameObject b;
	private GameObject[] evt;
	
	public CollEvent(GameObject a, GameObject b) {
		this.a = a;
		this.b = b;
		this.evt = new GameObject[2];
		this.evt[0] = this.a;
		this.evt[1] = this.b;
	}
	
	public GameObject[] getEvent() {
		return this.evt;
	}
}
