using System;
using UnityEngine;
using Timberborn.BlueprintSystem;
using Timberborn.Coordinates;
using Timberborn.Navigation;
using Timberborn.PrefabOptimization;
using Timberborn.Rendering;
using Timberborn.SingletonSystem;
using Timberborn.BuildingsNavigation;

namespace VerticalNavMesh {
  public class VerticalNavMeshDrawer : ILoadableSingleton, IUnloadableSingleton {
    internal static VerticalNavMeshDrawer Singleton { get; private set; }

    private readonly ISpecService _specService;

    private readonly DistanceToColorConverter _distanceToColorConverter;

    private Material _material;

    private readonly MeshBuilder _meshBuilder = new();

    private IntermediateMesh _mesh;

    private readonly Mesh _builtMesh;

    public VerticalNavMeshDrawer(ISpecService specService, DistanceToColorConverter distanceToColorConverter) {
      _specService = specService;
      _distanceToColorConverter = distanceToColorConverter;
      // PathMeshDrawer()
      _builtMesh = _meshBuilder.Build().Mesh;
      _builtMesh.MarkDynamic();
    }

    public void Load() {
      VerticalNavMeshSpec verticalNavMeshSpec = _specService.GetSingleSpec<VerticalNavMeshSpec>();
      // PathMeshDrawerFactory.Create
      _material = verticalNavMeshSpec.Material.Asset;
      // PathMeshDrawerFactory.GenerateNeighboredMeshes
      Material[] materials = new Material[1] { _material };
      Mesh mesh = verticalNavMeshSpec.Model.Asset;
      // PathMeshDrawerFactory.AddVariant
      _meshBuilder.Reset("");
      _meshBuilder.AppendMesh(mesh, materials, new OrientationTransform(Orientation.Cw0));
      _mesh = _meshBuilder.BuildIntermediateMesh();

      Singleton = this;
    }

    public void Unload() {
      Singleton = null;
    }

    public void Reset() {
      _meshBuilder.Reset("");
    }

    public void Draw() {
      if (_builtMesh.vertexCount > 0) {
        Graphics.DrawMesh(_builtMesh, Vector3.zero, Quaternion.identity, _material, Layers.UILayer, null, 0, null, castShadows: false, receiveShadows: false, useLightProbes: false);
      }
    }

    public void Build() {
      _meshBuilder.Build(_builtMesh);
    }

    public void Add(WeightedCoordinates node) {
      Vector3Int coordinates = node.Coordinates;
      Vector3 translation = CoordinateSystem.GridToWorldCentered(coordinates) + new Vector3(0f, PathMeshDrawer.VerticalOffset, 0f);
      Array.Fill(_mesh.Colors, _distanceToColorConverter.DistanceToColor(node.Distance));
      _meshBuilder.AppendIntermediateMesh(_mesh, new TranslationTransform(translation));
    }

  }
}