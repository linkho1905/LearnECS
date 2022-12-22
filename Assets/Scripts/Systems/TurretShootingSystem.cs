using Aspects;
using Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    internal partial struct TurretShootingSystem : ISystem
    {
        private ComponentLookup<WorldTransform> _worldTransformLookup;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _worldTransformLookup = state.GetComponentLookup<WorldTransform>(true);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _worldTransformLookup.Update(ref state);
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var shootJob = new TurretShoot
            {
                WorldTransformLookup = _worldTransformLookup,
                Ecb = ecb
            };
            shootJob.Schedule();
        }
    }
    
    [BurstCompile]
    internal partial struct TurretShoot : IJobEntity
    {
        [field: ReadOnly] public ComponentLookup<WorldTransform> WorldTransformLookup { get; set; }
        public EntityCommandBuffer Ecb { get; set; }

        // Note that the TurretAspects parameter is "in", which declares it as read only.
        // Making it "ref" (read-write) would not make a difference in this case, but you
        // will encounter situations where potential race conditions trigger the safety system.
        // So in general, using "in" everywhere possible is a good principle.
        private void Execute(in TurretAspect turretAspect)
        {
            var instance = Ecb.Instantiate(turretAspect.CannonBallPrefab);
            var spawnLocalToWorld = WorldTransformLookup[turretAspect.CannonBallSpawn];
            var cannonBallTransform = LocalTransform.FromPosition(spawnLocalToWorld.Position);
            // We are about to overwrite the transform of the new instance. If we didn't explicitly
            // copy the scale it would get reset to 1 and we'd have oversized cannon balls.
            cannonBallTransform.Scale = WorldTransformLookup[turretAspect.CannonBallPrefab].Scale;
            Ecb.SetComponent(instance, cannonBallTransform);
            Ecb.SetComponent(instance, new CannonBall
            {
                Speed = spawnLocalToWorld.Forward() * 20.0f
            });
            Ecb.SetComponent(instance, new URPMaterialPropertyBaseColor
            {
                Value = turretAspect.Color
            });
        }
    }
}