using UnityEngine;
using System.Collections;

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
		c.setRootScale(rs[0], rs[1], rs[2]);
		
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

		ArrayList limbs = c.getLimbs();
		for (int i=0; i<limbs.Count; i++) {
			ArrayList l = (ArrayList) limbs[i];
			for (int j=1; j<l.Count-1; j++) {
				Vector3 v = (Vector3) l[j];
				for (int k=0; k<3; k++) {
					double rand = rnd.NextDouble();
					if(rand < rate)
						v[k] += randomiseGene(factor);
				}
			}
		}
		
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
		// get limbs from first creature
		ArrayList c_limbs = c.getLimbs();

		// get limbs from second creature
		ArrayList c2_limbs = c2.getLimbs();
		
		// Collect limb attributes from second creature
		
		for (int i=0; i<c2_limbs.Count; i++) {
			ArrayList c2_l = (ArrayList) c2_limbs[i];
			
			for (int j=1; j<c2_l.Count-1; j++) {
				c2_v = (Vector3) c2_l[j];
			}
		}
		
		// Randomly select attributes from second creature's limbs to
		//	assign to creature's limbs
		for (int i=0; i<c_limbs.Count; i++) {
			ArrayList c_l = (ArrayList) c_limbs[i];
			
			for (int j=1; j<c_l.Count-1; j++) {
				Vector3 c_v = (Vector3) c_l[j];
				
				for (int k=0; k<3; k++) {
					double rand = rnd.NextDouble();
					int index = Random.Range(0,3);
					
					if(rand < rate)
						c_v[k] = c2_v[index];
				}
			}
		}		
		
		return c;
	}
	
	private static float randomiseGene(float factor) {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
