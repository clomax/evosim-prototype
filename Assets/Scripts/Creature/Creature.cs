using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/*
 *		Author: 	Craig Lomax
 *		Date: 		06.09.2011
 *		URL:		clomax.me.uk
 *		email:		craig@clomax.me.uk
 *
 */

public class Creature : MonoBehaviour
{
	Transform _t;

	Settings settings;
	Ether eth;

	public GameObject root;
	public Root root_script;
    public Chromosome chromosome;

    Vector3 max_root_scale;
	Vector3 min_root_scale;

	public GameObject eye;
	public GameObject mouth;
	public GameObject genital;

	List<ConfigurableJoint> joints = new List<ConfigurableJoint>();

	public double age;
	public decimal energy;
    public decimal low_energy_threshold;

	public double 	line_of_sight;
	decimal 			metabolic_rate;
	int 			age_sexual_maturity;

	public int offspring;
	public int food_eaten;

    float joint_frequency;
    float joint_amplitude;
    float joint_phase;

    float force_scalar = .1F;

    public delegate void CreatureState(Creature c);
    public static event CreatureState CreatureDead;

    // TODO: Fix this shit "state machine"
    public enum State {
						persuing_food,
						persuing_mate,
						searching_for_mate,
						mating,
						eating,
						searching_for_food,
                        dead,
						neutral
					  };
	public State state;
    private bool state_lock = false;

	public Eye eye_script;
	public Vector3 target_direction;
	private Quaternion lookRotation;
	private float sine;
	private Vector3 direction;

    private bool low_energy_lock = false;
    MeshRenderer[] ms;

    private ArrayList all_segments;
    int num_segments;
    float[] segment_scales;

    void Start()
    {
        _t = transform;
        name = "creature" + gameObject.GetInstanceID();

        eth = Ether.getInstance();
        settings = Settings.getInstance();

        max_root_scale = new Vector3();
        max_root_scale.x = float.Parse(settings.contents["creature"]["root"]["max_root_scale"]["x"].ToString());
        max_root_scale.y = float.Parse(settings.contents["creature"]["root"]["max_root_scale"]["y"].ToString());
        max_root_scale.z = float.Parse(settings.contents["creature"]["root"]["max_root_scale"]["z"].ToString());

        min_root_scale = new Vector3();
        min_root_scale.x = float.Parse(settings.contents["creature"]["root"]["min_root_scale"]["x"].ToString());
        min_root_scale.y = float.Parse(settings.contents["creature"]["root"]["min_root_scale"]["y"].ToString());
        min_root_scale.z = float.Parse(settings.contents["creature"]["root"]["min_root_scale"]["z"].ToString());

        joint_frequency  = chromosome.base_joint_frequency();
        joint_amplitude  = chromosome.base_joint_amplitude();
        joint_phase      = chromosome.base_joint_phase();

        root = GameObject.CreatePrimitive(PrimitiveType.Cube);
        root.name = "root";
        root.transform.parent = _t;
        root.transform.position = _t.position;
        root.transform.eulerAngles = _t.eulerAngles;
        root.AddComponent<Rigidbody>();
        root_script = root.AddComponent<Root>();
        root_script.setColour(chromosome.root_colour());
        root_script.setScale(chromosome.root_scale());
        root.GetComponent<Rigidbody>().mass = 1.5F;
        root.GetComponent<Rigidbody>().angularDrag = float.Parse(settings.contents["creature"]["angular_drag"].ToString());
        root.GetComponent<Rigidbody>().drag = float.Parse(settings.contents["creature"]["drag"].ToString());
        eye = new GameObject();
        eye.name = "Eye";
        eye.transform.parent = root.transform;
        eye.transform.eulerAngles = root.transform.eulerAngles;
        eye.transform.position = root.transform.position;
        eye_script = eye.AddComponent<Eye>();

        mouth = new GameObject();
        mouth.name = "Mouth";
        mouth.transform.parent = root.transform;
        mouth.transform.eulerAngles = root.transform.eulerAngles;
        mouth.transform.localPosition = new Vector3(0, 0, .5F);
        mouth.AddComponent<Mouth>();

        genital = new GameObject();
        genital.name = "Genital";
        genital.transform.parent = root.transform;
        genital.transform.eulerAngles = root.transform.eulerAngles;
        genital.transform.localPosition = new Vector3(0, 0, -.5F);
        genital.AddComponent<Genitalia>();

        line_of_sight = (double)settings.contents["creature"]["line_of_sight"];
        metabolic_rate = decimal.Parse(settings.contents["creature"]["metabolic_rate"].ToString());
        age_sexual_maturity = (int)settings.contents["creature"]["age_sexual_maturity"];

        age = 0.0;
        ChangeState(State.neutral);
        food_eaten = 0;
        offspring = 0;
        low_energy_threshold = decimal.Parse(settings.contents["creature"]["low_energy_threshold"].ToString());

        InvokeRepeating("updateState", 0, 0.1f);
        InvokeRepeating("RandomDirection", 1F, 5F);

        root.GetComponent<Rigidbody>().SetDensity(1F);
        force_scalar = 10F;

        ms = GetComponentsInChildren<MeshRenderer>();

        all_segments = new ArrayList();

        setupLimbs();
    }


    // TODO: Find a better way of controlling the joints with wave functions
    //				the current way needs some sort of magic scalar
    void FixedUpdate () {
		sine = Sine(joint_frequency, joint_amplitude, joint_phase);
		for (int i=0; i<joints.Count; i++) {
			joints[i].targetRotation = Quaternion.Euler (sine * new Vector3(5F,0F,0F));
		}

		if(eye_script.goal) {
			target_direction = (eye_script.goal.transform.position - root.transform.position).normalized;
		}

		if (target_direction != Vector3.zero) {
			lookRotation = Quaternion.LookRotation(target_direction);
		}

		float abs_sine = Mathf.Abs(sine);
		float pos_sine = System.Math.Max(sine,0);
		root.transform.rotation = Quaternion.Slerp(root.transform.rotation, lookRotation, Time.deltaTime * (abs_sine * 3F));

		if (pos_sine == 0) {
			direction = root.transform.forward;
		}
        Vector3 force = Mathf.Abs(force_scalar) * direction * pos_sine * num_segments;
        root.GetComponent<Rigidbody>().AddForce(force);
	}

	float Sine (float freq, float amplitude, float phase_shift) {
		return Mathf.Sin((float)age * freq + phase_shift) * amplitude;
	}

    float metabolise_timer = 1F;
    void Update()
    {
        age += Time.deltaTime;

        metabolise_timer -= Time.deltaTime;
        if(metabolise_timer <= 0 && state != State.dead)
        {
            metabolise();
            metabolise_timer = 1F;
        }

        if (energy <= low_energy_threshold && !low_energy_lock)
        {
            low_energy_lock = true;
            StartCoroutine(SlowDown());
            StartCoroutine(Darken());
        }

        if (energy > low_energy_threshold)
        {
            state_lock = false;
            low_energy_lock = false;
            StopCoroutine(SlowDown());
            StopCoroutine(Darken());
            Lighten();
            ResetSpeed();
        }

        float _force = force_scalar;
        float _joint_frequency = joint_frequency;
        if (state == State.mating || state == State.eating)
        {
            joint_frequency = 0F;
            force_scalar = 0F;
        }
        else
        {
            joint_frequency = _joint_frequency;
            force_scalar = _force;
        }

        if (state == State.dead)
        {
            kill();
        }
    }

	private void RandomDirection () {
		target_direction = new Vector3 (
						UnityEngine.Random.Range(-1F,1F),
						UnityEngine.Random.Range(-1F,1F),
						UnityEngine.Random.Range(-1F,1F)
		);
	}

	public void setEnergy(decimal n) {
		energy = n;
	}

	void updateState() {
		if(state != Creature.State.mating) {
			if (energy < chromosome.hunger_threshold()) {
                ChangeState((eye_script.targetFbit != null) ? State.persuing_food : State.searching_for_food);
			}
			if (energy >= chromosome.hunger_threshold() && age > age_sexual_maturity) {
                ChangeState((eye_script.targetCrt != null) ? State.persuing_mate : State.searching_for_mate);
			}
		}
	}

	public void invokechromosome (Chromosome gs) {
		this.chromosome = gs;
	}

    public void ChangeState(State s)
    {
        if (!state_lock)
        {
            state = s;
        }
    }

	public decimal getEnergy () {
		return energy;
	}

	public bool subtractEnergy (decimal n)
    {
        bool equal_or_below_zero = false;
        
        energy -= n;
        if (energy <= 0.0m)
        {
            eth.addEnergy(energy + n);
            energy = 0;
            equal_or_below_zero = true;
            state = State.dead;
            state_lock = true;
        }
        else
        {
            eth.addEnergy(n);
        }

        return (equal_or_below_zero);
	}

	private void metabolise () {
        subtractEnergy(metabolic_rate);
	}

	public void kill ()
    {
        subtractEnergy(energy);
        CreatureDead(this);
        Destroy(gameObject);
    }

    private float[] GetLimbGenes()
    {
        for(int i=1; i < chromosome.limb_metadata.Length; i++)
        {
            num_segments += chromosome.limb_metadata[i];
        }
        segment_scales = new float[num_segments*3];

        int segments_start = 13;
        for (int segment_scales_index=0;  segment_scales_index < num_segments*3; segment_scales_index++)
        {
            segment_scales[segment_scales_index] = chromosome.genes[segments_start + segment_scales_index];
        }

        return (segment_scales);
    }

    private void setupLimbs ()
    {
        float[] segment_scales = GetLimbGenes();
        int segment_scales_index = 0;

        for (int current_limb = 1;
             current_limb <= chromosome.num_limbs();
             current_limb++)
        {
            GameObject[] segments = new GameObject[chromosome.limb_metadata[current_limb]];

            for (int current_segment = 0;
                current_segment < chromosome.limb_metadata[current_limb];
                current_segment++)
            {
                GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
				segment.layer = LayerMask.NameToLayer("Creature");
				segment.name = current_limb+"_"+current_segment;
				segment.transform.parent = _t;
				Segment limb_script = segment.AddComponent<Segment>();

                Vector3 scale;
                scale.x = segment_scales[segment_scales_index];
                scale.y = segment_scales[segment_scales_index+1];
                scale.z = segment_scales[segment_scales_index+2];
                segment_scales_index += 3;

                limb_script.setScale(scale);
				limb_script.setColour((Color) chromosome.limb_colour());

                Vector3 position;
				if(current_segment == 0) {
                    position = Utility.RandomPointInsideCube(chromosome.root_scale());
                    limb_script.setPosition(position);
					segment.transform.LookAt(root.transform);
				} else {
					limb_script.setPosition( segments[current_segment - 1].transform.localPosition );
					segment.transform.LookAt(root.transform);
					segment.transform.Translate(0,0,-segments[current_segment - 1].transform.localScale.z);
				}
                segments[current_segment] = segment;

				segment.AddComponent<Rigidbody>();
				segment.AddComponent<BoxCollider>();
				segment.GetComponent<Collider>().material = (PhysicMaterial)Resources.Load("Physics Materials/Creature");

				ConfigurableJoint joint = segment.AddComponent<ConfigurableJoint>();
				joint.axis = new Vector3(0.5F, 0F, 0F);
				joint.anchor = new Vector3(0F, 0F, 0.5F);
				if(current_segment == 0) {
					joint.connectedBody = root.GetComponent<Rigidbody>();
				} else {
					joint.connectedBody = segments[current_segment - 1].GetComponent<Rigidbody>();
				}
				//segment.GetComponent<Rigidbody>().drag = .5F;

				joints.Add(joint);

				joint.xMotion = ConfigurableJointMotion.Locked;
				joint.yMotion = ConfigurableJointMotion.Locked;
				joint.zMotion = ConfigurableJointMotion.Locked;

				joint.angularXMotion = ConfigurableJointMotion.Free;
				joint.angularYMotion = ConfigurableJointMotion.Free;
				joint.angularZMotion = ConfigurableJointMotion.Free;

				JointDrive angXDrive = new JointDrive();
				angXDrive.mode = JointDriveMode.Position;
				angXDrive.positionSpring = 7F;
				angXDrive.maximumForce = 100000000F;
				joint.angularXDrive = angXDrive;
				joint.angularYZDrive = angXDrive;

				segment.GetComponent<Rigidbody>().SetDensity(1F);
                all_segments.Add(segment);
			}
		}
	}

    float d_col = 0.01F;
    private IEnumerator Darken ()
    {
        float col = 1F;
        while (col > 0.15F && energy < low_energy_threshold)
        {
            foreach (var m in ms)
            {
                m.material.color -= new Color(d_col, d_col, d_col);
            }
            col -= d_col;
            yield return new WaitForSeconds(0.025F);
        }
    }

    float d_freq = 0.01F;
    float d_force = 0.01F;


    private IEnumerator SlowDown ()
    {
        float freq = joint_frequency;
        while (freq > 0.15F && energy < low_energy_threshold)
        {
            freq -= d_freq;
            joint_frequency = freq;
            if(force_scalar > 0F)
                force_scalar -= d_force;
            yield return new WaitForSeconds(0.025F);
        }
    }

    private void Lighten ()
    {
        root.GetComponent<MeshRenderer>().material.color = root_script.original_colour;
        foreach (GameObject s in all_segments)
        {
            s.GetComponent<MeshRenderer>().material.color = s.GetComponent<Segment>().original_colour;
        }
    }

    private void ResetSpeed ()
    {
        joint_frequency = chromosome.base_joint_frequency();
        force_scalar = 1F;
    }
}
