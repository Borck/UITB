using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class VectorX {

    public static Vector3 ComponentWiseMultiply(this Vector3 a, Vector3 b)
    => new Vector3( a.x * b.x, a.y * b.y, a.z * b.z );
    public static Vector3 ComponentWiseDivide(this Vector3 a, Vector3 b)
    => new Vector3( a.x / b.x, a.y / b.y, a.z / b.z );
  }
}
