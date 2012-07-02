using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
#pragma warning disable 0414
	private Logger lg;
	private Ether eth;
	private CreatureCount crt_count;
	private GameObject crt;
	public static GameObject container;
	public static Spawner instance;
	private Vector3 pos;
#pragma warning restore 0414
	
	void Start () {
		lg = Logger.getInstance();
		eth = GameObject.Find("Ether").GetComponent<Ether>();
		crt = (GameObject)Resources.Load("Prefabs/Creature/PrototypeCreature");
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
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
		clone.transform.eulerAngles = Utility.RandomRotVec();
		clone.AddComponent("Creature");
		// 
		// take away energy from each creature
		crt_count.number_of_creatures += 1;
	}

}
