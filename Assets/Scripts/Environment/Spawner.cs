using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
#pragma warning disable 0414
	public static Spawner instance;
	Logger lg;
	CreatureCount crt_count;
	Ether eth;
	GameObject crt;
	static GameObject container;
	Vector3 pos;
#pragma warning restore 0414
	
	void Start () {
		lg = Logger.getInstance();
		crt = (GameObject)Resources.Load("Prefabs/Creature/PrototypeCreature");
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		eth = Ether.getInstance();
	}
	
	public static Spawner getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}
	
	public void spawn (Vector3 pos, Vector3 rot, double energy) {
		GameObject clone;
		clone = (GameObject)Instantiate(crt, pos, Quaternion.identity);
		clone.transform.eulerAngles = Utility.RandomRotVec();
		Creature crt_script = (Creature) clone.AddComponent("Creature");
		crt_script.addEnergy(energy);
		crt_count.number_of_creatures += 1;
		
		eth.subtractEnergy(energy);
	}

}
