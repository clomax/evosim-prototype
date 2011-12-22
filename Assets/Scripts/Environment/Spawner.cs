using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	private Logger lg;
	private Ether eth;
	public static GameObject container;
	public static Spawner instance;
	public bool spawnNow = false;
	
	void Start () {
		lg = Logger.getInstance();
		eth = GameObject.Find("Ether").GetComponent<Ether>();
	}
	
	public static Spawner getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}
	
	void Update () {
		if(spawnNow) {
			
		}
	}
	
	public void spawn (Vector3 pos) {
		//drop new creature at given position
	}

}
