using Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    internal partial class TankMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = SystemAPI.Time.DeltaTime;
            Entities
                .WithAll<Tank>()
                .ForEach((TransformAspect transform) =>
                {
                    var pos = transform.LocalPosition;
                    var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
                    var dir = float3.zero;
                    math.sincos(angle, out dir.x, out dir.z);
                    transform.LocalPosition += dir * dt * 5.0f;
                    transform.LocalRotation = quaternion.RotateY(angle);
                })
                .ScheduleParallel();
        }
    }
}