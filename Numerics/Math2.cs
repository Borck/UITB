using System;
using UnityEngine;



namespace Assets.UITB.Numerics {
  public static class Math2 {
    public static int Clamp(int value, int lowerInclusive, int upperExclusive) =>
      value < lowerInclusive
        ? lowerInclusive
        : value >= upperExclusive
          ? upperExclusive - 1
          : value;



    /// <summary>
    ///   Calculates the area covered by a triangle
    /// </summary>
    /// <param name="pt1"></param>
    /// <param name="pt2"></param>
    /// <param name="pt3"></param>
    /// <returns></returns>
    public static double AreaOfTriangle(Vector3 pt1, Vector3 pt2, Vector3 pt3) {
      var a = (pt1 - pt2).magnitude;
      var b = (pt2 - pt3).magnitude;
      var c = (pt3 - pt1).magnitude;
      var s = (a + b + c) / 2;
      return Math.Sqrt(s * (s - a) * (s - b) * (s - c));
    }
  }
}
