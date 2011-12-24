using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
#pragma warning disable 0414
	private Logger lg;
	private Ether eth;
	private GameObject crt;
	public static GameObject container;
	public static Spawner instance;
	private Vector3 pos;
#pragma warning restore 0414
	
	void Start () {
		lg = Logger.getInstance();
		eth = GameObject.Find("Ether").GetComponent<Ether>();
		crt = (GameObject)Resources.Load("Prefabs/Creature/PrototypeCreature");
	}
	
	public static Spawner getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}
	
	public void spawn (Vector3 pos, Vector3 rot) {
		GameObject clone;
		clone = (GameObject)Instantiate(crt, pos, Quaternion.identity);
		clone.transform.Rotate(rot);
		clone.AddComponent("Creature");
	}

}
