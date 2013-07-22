using UnityEngine;
using System.Collections;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsMain : MonoBehaviour {

	private Ether eth;
	private Settings settings;
	private Spawner spw;
	public static GameObject container;
	public static GeneticsMain instance;
	
	int chromosome_length = 6;
	byte[] chromosome;
	
	int starting_creatures;
	
	Vector3 max_root_scale;
	
	void Start () {
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		eth = Ether.getInstance();
		
		max_root_scale = new Vector3();
		max_root_scale.x = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["x"]);
		max_root_scale.y = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["y"]);
		max_root_scale.z = (float) ((double) settings.contents["creature"]["root"]["max_root_scale"]["z"]);

		starting_creatures = (int) settings.contents["ether"]["starting_creatures"];
		
		double energy = (double)settings.contents["creature"]["init_energy"];
		
		for (int i=0; i<starting_creatures; i++) {
			chromosome = new byte[chromosome_length];
			
			// random colours
			for (int j=0; j<3; j++) {
				chromosome[j] = (byte)Random.Range(0,255);
			}
			
			// random root sizes
			chromosome[3] = 	(byte)Random.Range(0,255);
			chromosome[4] = 	(byte)Random.Range(0,255);
			chromosome[5] = 	(byte)Random.Range(0,255);
			
			spw.spawn(Utility.RandomFlatVec(-200,5,200), Utility.RandomRotVec(), energy, chromosome);
			eth.subtractEnergy(energy);
		}
	}
	
	public static GeneticsMain getInstance () {
		if(!instance) {
			container = new GameObject();
			container.name = "GeneticsMain";
			instance = container.AddComponent<GeneticsMain>();
		}
		return instance;
	}
}
