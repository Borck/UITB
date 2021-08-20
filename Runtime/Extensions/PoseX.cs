using System.Collections.Generic;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class PoseX {
    /// <summary>
    ///   Calculates the delta pose required to transform the fromPose the the toPose with the GetTransformedBy method
    /// </summary>
    /// <param name="fromPose"></param>
    /// <param name="toPose"></param>
    /// <returns></returns>
    public static Pose Delta(this Pose fromPose, Pose toPose) {
      var fromRotInv = Quaternion.Inverse(fromPose.rotation);
      return new Pose(
        fromRotInv * (toPose.position - fromPose.position),
        toPose.rotation * fromRotInv
      );
    }



    /// <summary>
    ///   Calculations the length of the path defined by the ordered poses
    /// </summary>
    /// <param name="poses"></param>
    /// <returns></returns>
    public static float Distance(this IEnumerable<Pose> poses) {
      using (var etor = poses.GetEnumerator()) {
        if (!etor.MoveNext()) {
          return 0;
        }

        var currentPos = etor.Current.position;
        if (!etor.MoveNext()) {
          return 0;
        }

        float distance = 0;
        do {
          var nextPos = etor.Current.position;
          distance += (currentPos - nextPos).magnitude;
          currentPos = nextPos;
        } while (etor.MoveNext());

        return distance;
      }
    }
  }
}
