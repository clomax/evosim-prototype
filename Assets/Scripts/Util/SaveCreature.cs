using UnityEngine;
using System.Collections;
using System.IO;

public class SaveCreature : MonoBehaviour
{
    public delegate void Save();
    public static event Save CreatureSaved;

    public CreaturePane cp;

    string json_creature;

    CreatureInfoContainer creature_info;

    void Start ()
    {
        creature_info = CreatureInfoContainer.getInstance();
    }

    public void save ()
    {
        Chromosome chromosome = cp.crt.chromosome;
        int crt_id = Mathf.Abs(cp.crt.gameObject.GetInstanceID());
        if (!Directory.Exists(Application.dataPath + "/data/saved_creatures" + crt_id))
            Directory.CreateDirectory(Application.dataPath + "/data/saved_creatures/" + crt_id);

        string filename = Application.dataPath + "/data/saved_creatures/" + crt_id + "/" + crt_id + ".json";
        string json_creature_pattern = 
@"{{
    ""name"" : ""{0}"",
    ""attributes"" : {{
        ""colour"" : {{
            ""r"" : {1},
            ""g"" : {2},
            ""b"" : {3}
        }},
                    
        ""limb_colour"" : {{
            ""r"" : {4},
            ""g"" : {5},
            ""b"" : {6}
        }},

        ""root_scale"" : {{
            ""x"" : {7},
            ""y"" : {8},
            ""z"" : {9}
        }},

        ""base_joint_frequency"" : {10},
        ""base_joint_amplitude"" : {11},
        ""base_joint_phase""     : {12},
        ""hunger_threshold""     : {13},
        ""branches""             : {14},
        ";

        string[] args = {
            cp.Name.text,
            chromosome.colour.r.ToString(), chromosome.colour.g.ToString(), chromosome.colour.b.ToString(),
            chromosome.limb_colour.r.ToString(), chromosome.limb_colour.g.ToString(), chromosome.limb_colour.b.ToString(),
            chromosome.root_scale.x.ToString(), chromosome.root_scale.y.ToString(), chromosome.root_scale.z.ToString(),
            chromosome.base_joint_frequency.ToString(), chromosome.base_joint_amplitude.ToString(), chromosome.base_joint_phase.ToString(),
            chromosome.hunger_threshold.ToString(), chromosome.num_branches.ToString()
        };
        json_creature = string.Format(json_creature_pattern, args);

        json_creature +=
        @"""recurrences"" : [
        ";
        for(int i=0; i<chromosome.num_recurrences.Length; ++i)
        {
            string r_pattern =
            @"{0}";

            if (!(i == chromosome.num_recurrences.Length-1))
            {
                r_pattern +=
            @",
            ";
            }

            json_creature += string.Format(r_pattern, chromosome.num_recurrences[i]);
        }
        json_creature +=
        @"],
        ";        

        json_creature +=
        @"""limbs"" : {
        ";

        int branch_count = chromosome.getBranchCount();
        for(int i=0; i<branch_count; ++i)
        {
            string branch_string =
            @"

            ""{0}"" : [
            ";
            json_creature += string.Format(branch_string, i.ToString());

            ArrayList limbs = chromosome.getLimbs(i);
            for(int k=0; k<limbs.Count; ++k)
            {
                ArrayList attributes = (ArrayList)limbs[k];
                Vector3 position =  (Vector3)attributes[0];
                Vector3 scale =     (Vector3)attributes[1];

                string limb_string =
                @"{{
                    ""position"" : {{
                        ""x"": {1},
                        ""y"": {2},
                        ""z"": {3}
                    }},
                    ""scale"" : {{
                        ""x"": {4},
                        ""y"": {5},
                        ""z"": {6}
                    }}
                }}";

                if (!(k == limbs.Count - 1))
                    limb_string += @",
                    ";

                string[] l_args = {
                        k.ToString(),
                        position.x.ToString(), position.y.ToString(), position.z.ToString(),
                        scale.x.ToString(), scale.y.ToString(), scale.z.ToString()
                };
                json_creature += string.Format(limb_string, l_args);
            }

            json_creature +=
            @"]";

            if (!(i == branch_count - 1))
            {
                json_creature += @",
                ";
            }
        }

            json_creature +=
            @"
            }
            ";

        json_creature += 
        @"}";
json_creature +=
@"}";

        using (var sw = new StreamWriter(filename))
        {
            sw.Write(json_creature);
            sw.Close();
        }

        CreatureSaved();
    }	
}
