using UnityEngine;

namespace VerticalNavMesh {
  public static class VectorExtensions {

    public static Vector3Int Right(this Vector3Int value) {
      return new Vector3Int(value.x + 1, value.y, value.z);
    }

    public static Vector3Int Left(this Vector3Int value) {
      return new Vector3Int(value.x - 1, value.y, value.z);
    }

    public static Vector3Int Up(this Vector3Int value) {
      return new Vector3Int(value.x, value.y + 1, value.z);
    }

    public static Vector3Int Down(this Vector3Int value) {
      return new Vector3Int(value.x, value.y - 1, value.z);
    }

  }
}