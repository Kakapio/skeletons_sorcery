using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootCount : MonoBehaviour
{
    public GameObject LootUI;

    void Start()
    {
        UpdateLootCounts();
    }

    public void UpdateLootCounts()
    {
        List<LootDropData> items = PlayerItems.items;
        int num = 0;
        foreach(LootDropData item in items)
        {
            GameObject icon = Instantiate(LootUI, transform.position, transform.rotation);
            icon.transform.SetParent(transform);
            icon.transform.localScale = new Vector3(0.8f, 0.8f, 1);
            icon.transform.localPosition = new Vector3(-120 * num, 0, 0);
            icon.GetComponentInChildren<Button>().gameObject.GetComponent<Image>().sprite = item.Sprite;
            icon.GetComponentInChildren<Text>().text = item.Count.ToString();
            num++;
        }
    }
}
