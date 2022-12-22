using Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Aspects
{
    internal readonly partial struct TurretAspect : IAspect
    {
        private readonly RefRO<Turret> _turret;
        private readonly RefRO<URPMaterialPropertyBaseColor> _baseColor;

        public Entity CannonBallSpawn => _turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefab => _turret.ValueRO.CannonBallPrefab;
        public float4 Color => _baseColor.ValueRO.Value;
    }
}
