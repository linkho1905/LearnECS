using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    internal class ConfigAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject tankPrefab;
        [SerializeField] private int tankCount;
        [SerializeField] private float safeZoneRad;

        private class ConfigBaker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                AddComponent(new Config
                {
                    TankPrefab = GetEntity(authoring.tankPrefab),
                    TankCount = authoring.tankCount,
                    SafeZoneRad = authoring.safeZoneRad
                });
            }
        }
    }

    internal struct Config : IComponentData
    {
        public Entity TankPrefab { get; set; }
        public int TankCount { get; set; }
        public float SafeZoneRad { get; set; }
    }
}