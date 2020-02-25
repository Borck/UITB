using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class TransformX {
    /// <summary>
    ///   Inverse transforms the <see cref="localPoint" /> using <see cref="fromSpace" /> and transforms the result using
    ///   <see cref="toSpace" />.
    /// </summary>
    /// <param name="localPoint"></param>
    /// <param name="fromSpace"></param>
    /// <param name="toSpace"></param>
    /// <returns></returns>
    public static Vector3 TransformPoint(this Transform toSpace, Vector3 localPoint, Transform fromSpace) {
      return fromSpace == toSpace
               ? localPoint
               : toSpace.TransformPoint( fromSpace.InverseTransformPoint( localPoint ) );
    }



    /// <summary>
    /// Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <param name="localScale"></param>
    /// <param name="destination"></param>
    /// <returns></returns>
    public static (Vector3 pos, Quaternion rot, Vector3 scale) Transform(
      this Transform transform,
      Vector3 localPosition,
      Quaternion localRotation,
      Vector3 localScale,
      Transform destination) {
      var localMat = Matrix4x4.TRS( localPosition, localRotation, localScale );
      var destMatrix = destination.worldToLocalMatrix * transform.localToWorldMatrix * localMat;

      return (destMatrix.GetPosition(), destMatrix.GetRotation(), destMatrix.GetScale());
    }



    public static void SetLocalTRSRelativeTo(this Transform transform,
                                             Transform anchor,
                                             Vector3 localPosition,
                                             Quaternion localRotation,
                                             Vector3 localScale) {
      var parent = transform.parent;
      transform.parent = anchor;
      transform.localPosition = localPosition;
      transform.localRotation = localRotation;
      transform.localScale = localScale;
      transform.parent = parent;
    }

    public static void SetLocalPoseRelativeTo(this Transform transform,
                                             Transform anchor,
                                             Pose localPose) {
      var parent = transform.parent;
      transform.parent = anchor;
      transform.localPosition = localPose.position;
      transform.localRotation = localPose.rotation;
      transform.parent = parent;
    }



    public static (Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
      GetLocalTRSRelativeTo(this Transform transform,
                                               Transform anchor) {
      var parent = transform.parent;
      transform.parent = anchor;
      var lt = transform.localPosition;
      var lr = transform.localRotation;
      var ls = transform.localScale;
      transform.parent = parent;
      return (lt, lr, ls);
    }



    public static Bounds TransformBounds(this Transform transform, Bounds localBounds) {
      var center = transform.TransformPoint( localBounds.center );

      // transform the local extents' axes
      var extents = localBounds.extents;
      var axisX = transform.TransformVector( extents.x, 0, 0 );
      var axisY = transform.TransformVector( 0, extents.y, 0 );
      var axisZ = transform.TransformVector( 0, 0, extents.z );

      // sum their absolute value to get the world extents
      extents.x = Mathf.Abs( axisX.x ) + Mathf.Abs( axisY.x ) + Mathf.Abs( axisZ.x );
      extents.y = Mathf.Abs( axisX.y ) + Mathf.Abs( axisY.y ) + Mathf.Abs( axisZ.y );
      extents.z = Mathf.Abs( axisX.z ) + Mathf.Abs( axisY.z ) + Mathf.Abs( axisZ.z );

      return new Bounds { center = center, extents = extents };
    }



    public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds) {
      var center = transform.InverseTransformPoint( worldBounds.center );

      // transform the local extents' axes
      var extents = worldBounds.extents;
      var axisX = transform.InverseTransformVector( extents.x, 0, 0 );
      var axisY = transform.InverseTransformVector( 0, extents.y, 0 );
      var axisZ = transform.InverseTransformVector( 0, 0, extents.z );

      // sum their absolute value to get the world extents
      extents.x = Mathf.Abs( axisX.x ) + Mathf.Abs( axisY.x ) + Mathf.Abs( axisZ.x );
      extents.y = Mathf.Abs( axisX.y ) + Mathf.Abs( axisY.y ) + Mathf.Abs( axisZ.y );
      extents.z = Mathf.Abs( axisX.z ) + Mathf.Abs( axisY.z ) + Mathf.Abs( axisZ.z );

      return new Bounds { center = center, extents = extents };
    }



    public static void SetLossyScale(this Transform transform, Vector3 scale) {
      var parent = transform.parent;
      transform.parent = null;
      transform.localScale = scale;
      transform.parent = parent;
    }





    /// <summary>
    ///   Returns the next <see cref="Transform" /> in the scene tree.
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Transform Next(this Transform transform) {
      // Check where we are
      var iNext = transform.GetSiblingIndex() + 1;
      return transform.parent != null &&
             iNext < transform.parent.childCount
               ? transform.parent.GetChild( iNext )
               : default;
    }



    /// <summary>
    ///   Returns the previous <see cref="Transform" /> in the scene tree.
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static Transform Previous(this Transform transform) {
      // Check where we are
      var iPrevious = transform.GetSiblingIndex() - 1;
      return transform.parent != null &&
             iPrevious >= 0
               ? transform.parent.GetChild( iPrevious )
               : default;
    }



    public static Matrix4x4 GetMatrixTo(this Transform source, Transform destination) {
      return destination.worldToLocalMatrix * source.localToWorldMatrix;
    }


    public static Pose GetLocalPose(this Transform transform) {
      return new Pose( transform.localPosition, transform.localRotation );
    }



    public static void SetLocalPose(this Transform transform, Pose localPose) {
      transform.localPosition = localPose.position;
      transform.localRotation = localPose.rotation;
    }



    public static Matrix4x4 GetLocalMatrix(this Transform transform) {
      return Matrix4x4.TRS(
        transform.localPosition,
        transform.localRotation,
        transform.localScale
      );
    }
  }
}
