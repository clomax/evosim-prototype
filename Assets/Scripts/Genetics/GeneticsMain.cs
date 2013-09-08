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
	
	Chromosome chromosome;
	
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
		
		starting_creatures	= (int) 		settings.contents["ether"]	 ["starting_creatures"];
		double energy		= (double)		settings.contents["creature"]["init_energy"];
		int branch_limit 	= (int)			settings.contents["creature"]["branch_limit"];
		int recursion_limit = (int)			settings.contents["creature"]["recursion_limit"];

		
		/*
		 * For each new creature, generate random genes and spawn the bugger
		 */
		for (int i=0; i<starting_creatures; i++) {
			chromosome = new Chromosome();
			
			// random colours
			Color col = new Color( (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F),
								   (float)Random.Range(0.0F,1.0F)
								 );
			chromosome.setColour(col.r, col.g, col.b);
			
			// random root sizes
			chromosome.setRootScale((float) Random.Range(min_root_scale.x,max_root_scale.x),
									(float) Random.Range(min_root_scale.y,max_root_scale.y),
									(float) Random.Range(min_root_scale.z,max_root_scale.z)
								   );
			
			// random initial limbs
			int branches = Random.Range (1,branch_limit);
			for (i=0; i<branches; i++) {
				Vector3 point = Utility.RandomPointInsideCube(chromosome.getRootScale());
				Vector3 rot = Utility.RandomRotVec();
				Vector3 scale = new Vector3 (5F,2F,2F);
				int recurrances = Random.Range(0,recursion_limit);
				chromosome.addLimb(col, point, rot, scale, recurrances);
				// foreach recurrance
					// create new limb, place at business end of its parent limb
					// decrement recurrance count
			}
			
			/*
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
			*/
			
			spw.spawn(Utility.RandomFlatVec(-100,10,100), Utility.RandomRotVec(), energy, chromosome);
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
