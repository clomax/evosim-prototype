using UnityEngine;
using System.Collections;

public class GenitalRadius : MonoBehaviour {
	
	public static int MAX_SIZE = 50;
	public GameObject[] g;
	
	void Start () {
		g = new GameObject[MAX_SIZE];
	}
	
	void OnTriggerEnter (Collider c) {
		for(int i=0; i<this.g.Length; i++) {
			if (null == this.g[i]) {
				this.g[i] = c.gameObject;
				break;
			}
		}
	}
	
	void OnTriggerExit (Collider c) {
		for(int i=0; i<this.g.Length; i++) {
			if (c.gameObject == this.g[i]) {
				this.g[i] = null;
				break;
			}
		}
	}
}
