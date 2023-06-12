using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootCount : MonoBehaviour
{
    public GameObject LootImage;

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
            GameObject image = Instantiate(LootImage, transform.position, transform.rotation);
            image.transform.SetParent(transform);
            image.transform.localScale = new Vector3(1,1,1);
            image.transform.localPosition = new Vector3(-100 * num, 0, 0);
            image.GetComponent<Image>().sprite = item.Sprite;
            image.GetComponentInChildren<Text>().text = item.Count.ToString();
            num++;
        }
    }
}
