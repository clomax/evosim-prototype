using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *		Author: 	Craig Lomax
 *		Date: 		07.09.2013
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Chromosome {
	
	Color colour;
	Color limb_colour;
	Vector3 root_scale;
	
	int branches;
	ArrayList limbs = new ArrayList();	
	ArrayList limb;

	public Color getColour () {
		return colour;	
	}

	public Color getLimbColour () {
		return limb_colour;
	}
	
	public Vector3 getRootScale () {
		return root_scale;	
	}
	
	public int getBranches () {
		return branches;
	}
	
	public void setColour (float r, float g, float b) {
		colour = new Color(r,g,b);
	}

	public void setLimbColour (float r, float g, float b) {
		limb_colour = new Color(r,g,b);
	}
	
	public void setRootScale (float x, float y, float z) {
		root_scale = new Vector3(x,y,z);
	}
	
	public void setBranches (int b) {
		branches = b;	
	}
	
	public void addLimb (Color col, Vector3 point, Vector3 scale, int recur) {
		limb_colour = col;
		limb = new ArrayList();
		limb.Add(col);
		limb.Add(point);
		limb.Add(scale);
		limb.Add(recur);
		limbs.Add(limb);
	}
	
	public void delLimb () {
		
	}
	
	public ArrayList getLimbs () {
		return limbs;
	}

}
