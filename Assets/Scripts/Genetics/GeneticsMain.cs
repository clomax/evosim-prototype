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
	
	int chromosome_length;
	float[] chromosome;
	
	int starting_creatures;
	
	Vector3 max_root_scale;
	Vector3 min_root_scale;
	
	
	void Start () {
		spw = Spawner.getInstance();
		settings = Settings.getInstance();
		eth = Ether.getInstance();
		
		max_root_scale 		= new Vector3();
		max_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["x"].ToString() );
		max_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["y"].ToString() );
		max_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["max_root_scale"]["z"].ToString() );
		
		min_root_scale 		= new Vector3();
		min_root_scale.x 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["x"].ToString() );
		min_root_scale.y 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["y"].ToString() );
		min_root_scale.z 	= float.Parse( settings.contents["creature"]["root"]["min_root_scale"]["z"].ToString() );
		
		starting_creatures	= (int) 			settings.contents["ether"]	 ["starting_creatures"];
		chromosome_length	= (int) 			settings.contents["genetics"]["chromosome_length"];
		double energy		= (double)			settings.contents["creature"]["init_energy"];
		
		for (int i=0; i<starting_creatures; i++) {
			chromosome = new float[chromosome_length];
			
			// random colours
			for (int j=0; j<3; j++) {
				chromosome[j] = (float)Random.Range(0.0F,1.0F);
			}
			
			// random root sizes
			chromosome[3] 	= (float) Random.Range(min_root_scale.x,max_root_scale.x);
			chromosome[4] 	= (float) Random.Range(min_root_scale.y,max_root_scale.y);
			chromosome[5] 	= (float) Random.Range(min_root_scale.z,max_root_scale.z);
			Vector3 root_scale = new Vector3(chromosome[3], chromosome[4], chromosome[5]);
			
			// random joint connection point
			Vector3 tmp = Utility.RandomPointInsideCube(root_scale);
			chromosome[6]	= (float) tmp.x;
			chromosome[7]	= (float) tmp.y;
			chromosome[8]	= (float) tmp.z;
			
			// random limb rotation
			tmp = Utility.RandomVector3();
			chromosome[9]	= (float) tmp.x;
			chromosome[10]	= (float) tmp.y;
			chromosome[11]	= (float) tmp.z;
			
			// random limb axis
			tmp = Utility.RandomVector3();
			chromosome[12]	= (float) tmp.x;
			chromosome[13]	= (float) tmp.y;
			chromosome[14]	= (float) tmp.z;
			
			spw.spawn(Utility.RandomFlatVec(-200,10,200), Utility.RandomRotVec(), energy, chromosome);
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
