using Authoring;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Aspects
{
    internal readonly partial struct CannonBallAspect : IAspect
    {
        public readonly Entity Self;
        private readonly TransformAspect _transform;
        private readonly RefRW<CannonBall> _cannonBall;

        public float3 Position
        {
            get => _transform.LocalPosition;
            set => _transform.LocalPosition = value;
        }

        public float3 Speed
        {
            get => _cannonBall.ValueRO.Speed;
            set => _cannonBall.ValueRW.Speed = value;
        }
    }
}