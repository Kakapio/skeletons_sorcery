using UnityEngine;

namespace Passive_Items
{
    public class ExampleLoot : LootDropData
    {
        private readonly int pointValue = 25;
        
        public ExampleLoot(Sprite sprite) : base("ExampleLoot", sprite)
        {
        }
        
        public override void Activate()
        {
            GameObject.FindWithTag("LevelManager").GetComponent<LevelManager>().UpdateScore(pointValue, "Collected money loot.");
        }
    }
}