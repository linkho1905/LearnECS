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
            // The Entities.ForEach below is Burst compiled (implicitly).
            // And time is a member of SystemBase, which is a managed type (class).
            // This means that it wouldn't be possible to directly access Time from there.
            // So we need to copy the value we need (DeltaTime) into a local variable.
            var dt = SystemAPI.Time.DeltaTime;
            
            // Entities.ForEach is an older approach to processing queries. Its use is not
            // encouraged, but it remains convenient until we get feature parity with IFE.
            Entities
                .WithAll<Tank>()
                .ForEach((Entity entity, TransformAspect transform) =>
                {
                    var pos = transform.LocalPosition;
                    // This does not modify the actual position of the tank, only the point at
                    // which we sample the 3D noise function. This way, every tank is using a
                    // different slice and will move along its own different random flow field.
                    pos.y = entity.Index;
                    var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
                    var dir = float3.zero;
                    math.sincos(angle, out dir.x, out dir.z);
                    transform.LocalPosition += dir * dt * 5.0f;
                    transform.LocalRotation = quaternion.RotateY(angle);
                })
                // The last function call in the Entities.ForEach sequence controls how the code
                // should be executed: Run (main thread), Schedule (single thread, async), or
                // ScheduleParallel (multiple threads, async).
                // Entities.ForEach is fundamentally a job generator, and it makes it very easy to
                // create parallel jobs. This unfortunately comes with a complexity cost and weird
                // arbitrary constraints, which is why more explicit approaches are preferred.
                // Those explicit approaches (IJobEntity) are covered later in this tutorial.
                .ScheduleParallel();
        }
    }
}