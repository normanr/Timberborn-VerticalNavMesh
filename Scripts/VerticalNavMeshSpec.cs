using Timberborn.BlueprintSystem;
using UnityEngine;

namespace VerticalNavMesh {
  internal record VerticalNavMeshSpec : ComponentSpec {
    [Serialize]
    public AssetRef<Mesh> Model { get; init; }

    [Serialize]
    public AssetRef<Material> Material { get; init; }
  }
}
