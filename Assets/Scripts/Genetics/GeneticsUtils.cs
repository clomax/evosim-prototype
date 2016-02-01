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


public class GeneticsUtils
{

	static double rand;
	static System.Random rnd = new System.Random();
	
	public static Chromosome mutate (Chromosome c, double rate, float factor)
    {
		// Mutate colour
		float[] cs = new float[3];
		Color cc = c.getColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++)
        {
			rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		c.setColour(cs[0], cs[1], cs[2]);
		
		// Mutate root scale
		Vector3 rc = c.getRootScale();

		if (rc.x > 1F && rc.y > 1F && rc.z > 1F)
        {
			float[] rs = new float[3];
			rs[0] = rc.x;
			rs[1] = rc.y;
			rs[2] = rc.z;
			for (int i=0; i<3; i++)
            {
				rand = rnd.NextDouble();
				if (rand < rate)
					rs[i] += randomiseGene(factor);
			}
			Vector3 rootScale = new Vector3 (rs[0], rs[1], rs[2]);
			c.setRootScale(rootScale);
		}
		
		// mutate limbs
		cc = c.getLimbColour();
		cs[0] = cc.r;
		cs[1] = cc.g;
		cs[2] = cc.b;
		for (int i=0; i<3; i++)
        {
			rand = rnd.NextDouble();
			if (rand < rate)
				cs[i] += randomiseGene(factor);
		}
		c.setLimbColour(cs[0], cs[1], cs[2]);

		ArrayList branches = c.branches;
		for (int b=0; b<branches.Count; b++)
        {
			ArrayList limbs = (ArrayList) branches[b];
			for (int i=0; i<limbs.Count; i++)
            {
				ArrayList limb = (ArrayList) limbs[i];
				Vector3 v = (Vector3) limb[1];
				for (int k=0; k<3; k++)
                {
					rand = rnd.NextDouble();
					if(rand < rate)
						v[k] += randomiseGene(factor);
					}

			}
		}

		// mutate base frequency and amplitude
		rand = rnd.NextDouble();
		if(rand < rate)
        {
			c.base_joint_amplitude += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			c.base_joint_frequency += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			c.base_joint_phase += randomiseGene(factor);
		}

		rand = rnd.NextDouble();
		if(rand < rate)
        {
			c.hunger_threshold += randomiseGene(factor);
		}

		c.setBranches(branches);
		return c;
	}
	
	public static Chromosome crossover (Chromosome c1, Chromosome c2, double rate)
    {
		Chromosome c = new Chromosome();
		
		// Crossover colour
		Color c1_col = c1.getColour();
		Color c2_col = c2.getColour();
		float r = (.5F * c1_col.r) + (.5F * c2_col.r);
		float g = (.5F * c1_col.g) + (.5F * c2_col.g);
		float b = (.5F * c1_col.b) + (.5F * c2_col.b);
		c.setColour(r,g,b);

		Color c1_limb_col = c1.getLimbColour();
		Color c2_limb_col = c2.getLimbColour();
		r = (.5F * c1_limb_col.r) + (.5F * c2_limb_col.r);
		g = (.5F * c1_limb_col.g) + (.5F * c2_limb_col.g);
		b = (.5F * c1_limb_col.b) + (.5F * c2_limb_col.b);
		c.setLimbColour(r,g,b);

		// Crossover limbs
		ArrayList c1_branches = c1.branches;
		ArrayList c2_branches = c2.branches;
		ArrayList c_branches;

		// Randomly select the parent from which the child will derive its limb structure
		int select = Random.Range(0,2);
		ArrayList other_crt_branches;
		if (select == 0)
        {
			c_branches = c1_branches;
			other_crt_branches = c2_branches;
		}
        else
        {
			c_branches = c2_branches;
			other_crt_branches = c1_branches;
		}

		select = Random.Range(0,2);
		if (select == 0)
        {
			c.setRootScale(c1.getRootScale());
		}
        else
        {
			c.setRootScale(c2.getRootScale());
		}

        // Randomly select attributes from the selected creature's limbs to
        //	assign to child creature's limbs

        c.num_recurrences = new int[c_branches.Count];
		for (int i=0; i<c_branches.Count; i++)
        {
			ArrayList c_limbs = (ArrayList) c_branches[i];
            c.num_recurrences[i] = c_limbs.Count;

            int index;
            for (int j=1; j<c_limbs.Count; j++)
            {
				ArrayList c_attributes = (ArrayList) c_limbs[j];

                //select random limb segment from other creature
                index = Random.Range(0, other_crt_branches.Count);
                ArrayList other_crt_limbs = (ArrayList) other_crt_branches[index];

                index = Random.Range(0, other_crt_limbs.Count);
                ArrayList other_crt_attributes = (ArrayList)other_crt_limbs[index];

				Vector3 c_scale = (Vector3) c_attributes[1];
				Vector3 other_crt_scale = (Vector3) other_crt_attributes[1];
				for (int s=0; s<3; s++)
                {
					rand = rnd.NextDouble();
					if (rand < rate) {
						c_scale[s] = other_crt_scale[s];
					}
				}

				//select random limb segment from other creature
				other_crt_limbs = (ArrayList) other_crt_branches[Random.Range (0,other_crt_branches.Count)];
				other_crt_attributes = (ArrayList) other_crt_limbs[Random.Range(0,other_crt_limbs.Count)];

				Vector3 c_pos = (Vector3) c_attributes[0];
				Vector3 other_crt_pos = (Vector3) other_crt_attributes[0];
				for (int p=0; p<3; p++)
                {
					rand = rnd.NextDouble();
					if (rand < rate)
                    {
						c_pos[p] = other_crt_pos[p];
					}
				}
			}
			c_branches[i] = c_limbs;
            c.num_branches = c_branches.Count;
		}

		// Crossover frequency and amplitude
		c.base_joint_amplitude = c1.base_joint_amplitude;
		c.base_joint_frequency = c1.base_joint_frequency;
		c.base_joint_phase	   = c1.base_joint_phase;

		rand = rnd.NextDouble();
		if (rand < 0.5f)
        {
			c.base_joint_amplitude = c2.base_joint_amplitude;
		}

		rand = rnd.NextDouble();
		if (rand < 0.5f)
        {
			c.base_joint_frequency = c2.base_joint_frequency;
		}

		rand = rnd.NextDouble();
		if (rand < 0.5f)
        { 
			c.base_joint_phase = c2.base_joint_phase;
		}

		rand = rnd.NextDouble();
		if (rand < 0.5f)
        {
			c.hunger_threshold = c2.hunger_threshold;
		}
		else
        {
			c.hunger_threshold = c1.hunger_threshold;
		}

		c.setBranches(c_branches);

        return (c);
	}

	public static float similar_colour (Chromosome c1, Chromosome c2)
    {
		Color colour1 = c1.getColour();
		Color colour2 = c2.getColour();
		
		return Mathf.Abs((colour1.r * colour2.r) - (colour1.g * colour2.g) - (colour1.b * colour2.g));
	}
	
	private static float randomiseGene(float factor)
    {
		return (float) rnd.NextDouble() * ( Mathf.Abs(factor-(-factor)) ) + (-factor);
	}
	
}
