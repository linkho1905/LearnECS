using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    internal class TankAuthoring : MonoBehaviour
    {
        private class TankBaker : Baker<TankAuthoring>
        {
            public override void Bake(TankAuthoring authoring)
            {
                AddComponent<Tank>();
            }
        }
    }

    internal struct Tank : IComponentData
    {
    }
}
