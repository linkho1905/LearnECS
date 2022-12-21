using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    // Authoring MonoBehaviours are regular GameObject components.
    // They constitute the inputs for the baking systems which generates ECS data.
    internal class TurretAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject cannonBallPrefab;
        [SerializeField] private Transform cannonBallSpawn;
        
        // Bakers convert authoring MonoBehaviours into entities and components.
        // (Nesting a baker in its associated Authoring component is not necessary but is a common convention.)
        private class TurretBaker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                AddComponent(new Turret
                {
                    // By default, each authoring GameObject turns into an Entity.
                    // Given a GameObject (or authoring component), GetEntity looks up the resulting Entity.
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
