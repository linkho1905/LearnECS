using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Authoring
{
    internal class CannonBallAuthoring : MonoBehaviour
    {
        private class CannonBallBaker : Baker<CannonBallAuthoring>
        {
            public override void Bake(CannonBallAuthoring authoring)
            {
                AddComponent<CannonBall>();
            }
        }
    }

    internal struct CannonBall : IComponentData
    {
        public float3 Speed { get; set; }
    }
}
