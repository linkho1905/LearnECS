using Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Systems
{
    [BurstCompile]
    internal partial struct CannonBallSystem : ISystem 
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            var cannonBallJob = new CannonBallJob
            {
                Ecb = ecb.AsParallelWriter(),
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            cannonBallJob.ScheduleParallel();
        }
    }
    
    [BurstCompile]
    internal partial struct CannonBallJob : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter Ecb { get; set; }
        public float DeltaTime { get; set; }

        private void Execute([ChunkIndexInQuery] int chunkIndex, ref CannonBallAspect cannonBallAspect)
        {
            var gravity = new float3(0, -9.82f, 0.0f);
            var invertY = new float3(1.0f, -1.0f, 1.0f);
            cannonBallAspect.Position += cannonBallAspect.Speed * DeltaTime;
            if (cannonBallAspect.Position.y < 0.0f)
            {
                cannonBallAspect.Position *= invertY;
                cannonBallAspect.Speed *= invertY * 0.8f;
            }

            cannonBallAspect.Speed += gravity * DeltaTime;
            var speed = math.lengthsq(cannonBallAspect.Speed);
            if (speed < 0.1f)
                Ecb.DestroyEntity(chunkIndex, cannonBallAspect.Self);
        }
    }
}