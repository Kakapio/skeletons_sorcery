using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blueflame : LootDropData
{
    public Blueflame(Sprite sprite) : base("Blueflame", sprite)
    {
    }

    public override void Activate()
    {
        var attackSystem = GameObject.FindWithTag("Player").GetComponent<ShootProjectile>();
        attackSystem.fireballPrefab = attackSystem.bluefireballPrefab; // Upgrade our attack!
    }
}
