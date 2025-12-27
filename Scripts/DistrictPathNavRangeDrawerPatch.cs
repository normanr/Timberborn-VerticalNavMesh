using UnityEngine;
using HarmonyLib;
using Timberborn.BuildingsNavigation;
using Timberborn.Common;
using Timberborn.Navigation;

namespace VerticalNavMesh {

  [HarmonyPatch(typeof(DistrictPathNavRangeDrawer))]
  public static class DistrictPathNavRangeDrawerPatch {

    [HarmonyPatch(nameof(DistrictPathNavRangeDrawer.UpdateDrawers))]
    [HarmonyPrefix]
    private static void UpdateDrawersPrefix() {
      VerticalNavMeshDrawer.Singleton?.Reset();
    }

    [HarmonyPatch(nameof(DistrictPathNavRangeDrawer.UpdateDrawers))]
    [HarmonyPostfix]
    private static void UpdateDrawersPostfix() {
      VerticalNavMeshDrawer.Singleton?.Build();
    }

    [HarmonyPatch(nameof(DistrictPathNavRangeDrawer.Draw))]
    [HarmonyPostfix]
    private static void DrawPostfix() {
      VerticalNavMeshDrawer.Singleton?.Draw();
    }

    [HarmonyPatch(nameof(DistrictPathNavRangeDrawer.AddTile))]
    [HarmonyPrefix]
    public static bool AddTilePrefix(DistrictPathNavRangeDrawer __instance,
                                     WeightedCoordinates node) {
      bool MultipleConnections(Vector3Int coordinates, bool checkBelow = false) {
        bool d = __instance.IsConnected(coordinates, coordinates.Down());
        bool l = __instance.IsConnected(coordinates, coordinates.Left());
        bool u = __instance.IsConnected(coordinates, coordinates.Up());
        bool r = __instance.IsConnected(coordinates, coordinates.Right());
        bool b = __instance.IsConnected(coordinates, coordinates.Below());
        return (d && (l || u || r)) || (l && (u || r)) || (u && r) || 
               (checkBelow && b && (d || l || u || r));
      }

      Vector3Int coordinates = node.Coordinates;
      if (__instance.IsTileVisible(coordinates) && __instance._levelVisibilityService.BlockIsVisible(coordinates)) {
        if (__instance.IsConnectedToPath(coordinates, coordinates.Above())) {
          if (MultipleConnections(coordinates, true) || MultipleConnections(coordinates.Above())) {
            __instance._regularMeshDrawer.Add(node);
            VerticalNavMeshDrawer.Singleton?.Add(node);
            return false;
          }
        }
        if (__instance.IsConnectedToPath(coordinates, coordinates.Below())) {
          if (MultipleConnections(coordinates) || MultipleConnections(coordinates.Below())) {
            __instance._regularMeshDrawer.Add(node);
            return false;
          }
        }
      }
      return true;
    }
  }
}