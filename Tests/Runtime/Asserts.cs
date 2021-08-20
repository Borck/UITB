using UnityEngine;
using UnityEngine.Assertions;



namespace Assets.MT_Sync.Tests {
  public static class Asserts {
    public static void AreApproximatelyEqual(Vector3 expected, Vector3 actual) {
      Assert.AreEqual(expected, actual, $"Differing rotations\nexpected: {expected}\nactual: {actual}");
    }



    public static void AreApproximatelyEqual(Quaternion expected,
                                             Quaternion actual,
                                             float toleranceEuler = 0.1f) {
      var deltaEuler = (Quaternion.Inverse(expected) * actual).eulerAngles;
      deltaEuler = new Vector3(
        Mathf.Abs(deltaEuler.x),
        Mathf.Abs(deltaEuler.y),
        Mathf.Abs(deltaEuler.z)
      );
      deltaEuler = new Vector3(
        Mathf.Min(deltaEuler.x, 360 - deltaEuler.x),
        Mathf.Min(deltaEuler.y, 360 - deltaEuler.y),
        Mathf.Min(deltaEuler.z, 360 - deltaEuler.z)
      );
      Assert.AreApproximatelyEqual(
        deltaEuler.magnitude,
        0,
        toleranceEuler,
        $"Differing rotations\nexpected: {expected}\nactual: {actual}"
      );
    }
  }
}
