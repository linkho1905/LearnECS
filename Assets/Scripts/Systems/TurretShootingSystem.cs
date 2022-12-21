using Aspects;
using Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
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
        { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _worldTransformLookup.Update(ref state);
            var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var turretShootJob = new TurretShoot
            {
                WorldTransformLookup = _worldTransformLookup,
                ECB = ecb
            };
            turretShootJob.Schedule();
        }
    }
    
    [BurstCompile]
    internal partial struct TurretShoot : IJobEntity
    {
        [ReadOnly] public ComponentLookup<WorldTransform> WorldTransformLookup;
        public EntityCommandBuffer ECB;

        private void Execute(in TurretAspect turretAspect)
        {
            for (int i = 0; i < 10; i++)
            {
                var instance = ECB.Instantiate(turretAspect.CannonBallPrefab);
                var spawnLocalToWorld = WorldTransformLookup[turretAspect.CannonBallSpawn];
                var cannonBallTransform = LocalTransform.FromPosition(spawnLocalToWorld.Position);
                cannonBallTransform.Scale = WorldTransformLookup[turretAspect.CannonBallPrefab].Scale;
                ECB.SetComponent(instance, cannonBallTransform);
                ECB.SetComponent(instance, new CannonBall
                {
                    Speed = spawnLocalToWorld.Forward() * 20.0f
                });
            }
        }
    }
}