using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chromosome
{
    public List<float> genes;
    public int[] limb_metadata;

    public Chromosome ()
    {
        genes = new List<float>();
    }

    public Color root_colour()
    {
        return new Color(genes[0], genes[1], genes[2]);
    }

    public Color limb_colour()
    {
        return new Color(genes[3], genes[4], genes[5]);
    }

    public Vector3 root_scale()
    {
        return new Vector3(genes[6], genes[7], genes[8]);
    }

    public decimal hunger_threshold()
    {
        return (decimal)genes[9];
    }

    public float base_joint_frequency()
    {
        return genes[10];
    }

    public float base_joint_amplitude()
    {
        return genes[11];
    }

    public float base_joint_phase()
    {
        return genes[12];
    }

    public int num_limbs()
    {
        return limb_metadata[0];
    }
}
