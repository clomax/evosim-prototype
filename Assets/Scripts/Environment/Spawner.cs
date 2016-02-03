using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{	
#pragma warning disable 0414
	public static Spawner instance;
	Logger lg;
    Data d;
	CreatureCount crt_count;
	Ether eth;
	GameObject crt;
	static GameObject container;
	Vector3 pos;
#pragma warning restore 0414

    public delegate void Crt(Creature c);
    public static event Crt CreatureSpawned;
	
	void Start ()
    {
		lg = Logger.getInstance();
        d = Data.getInstance();
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
		GameObject child = new GameObject();
		child.transform.localPosition = pos;
		child.transform.eulerAngles = Utility.RandomRotVec();
		Creature crt_script = child.AddComponent<Creature>();
		child.tag = "Creature";
		crt_script.invokechromosome(chromosome);
		crt_script.setEnergy(energy);
        CreatureSpawned(crt_script);
	}
}
