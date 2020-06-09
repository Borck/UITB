using System.Collections.Generic;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class VectorX {
    public static Vector3 ComponentWiseMultiply(this Vector3 a, Vector3 b)
      => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);



    public static Vector3 ComponentWiseDivide(this Vector3 a, Vector3 b)
      => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);



    /// <summary>
    ///   Calculations the length of the path defined by the ordered positions
    /// </summary>
    /// <param name="positions"></param>
    /// <returns></returns>
    public static float Distance(this IEnumerable<Vector3> positions) {
      using (var etor = positions.GetEnumerator()) {
        if (!etor.MoveNext()) {
          return 0;
        }

        var currentPos = etor.Current;
        if (!etor.MoveNext()) {
          return 0;
        }

        float distance = 0;
        do {
          var nextPos = etor.Current;
          distance += (currentPos - nextPos).magnitude;
          currentPos = nextPos;
        } while (etor.MoveNext());

        return distance;
      }
    }
  }
}
