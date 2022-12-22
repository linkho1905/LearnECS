using Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    internal partial struct TankSpawnSystem : ISystem
    {
        // Queries should not be created on the spot in OnUpdate, so they are cached in fields.
        private EntityQuery _baseColorQuery;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            // This system should not run before the Config singleton has been loaded.
            state.RequireForUpdate<Config>();
            _baseColorQuery = state.GetEntityQuery(ComponentType.ReadOnly<URPMaterialPropertyBaseColor>());
            // var config = SystemAPI.GetSingleton<Config>();
            // var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
            // state.EntityManager.Instantiate(config.TankPrefab, vehicles);
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var config = SystemAPI.GetSingleton<Config>();
            //var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            //var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            // This system will only run once, so the random seed can be hard-coded.
            // Using an arbitrary constant seed makes the behavior deterministic.
            var random = Random.CreateFromIndex(1234);
            var hue = random.NextFloat();
            URPMaterialPropertyBaseColor RandomColor()
            {
                hue = (hue + 0.618034005f) % 1;
                var color = UnityEngine.Color.HSVToRGB(hue, 1.0f, 1.0f);
                return new URPMaterialPropertyBaseColor
                {
                    Value = (UnityEngine.Vector4)color
                };
            }
            var vehicles = CollectionHelper.CreateNativeArray<Entity>(config.TankCount, Allocator.Temp);
            //ecb.Instantiate(config.TankPrefab, vehicles);
            state.EntityManager.Instantiate(config.TankPrefab, vehicles);
            foreach (var entity in _baseColorQuery.ToEntityArray(Allocator.Temp))
                state.EntityManager.SetComponentData(entity, RandomColor());
        }
    }
}