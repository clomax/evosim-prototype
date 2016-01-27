using UnityEngine;
using System.Collections;
using System.IO;

public class SaveCreature : MonoBehaviour
{
    public CreaturePane cp;

    string json_creature;

    public void save ()
    {
        Chromosome chromosome = cp.crt.chromosome;
        int crt_id = Mathf.Abs(cp.crt.gameObject.GetInstanceID());
        if (!Directory.Exists(Application.dataPath + "/data/saved_creatures" + crt_id))
            Directory.CreateDirectory(Application.dataPath + "/data/saved_creatures/" + crt_id);

        string filename = Application.dataPath + "/data/saved_creatures/" + crt_id + "/" + crt_id + ".json";
        string json_creature_pattern = 
@"{{
    ""crt_id\"" : {{
        ""colour"" : {{
            ""r"" : {0},
            ""g"" : {1},
            ""b"" : {2},
        }},
                    
        ""limb_colour"" : {{
            ""r"" : {3},
            ""g"" : {4},
            ""b"" : {5},
        }},

        ""root_scale"" : {{
            ""r"" : {6},
            ""g"" : {7},
            ""b"" : {8},
        }},

        ""base_joint_frequency"" : {9},
        ""base_joint_amplitude"" : {10},
        ""base_joint_phase""     : {11},
        ""hunder_threshold""     : {12},
        ";

        string[] args = {
            chromosome.colour.r.ToString(), chromosome.colour.g.ToString(), chromosome.colour.b.ToString(),
            chromosome.limb_colour.a.ToString(), chromosome.limb_colour.g.ToString(), chromosome.limb_colour.b.ToString(),
            chromosome.root_scale.x.ToString(), chromosome.root_scale.y.ToString(), chromosome.root_scale.z.ToString(),
            chromosome.base_joint_frequency.ToString(), chromosome.base_joint_amplitude.ToString(), chromosome.base_joint_phase.ToString(),
            chromosome.hunger_threshold.ToString()
        };

        json_creature = string.Format(json_creature_pattern, args);

        json_creature +=
        @"""limbs"" : {{
        ";

        int branch_count = chromosome.getBranchCount();
        for(int i=0; i<branch_count; ++i)
        {
            ArrayList limbs = chromosome.getLimbs(i);
            for(int k=0; k<limbs.Count; ++k)
            {
                ArrayList attributes = (ArrayList)limbs[k];
                Vector3 position =  (Vector3)attributes[0];
                Vector3 scale =     (Vector3)attributes[1];

                string limb_string =
        @"""{0}_{1}"" : {{
            ""position"" : {{
                {2},
                {3},
                {4}
            }},
            ""scale"" : {{
                {5},
                {6},
                {7}
            }}
        }}";
                string[] l_args = {
                        i.ToString(), k.ToString(),
                        position.x.ToString(), position.y.ToString(), position.z.ToString(),
                        scale.x.ToString(), scale.y.ToString(), scale.z.ToString()
                };
                json_creature += string.Format(limb_string, l_args);
            }
        }

        json_creature +=
        @"}}
        ";

        json_creature += 
@"}}";

        using (var sw = new StreamWriter(filename))
        {
            sw.Write(json_creature);
        }
    }	
}
