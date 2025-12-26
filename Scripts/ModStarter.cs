using HarmonyLib;
using Timberborn.ModManagerScene;

namespace VerticalNavMesh {
  internal class ModStarter : IModStarter {

    internal const string ModName = "Vertical Nav Mesh";

    public void StartMod(IModEnvironment modEnvironment) {
      var harmony = new Harmony(ModName);
      harmony.PatchAll();
    }
  }
}
