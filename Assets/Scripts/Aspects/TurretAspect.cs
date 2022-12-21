using Authoring;
using Unity.Entities;

namespace Aspects
{
    internal readonly partial struct TurretAspect : IAspect
    {
        private readonly RefRO<Turret> _turret;

        public Entity CannonBallSpawn => _turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefab => _turret.ValueRO.CannonBallPrefab;
    }
}
