using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
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
                AddComponent<URPMaterialPropertyBaseColor>();
            }
        }
    }

    internal struct CannonBall : IComponentData
    {
        public float3 Speed { get; set; }
    }
}
