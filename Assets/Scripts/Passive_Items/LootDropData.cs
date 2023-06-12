using System;
using System.Collections;
using System.Collections.Generic;
using Passive_Items;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class LootDropData
{
    public string ItemName { get; private set; }
    public Sprite Sprite { get; private set; }
    public int Count { get; private set; }

    public LootDropData(string itemName, Sprite sprite, int count = 1)
    {
        ItemName = itemName;
        Sprite = sprite;
        Count = count;
    }

    public void IncrementCount()
    {
        Count++;
    }
}
