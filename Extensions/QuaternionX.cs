using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class QuaternionX {
    public static Vector4 ToVector4(this Quaternion quaternion) {
      return new Vector4( quaternion.x, quaternion.y, quaternion.z, quaternion.w );
    }



    public static Quaternion FromVector4(Vector4 vector) {
      return new Quaternion( vector.x, vector.y, vector.z, vector.w );

    }
  }
}
