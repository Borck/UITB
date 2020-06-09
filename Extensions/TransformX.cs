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
    public static Vector3 TransformPoint(this Transform fromSpace, Vector3 localPoint, Transform toSpace) =>
      fromSpace == toSpace
        ? localPoint
        : toSpace.TransformPoint(fromSpace.InverseTransformPoint(localPoint));



    /// <summary>
    ///   Transform TRS from source space into destination space
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
      var localMat = Matrix4x4.TRS(localPosition, localRotation, localScale);
      var destMatrix = destination.worldToLocalMatrix * transform.localToWorldMatrix * localMat;

      return (destMatrix.GetPosition(), destMatrix.GetRotation(), destMatrix.GetScale());
    }



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <param name="localScale"></param>
    /// <returns></returns>
    public static (Vector3 pos, Quaternion rot, Vector3 scale) Transform(
      this Transform transform,
      Vector3 localPosition,
      Quaternion localRotation,
      Vector3 localScale) {
      var localMat = Matrix4x4.TRS(localPosition, localRotation, localScale);
      var destMatrix = transform.localToWorldMatrix * localMat;

      return (destMatrix.GetPosition(), destMatrix.GetRotation(), destMatrix.GetScale());
    }



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <returns></returns>
    public static Pose Transform(
      this Transform transform,
      Vector3 localPosition,
      Quaternion localRotation) {
      var localMat = Matrix4x4.TRS(localPosition, localRotation, Vector3.one);
      var destMatrix = transform.localToWorldMatrix * localMat;

      return new Pose(destMatrix.GetPosition(), destMatrix.GetRotation());
    }



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPose"></param>
    /// <returns></returns>
    public static Pose Transform(this Transform transform, Pose localPose)
      => transform.Transform(localPose.position, localPose.rotation);



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static (Vector3 pos, Quaternion rot, Vector3 scale) InverseTransform(
      this Transform transform,
      Vector3 position,
      Quaternion rotation,
      Vector3 scale) {
      var localMat = Matrix4x4.TRS(position, rotation, scale);
      var destMatrix = transform.worldToLocalMatrix * localMat;
      return (destMatrix.GetPosition(), destMatrix.GetRotation(), destMatrix.GetScale());
    }



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public static Pose InverseTransform(
      this Transform transform,
      Vector3 position,
      Quaternion rotation) {
      var localMat = Matrix4x4.TRS(position, rotation, Vector3.one);
      var destMatrix = transform.worldToLocalMatrix * localMat;
      return new Pose(destMatrix.GetPosition(), destMatrix.GetRotation());
    }



    /// <summary>
    ///   Transform TRS from source space into destination space
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="pose"></param>
    /// <returns></returns>
    public static Pose InverseTransform(this Transform transform, Pose pose)
      => InverseTransform(transform, pose.position, pose.rotation);



    public static void SetLocalTRSRelativeTo(this Transform transform,
                                             Transform anchor,
                                             Vector3 localPosition,
                                             Quaternion localRotation,
                                             Vector3 localScale) {
      var parentBackup = transform.parent;
      transform.parent = anchor;
      transform.localPosition = localPosition;
      transform.localRotation = localRotation;
      transform.localScale = localScale;
      transform.parent = parentBackup;
    }



    /// <summary>
    ///   Short hand function for (transform.localPosition, transform.localRotation, transform.localScale)
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static (Vector3 t, Quaternion r, Vector3 s) GetLocalTRS(this Transform transform) =>
      (transform.localPosition, transform.localRotation, transform.localScale);



    /// <summary>
    ///   Short hand function for (transform.localPosition, transform.localRotation, transform.localScale)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <param name="localScale"></param>
    /// <returns></returns>
    public static void SetLocalTRS(this Transform transform,
                                   Vector3 localPosition,
                                   Quaternion localRotation,
                                   Vector3 localScale) {
      transform.localPosition = localPosition;
      transform.localRotation = localRotation;
      transform.localScale = localScale;
    }



    /// <summary>
    ///   Short hand function for (transform.position, transform.rotation, transform.lossyScale)
    /// </summary>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static (Vector3 t, Quaternion r, Vector3 s) GetTRS(this Transform transform) =>
      (transform.position, transform.rotation, transform.lossyScale);



    /// <summary>
    ///   Short hand function for (transform.position, transform.rotation, transform.lossyScale)
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="lossyScale"></param>
    /// <returns></returns>
    public static void SetTRS(this Transform transform, Vector3 position, Quaternion rotation, Vector3 lossyScale) {
      transform.position = position;
      transform.rotation = rotation;
      transform.SetLossyScale(lossyScale);
    }



    public static (Vector3 t, Quaternion r, Vector3 s) InverseTransformToAnchor(this Transform transform,
                                                                                Transform anchor,
                                                                                Vector3 anchorPosition,
                                                                                Quaternion anchorRotation,
                                                                                Vector3 anchorScale) {
      var localTRSBackup = transform.GetLocalTRS();
      transform.SetLocalTRSRelativeTo(anchor, anchorPosition, anchorRotation, anchorScale);
      var trs = transform.GetLocalTRS();
      transform.SetLocalTRS(localTRSBackup.t, localTRSBackup.r, localTRSBackup.s);
      return trs;
    }



    public static (Vector3 t, Quaternion r, Vector3 s) TransformToAnchor(this Transform transform,
                                                                         Transform anchor,
                                                                         Vector3 localPosition,
                                                                         Quaternion localRotation,
                                                                         Vector3 localScale) {
      var localTRSBackup = transform.GetLocalTRS();
      transform.SetLocalTRS(localPosition, localRotation, localScale);
      var trs = transform.GetLocalTRSRelativeTo(anchor);
      transform.SetLocalTRS(localTRSBackup.t, localTRSBackup.r, localTRSBackup.s);
      return trs;
    }



    /// <summary>
    ///   Temporary set the local TRS of the transform and return the corresponding world TRS
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <param name="localScale"></param>
    /// <returns></returns>
    public static (Vector3 t, Quaternion r, Vector3 s) GetHypotheticalTRS(this Transform transform,
                                                                          Vector3 localPosition,
                                                                          Quaternion localRotation,
                                                                          Vector3 localScale) {
      var localTRSBackup = transform.GetLocalTRS();
      transform.SetLocalTRS(localPosition, localRotation, localScale);
      var trs = transform.GetTRS();
      transform.SetLocalTRS(localTRSBackup.t, localTRSBackup.r, localTRSBackup.s);
      return trs;
    }



    /// <summary>
    ///   Temporary set the local position of the transform and return the corresponding world position
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <returns></returns>
    public static Vector3 GetHypotheticalPosition(this Transform transform, Vector3 localPosition) {
      var localPosBackup = transform.localPosition;
      transform.localPosition = localPosition;
      var pos = transform.position;
      transform.localPosition = localPosBackup;
      return pos;
    }



    /// <summary>
    ///   Temporary set the local rotation of the transform and return the corresponding world rotation
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localRotation"></param>
    /// <returns></returns>
    public static Quaternion GetHypotheticalRotation(this Transform transform, Quaternion localRotation) {
      var localRotBackup = transform.localRotation;
      transform.localRotation = localRotation;
      var rot = transform.rotation;
      transform.localRotation = localRotBackup;
      return rot;
    }



    /// <summary>
    ///   Temporary set the local pose of the transform and return the corresponding world pose
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPose"></param>
    /// <returns></returns>
    public static Pose GetHypotheticalPose(this Transform transform, Pose localPose)
      => transform.GetHypotheticalPose(localPose.position, localPose.rotation);



    /// <summary>
    ///   Temporary set the local pose of the transform and return the corresponding world pose
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="localPosition"></param>
    /// <param name="localRotation"></param>
    /// <returns></returns>
    public static Pose GetHypotheticalPose(this Transform transform, Vector3 localPosition, Quaternion localRotation) {
      var localPosBackup = transform.localPosition;
      var localRotBackup = transform.localRotation;
      transform.localPosition = localPosition;
      transform.localRotation = localRotation;

      var pos = transform.position;
      var rot = transform.rotation;
      transform.localPosition = localPosBackup;
      transform.localRotation = localRotBackup;
      return new Pose(pos, rot);
    }



    /// <summary>
    ///   Temporary set the world TRS of the transform and return the corresponding local TRS
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="position">position in world coordinates</param>
    /// <param name="rotation"></param>
    /// <param name="scale"></param>
    /// <returns></returns>
    public static (Vector3 t, Quaternion r, Vector3 s) GetHypotheticalLocalTRS(this Transform transform,
                                                                               Vector3 position,
                                                                               Quaternion rotation,
                                                                               Vector3 scale) {
      var localTRSBackup = transform.GetLocalTRS();
      transform.SetTRS(position, rotation, scale);
      var trs = transform.GetLocalTRS();
      transform.SetLocalTRS(localTRSBackup.t, localTRSBackup.r, localTRSBackup.s);
      return trs;
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



    /// <summary>
    ///   Returns the local TRS as the anchor would be the direct parent.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="anchor"></param>
    /// <returns></returns>
    public static (Vector3 localPosition, Quaternion localRotation, Vector3 localScale)
      GetLocalTRSRelativeTo(this Transform transform,
                            Transform anchor) {
      var parentBackup = transform.parent;
      transform.parent = anchor;
      var lt = transform.localPosition;
      var lr = transform.localRotation;
      var ls = transform.localScale;
      transform.parent = parentBackup;
      return (lt, lr, ls);
    }



    public static Bounds TransformBounds(this Transform transform, Bounds localBounds) {
      var center = transform.TransformPoint(localBounds.center);

      // transform the local extents' axes
      var extents = localBounds.extents;
      var axisX = transform.TransformVector(extents.x, 0, 0);
      var axisY = transform.TransformVector(0, extents.y, 0);
      var axisZ = transform.TransformVector(0, 0, extents.z);

      // sum their absolute value to get the world extents
      extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
      extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
      extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

      return new Bounds {center = center, extents = extents};
    }



    public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds) {
      var center = transform.InverseTransformPoint(worldBounds.center);

      // transform the local extents' axes
      var extents = worldBounds.extents;
      var axisX = transform.InverseTransformVector(extents.x, 0, 0);
      var axisY = transform.InverseTransformVector(0, extents.y, 0);
      var axisZ = transform.InverseTransformVector(0, 0, extents.z);

      // sum their absolute value to get the world extents
      extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
      extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
      extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

      return new Bounds {center = center, extents = extents};
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
               ? transform.parent.GetChild(iNext)
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
               ? transform.parent.GetChild(iPrevious)
               : default;
    }



    public static Matrix4x4 GetMatrixTo(this Transform source, Transform destination) =>
      destination.worldToLocalMatrix * source.localToWorldMatrix;



    public static Pose GetLocalPose(this Transform transform) =>
      new Pose(transform.localPosition, transform.localRotation);



    public static void SetLocalPose(this Transform transform, Pose localPose) {
      transform.localPosition = localPose.position;
      transform.localRotation = localPose.rotation;
    }



    public static Pose GetPose(this Transform transform) => new Pose(transform.position, transform.rotation);



    public static Matrix4x4 GetLocalMatrix(this Transform transform) =>
      Matrix4x4.TRS(
        transform.localPosition,
        transform.localRotation,
        transform.localScale
      );
  }
}
