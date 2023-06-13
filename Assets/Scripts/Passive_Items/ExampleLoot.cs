using UnityEngine;

namespace Passive_Items
{
    public class ExampleLoot : LootDropData
    {
        private readonly int pointValue = 5;
        
        public ExampleLoot(Sprite sprite) : base("ExampleLoot", sprite)
        {
        }
        
        public override void Activate()
        {
            GameObject.FindObjectOfType<LevelManager>().UpdateScore(pointValue, "Found Gold");
        }
    }
}