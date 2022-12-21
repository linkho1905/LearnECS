using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    internal class TurretAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject cannonBallPrefab;
        [SerializeField] private Transform cannonBallSpawn;
        private class TurretBaker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                AddComponent(new Turret
                {
                    CannonBallPrefab = GetEntity(authoring.cannonBallPrefab),
                    CannonBallSpawn = GetEntity(authoring.cannonBallSpawn)
                });
            }
        }
    }
    
    // An ECS component.
    // An empty component is called a "tag component".
    internal struct Turret : IComponentData
    {
        public Entity CannonBallSpawn { get; set; }
        public Entity CannonBallPrefab { get; set; }
    }
}
