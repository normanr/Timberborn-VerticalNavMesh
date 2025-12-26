using Bindito.Core;

namespace VerticalNavMesh {
	[Context("Game")]
	internal class VerticalNavMeshConfigurator : Configurator {
		protected override void Configure() {
			Bind<VerticalNavMeshDrawer>().AsSingleton();
		}
	}
}
