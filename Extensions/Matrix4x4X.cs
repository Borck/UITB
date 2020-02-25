using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class Matrix4x4X {
    public static (Vector3 t, Quaternion r, Vector3 s) TransformTRS(this Matrix4x4 matrix, Vector3 t, Quaternion r, Vector3 s) {
      var resultMat = matrix * Matrix4x4.TRS( t, r, s );
      return (resultMat.GetPosition(), resultMat.rotation, resultMat.GetScale());
    }



    public static Pose Transform(this Matrix4x4 matrix, Pose pose) {
      var resultMat = matrix * Matrix4x4.TRS( pose.position, pose.rotation, Vector3.one );
      return new Pose( resultMat.GetPosition(), resultMat.rotation );
    }
  }
}
