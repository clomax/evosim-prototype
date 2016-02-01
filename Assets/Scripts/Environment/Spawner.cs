using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{	
#pragma warning disable 0414
	public static Spawner instance;
	Logger lg;
	CreatureCount crt_count;
	Ether eth;
	GameObject crt;
	static GameObject container;
	Vector3 pos;
#pragma warning restore 0414
	
	void Start ()
    {
		lg = Logger.getInstance();
		crt_count = GameObject.Find("CreatureCount").GetComponent<CreatureCount>();
		eth = Ether.getInstance();
    }
	
	public static Spawner getInstance ()
    {
		if(!instance)
        {
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}
	
	public void spawn (Vector3 pos, Vector3 rot, decimal energy, Chromosome chromosome)
    {
		GameObject clone = new GameObject();
		clone.transform.localPosition = pos;
		clone.transform.eulerAngles = Utility.RandomRotVec();
		Creature crt_script = clone.AddComponent<Creature>();
		clone.tag = "Creature";
	
		crt_script.invokechromosome(chromosome);
		
		crt_script.setEnergy(energy);
		crt_count.number_of_creatures += 1;
	}

}
