using System;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class Matrix4x4X {
    public static (Vector3 t, Quaternion r, Vector3 s) TransformTRS(this Matrix4x4 matrix,
                                                                    Vector3 t,
                                                                    Quaternion r,
                                                                    Vector3 s) {
      var resultMat = matrix * Matrix4x4.TRS(t, r, s);
      return (resultMat.GetPosition(), resultMat.rotation, resultMat.GetScale());
    }



    public static Pose Transform(this Matrix4x4 matrix, Pose pose) {
      var resultMat = matrix * Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one);
      return new Pose(resultMat.GetPosition(), resultMat.rotation);
    }



    public static Quaternion GetRotation(this Matrix4x4 matrix) {
      var q = new Quaternion {
        w = Mathf.Sqrt(Mathf.Max(0, 1 + matrix.m00 + matrix.m11 + matrix.m22)) / 2,
        x = Mathf.Sqrt(Mathf.Max(0, 1 + matrix.m00 - matrix.m11 - matrix.m22)) / 2,
        y = Mathf.Sqrt(Mathf.Max(0, 1 - matrix.m00 + matrix.m11 - matrix.m22)) / 2,
        z = Mathf.Sqrt(Mathf.Max(0, 1 - matrix.m00 - matrix.m11 + matrix.m22)) / 2
      };
      q.x = _copysign(q.x, matrix.m21 - matrix.m12);
      q.y = _copysign(q.y, matrix.m02 - matrix.m20);
      q.z = _copysign(q.z, matrix.m10 - matrix.m01);
      return q;
    }



    public static Vector3 GetPosition(this Matrix4x4 matrix) {
      var x = matrix.m03;
      var y = matrix.m13;
      var z = matrix.m23;

      return new Vector3(x, y, z);
    }



    public static Vector3 GetScale(this Matrix4x4 m) {
      var x = Mathf.Sqrt(m.m00 * m.m00 + m.m01 * m.m01 + m.m02 * m.m02);
      var y = Mathf.Sqrt(m.m10 * m.m10 + m.m11 * m.m11 + m.m12 * m.m12);
      var z = Mathf.Sqrt(m.m20 * m.m20 + m.m21 * m.m21 + m.m22 * m.m22);

      return new Vector3(x, y, z);
    }



    public static float GetLossyScale(Transform t) {
      return t.lossyScale.x;
    }



    private static float _copysign(float sizeval, float signval) {
      return Math.Abs(Mathf.Sign(signval) - 1f) < float.Epsilon
               ? Mathf.Abs(sizeval)
               : -Mathf.Abs(sizeval);
    }
  }
}
