using Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct TurretRotationSystem : ISystem
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
            var rotation = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
            foreach (var transformAspect in SystemAPI.Query<TransformAspect>().WithAll<Turret>())
            {
                transformAspect.RotateWorld(rotation);
            }
        }
    }
}
