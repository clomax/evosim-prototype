using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CreatureList : MonoBehaviour
{
    public GameObject button_prefab;
    public List<GameObject> selections;

    public void PopulateMenu(SortedList<string, Chromosome> creatures)
    {
        for (int i = 0; i < creatures.Count; ++i)
        {
            GameObject button = Instantiate(button_prefab, transform.position, transform.rotation) as GameObject;
            button.transform.SetParent(transform);
            LayoutElement le = button.AddComponent<LayoutElement>();
            le.preferredHeight = 30;

            LoadChromosome lc = button.GetComponent<LoadChromosome>();
            lc.parent = GetComponent<UIElement>();
            lc.c = creatures.Values[i];

            Button b = button.GetComponent<Button>();
            Text t = button.GetComponentInChildren<Text>();
            t.text = creatures.Keys[i];

            Image[] colours = GameObject.FindObjectsOfType<Image>();
            colours[0].color = creatures.Values[i].colour;
            colours[1].color = creatures.Values[i].limb_colour;

            button.AddComponent<Image>();

            RectTransform rt = button.GetComponent<RectTransform>();
            rt.localScale = new Vector3(1, 1, 1);

            selections.Add(button);
        }
    }

    public void DepopulateMenu()
    {
        foreach (var s in selections)
            Destroy(s);
    }
}
