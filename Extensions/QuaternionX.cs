using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class QuaternionX {
    public static Vector4 ToVector4(this Quaternion quaternion) {
      return new Vector4(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }



    public static Quaternion FromVector4(Vector4 vector) {
      return new Quaternion(vector.x, vector.y, vector.z, vector.w);
    }



    /// <summary>
    ///   Calculates the delta rotation required to transform the fromRotation the the toRotation with the GetTransformedBy method
    /// </summary>
    /// <param name="fromRotation"></param>
    /// <param name="toRotation"></param>
    /// <returns></returns>
    public static Quaternion Delta(this Quaternion fromRotation, Quaternion toRotation) {
      return toRotation * Quaternion.Inverse(fromRotation);
    }
  }
}
