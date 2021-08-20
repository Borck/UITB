using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class MeshFilterX {
    public static Mesh GetMesh(this MeshFilter meshFilter, out bool isShared) {
      if (meshFilter.sharedMesh != null) {
        isShared = true;
        return meshFilter.sharedMesh;
      }

      isShared = false;
      return meshFilter.mesh;
    }
  }
}
