using StarterAssets;
using UnityEngine;

namespace Passive_Items
{
    public class CuckooFeather : LootDropData
    {
        private readonly float jumpBoostAmount = 0.5f;
        
        public CuckooFeather(Sprite sprite) : base("CuckooFeather", sprite)
        {
        }
        
        public override void Activate()
        {
            var player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonController>();
            player.JumpHeight = Mathf.Clamp(player.JumpHeight + jumpBoostAmount, 1.2f, 6f);
        }
    }
}