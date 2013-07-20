using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
#pragma warning disable 0414
	public static Spawner instance;
	Logger lg;
	CreatureCount crt_count;
	Ether eth;
	Settings settings;
	GameObject crt;
	static GameObject container;
	Vector3 pos;
	Vector3 max_root_scale;
#pragma warning restore 0414
	
	void Start () {
		lg = Logger.getInstance();
		//crt = (GameObject)Resources.Load("Prefabs/Creature/PrototypeCreature");
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		eth = Ether.getInstance();
		settings = Settings.getInstance();
		
		max_root_scale.x = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["x"]);
		max_root_scale.y = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["y"]);
		max_root_scale.z = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["z"]);
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
		GameObject clone = new GameObject();
		clone.transform.localPosition = pos;
		clone.transform.eulerAngles = Utility.RandomRotVec();
		Creature crt_script = (Creature) clone.AddComponent("Creature");
		
		//pretend there are genes being invoked to set the size...
		crt_script.setRootSize(new Vector3(Random.Range(1.5F,max_root_scale.x),
				 						   Random.Range(1.5F,max_root_scale.y),
										   Random.Range(1.5F,max_root_scale.z))
							  );
		
		crt_script.addEnergy(energy);
		crt_count.number_of_creatures += 1;
	}

}
