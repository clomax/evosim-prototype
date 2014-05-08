using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *		Author: 	Craig Lomax
 *		Date: 		26.07.2013
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */


public class GeneticsUtils {
	
	static System.Random rnd = new System.Random();

	static Vector3 c2_v;

	public static Chromosome mutate (Chromosome c, double rate, float factor) {
		// Mutate colour
		float[] cs = new float[3];
		Color cc = c.getColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		c.setColour(cs[0], cs[1], cs[2]);
		
		// Mutate root scale
		float[] rs = new float[3];
		Vector3 rc = c.getRootScale();
		rs[0] = rc.x;
		rs[1] = rc.y;
		rs[2] = rc.z;
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				rs[i] += randomiseGene(factor);
		}
		Vector3 rootScale = new Vector3 (rs[0], rs[1], rs[2]);
		c.setRootScale(rootScale);
		
		// mutate limbs
		cc = c.getLimbColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		c.setLimbColour(cs[0], cs[1], cs[2]);

		MultiDimList branches = c.getBranches();
		for (int b=0; b<branches.Count; b++) {
			List<GameObject> limbs = branches[b];
			for (int i=0; i<limbs.Count; i++) {
				GameObject l = limbs[i];
				try {
					Limb l_script = l.GetComponent<Limb>();
					Vector3 v = l_script.getScale();
					for (int k=0; k<3; k++) {
						double rand = rnd.NextDouble();
						if(rand < rate)
							v[k] += randomiseGene(factor);
						}
				} catch {
					break;
				}
			}
		}
		c.setBranches(branches);
		
		return c;
	}
	
	public static Chromosome crossover (Chromosome c1, Chromosome c2, double rate) {
		Chromosome c = c1;
		
		// Crossover colour
		Color col = c1.getColour();
		for (int i=0; i<3; i++) {
			double rand = rnd.NextDouble();
			if (rand < rate)
				col[i] = c2.getColour()[i];
		}
		c.setColour(col[0], col[1], col[2]);

		// Crossover limbs
		MultiDimList c1_branches = c1.getBranches();
		MultiDimList c2_branches = c2.getBranches();
		MultiDimList c_branches;

		// Randomly select the parent from which the child will derive its limb structure
		int select = Random.Range(0,2);
		MultiDimList other_crt_branches;
		if (select == 0) {
			c_branches = c1_branches;
			other_crt_branches = c2_branches;
		} else {
			c_branches = c2_branches;
			other_crt_branches = c1_branches;
		}
		
		// Randomly select attributes from the selected creature's limbs to
		//	assign to child creature's limbs
		for (int i=0; i<c_branches.Count; i++) {
			List<GameObject> c_limbs = c_branches[i];
			
			for (int j=1; j<c_limbs.Count; j++) {
				double rand = rnd.NextDouble();
				if (rand < rate) {
					Limb limb_script;
					Limb child_limb_script;
					try {
						limb_script = c_limbs[j].GetComponent<Limb>();
						child_limb_script = other_crt_branches[i][j].GetComponent<Limb>();
					} catch {
						break;
					}
					child_limb_script.setScale(limb_script.getScale());
					child_limb_script.setPosition(limb_script.getPosition());
				}
			}
		}		
		
		return c;
	}
	
	private static float randomiseGene(float factor) {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
