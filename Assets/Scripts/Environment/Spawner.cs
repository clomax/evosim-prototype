using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
	
	private Logger lg;
	private Ether eth;
	public GameObject crt;
	public static GameObject container;
	public static Spawner instance;
	
	void Start () {
		lg = Logger.getInstance();
		eth = GameObject.Find("Ether").GetComponent<Ether>();
		crt = (GameObject)Instantiate(Resources.Load("Prefabs/PrototypeCreature"));
	}
	
	public static Spawner getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "Spawner";
			instance = container.AddComponent(typeof(Spawner)) as Spawner;
		}
		return instance;
	}

}
