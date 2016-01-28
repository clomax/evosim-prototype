using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ImportCreature : MonoBehaviour
{
    string creatures_folder;
    string[] creature_files;

    StreamReader sr;
    string raw_contents;
    public JsonData contents;
    public SortedList<string, Chromosome> creatures;

    public GameObject selection_prefab;
    private UIElement ui_element;

    List<GameObject> selections;

    GameObject s;

    void OnEnable ()
    {
        SaveCreature.CreatureSaved += LoadCreatures;
    }

    void OnDisable()
    {
        SaveCreature.CreatureSaved -= LoadCreatures;
    }

    void Start ()
    {
        creatures = new SortedList<string, Chromosome>();
        creatures_folder = Application.dataPath + "/data/saved_creatures";
        ui_element = GetComponent<UIElement>();
        selections = new List<GameObject>();
    }

    public void OnVisible()
    {
        if (ui_element.visible)
        {
            LoadCreatures();
            PopulateMenu();
        }

        if(!ui_element.visible)
        {
            foreach(var s in selections)
            {
                GameObject.Destroy(s);
            }
        }
    }

    void LoadCreatures()
    {
        creatures.Clear();
        string[] fs;
        creature_files = Directory.GetDirectories(creatures_folder);
        foreach (var s in creature_files)
        {
            fs = Directory.GetFiles(s, "*.json");
            foreach (var f in fs)
            {
                sr = new StreamReader(f);
                raw_contents = sr.ReadToEnd();
                contents = JsonMapper.ToObject(raw_contents);
                sr.Close();

                string name = contents["name"].ToString();

                Color root_col = new Color();
                root_col.r = float.Parse(contents["attributes"]["colour"]["r"].ToString());
                root_col.g = float.Parse(contents["attributes"]["colour"]["g"].ToString());
                root_col.b = float.Parse(contents["attributes"]["colour"]["b"].ToString());
                root_col.a = 1;

                Color limb_col = new Color();
                limb_col.r = float.Parse(contents["attributes"]["limb_colour"]["r"].ToString());
                limb_col.g = float.Parse(contents["attributes"]["limb_colour"]["g"].ToString());
                limb_col.b = float.Parse(contents["attributes"]["limb_colour"]["b"].ToString());
                limb_col.a = 1;

                Vector3 root_scale = new Vector3();
                root_scale.x = float.Parse(contents["attributes"]["root_scale"]["x"].ToString());
                root_scale.y = float.Parse(contents["attributes"]["root_scale"]["y"].ToString());
                root_scale.z = float.Parse(contents["attributes"]["root_scale"]["z"].ToString());

                float bjf = float.Parse(contents["attributes"]["base_joint_frequency"].ToString());
                float bja = float.Parse(contents["attributes"]["base_joint_amplitude"].ToString());
                float bjp = float.Parse(contents["attributes"]["base_joint_phase"].ToString());
                float ht =  float.Parse(contents["attributes"]["hunger_threshold"].ToString());

                ArrayList branches =  new ArrayList();
                int num_branches = (int)contents["attributes"]["branches"];
                for (int j = 0; j < num_branches ; j++)
                {
                    ArrayList limbs = new ArrayList();
                    int recurrences = (int)contents["attributes"]["recurrences"][j];
                    for (int k = 0; k <= recurrences; k++)
                    {
                        float x = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["x"].ToString());
                        float y = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["y"].ToString());
                        float z = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["position"]["z"].ToString());
                        Vector3 position = new Vector3(x,y,z);

                        x = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["x"].ToString());
                        y = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["y"].ToString());
                        z = float.Parse(contents["attributes"]["limbs"][j.ToString()][k]["scale"]["z"].ToString());
                        Vector3 scale = new Vector3(x,y,z);

                        ArrayList limb = new ArrayList();
                        limb.Add(position);
                        limb.Add(scale);
                        limbs.Add(limb);
                    }
                    branches.Add(limbs);
                }
                Chromosome chromosome = new Chromosome();

                chromosome.colour = root_col;
                chromosome.limb_colour = limb_col;
                chromosome.hunger_threshold = ht;
                chromosome.setRootScale(root_scale);
                chromosome.setBaseFequency(bjf);
                chromosome.setBaseAmplitude(bja);
                chromosome.setBasePhase(bjp);
                chromosome.setBranches(branches);

                creatures.Add(name, chromosome);
            }
        }
    }

    void PopulateMenu ()
    {
        for (int i=0; i<creatures.Count; ++i)
        {
            s = Instantiate(selection_prefab, selection_prefab.transform.position, selection_prefab.transform.rotation) as GameObject;
            s.transform.SetParent(transform, false);
            selections.Add(s);
            s.GetComponent<RectTransform>().localPosition += new Vector3(0, (-40F*i), 0);

            LoadChromosome lc = s.GetComponent<LoadChromosome>();
            lc.parent = GetComponent<UIElement>();
            lc.c = creatures.Values[i];

            Button b = s.GetComponent<Button>();
            Text t = b.GetComponentInChildren<Text>();
            t.text = creatures.Keys[i];

            Image[] colours = new Image[2];
            colours = GameObject.FindObjectsOfType<Image>();
            colours[0].color = creatures.Values[i].colour;
            colours[1].color = creatures.Values[i].limb_colour;
        }
    }
}
