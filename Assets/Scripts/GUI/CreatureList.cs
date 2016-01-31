using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreatureList : MonoBehaviour
{
    public GameObject button_prefab;
    public List<GameObject> selections;

    CreatureInfoContainer creature_info;

    void Start ()
    {
        creature_info = CreatureInfoContainer.getInstance();
    }

    void OnEnable ()
    {
        SaveCreature.CreatureSaved += OnUpdate;
    }

    void OnDisable ()
    {
        SaveCreature.CreatureSaved -= OnUpdate;
    }

    public void PopulateMenu (SortedList<string, Chromosome> creatures)
    {
        DepopulateMenu();
               
        for (int i = 0; i < creatures.Count; ++i)
        {
            string name = creatures.Keys[i];
            Chromosome chromosome = creatures.Values[i];
            GameObject button = Instantiate(button_prefab, transform.position, transform.rotation) as GameObject;
            button.transform.SetParent(transform);
            LayoutElement le = button.AddComponent<LayoutElement>();
            le.minHeight = 30;

            LoadChromosome lc = button.GetComponent<LoadChromosome>();
            lc.parent = GetComponentInParent<UIElement>();
            lc.c = chromosome;

            Button b = button.GetComponent<Button>();
            Text t = button.GetComponentInChildren<Text>();
            t.text = name;

            Image[] colours = GameObject.FindObjectsOfType<Image>();
            colours[0].color = chromosome.colour;
            colours[1].color = chromosome.limb_colour;

            button.AddComponent<Image>();

            RectTransform rt = button.GetComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);

            selections.Add(button);
        }
    }

    public void DepopulateMenu ()
    {
        foreach (var s in selections)
        {
            Destroy(s);
        }
        selections.Clear();
    }

    void OnUpdate ()
    {
        DepopulateMenu();
        GetComponentInParent<ImportCreature>().LoadCreatures();
        PopulateMenu(creature_info.creatures);
    }
}
