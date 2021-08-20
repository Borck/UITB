using System.Collections.Generic;
using System.Linq;
using Assets.UITB.Common;
using Assets.UITB.Extensions;
using UnityEngine;



namespace Assets.UITB.Components {
  /// <summary>
  ///   Applies a transform relatively to its defined parent, like it is a direct child
  /// </summary>
  [ExecuteInEditMode]
  public class AnchorTransform : MonoBehaviour {
    #region static members

    private const string PathLocalPos = "transform.localPosition";
    private const string PathLocalRot = "transform.localRotation";
    private const string PathLocalSca = "transform.localScale";

    private static readonly ISet<string> PushMembers = new HashSet<string> {
      nameof(localPosition),
      nameof(localRotation),
      nameof(localScale),
      nameof(_mLocalRotation),
      nameof(AnchorOverride)
    };

    private static readonly ISet<string> PullMembers = new HashSet<string> {PathLocalPos, PathLocalRot, PathLocalSca};

    #endregion


    #region members

    private readonly RecordedFieldsManager _recMan;
    public bool LinkPosition = true;

    /// <summary>
    ///   Link local position
    /// </summary>
    [SerializeField]
    // ReSharper disable once InconsistentNaming
    public Vector3 localPosition = Vector3.zero;

    public bool LinkRotation = true;

    /// <summary>
    ///   Link local rotation as euler angles, only for inspector
    /// </summary>
    [SerializeField]
    private Vector3 _mLocalRotation = Vector3.zero;

    /// <summary>
    ///   Link local rotation
    /// </summary>
    [SerializeField]
    [HideInInspector]
    // ReSharper disable once InconsistentNaming
    public Quaternion localRotation = Quaternion.identity;


    public bool LinkScale = true;

    /// <summary>
    ///   Link local scale
    /// </summary>
    [SerializeField]
    // ReSharper disable once InconsistentNaming
    public Vector3 localScale = Vector3.one;



    /// <summary>
    ///   Linked parent as transform reference
    /// </summary>
    public Transform AnchorOverride;


    /// <summary>
    ///   World transform relative applying <see cref="localPosition" />, <see cref="localRotation" /> and
    ///   <see cref="localScale" /> on anchor transform
    /// </summary>
    public TransformData anchorRelativeWorldTransform {
      get {
        var localTfBackup = TransformData.FromLocalTransform(transform);

        transform.SetLocalTRSRelativeTo(
          AnchorOverride,
          localPosition,
          localRotation,
          localScale
        );

        var anchorRelativeWorldTransform = TransformData.FromWorldTransform(transform);
        localTfBackup.ApplyLocalTRSTo(transform);
        return anchorRelativeWorldTransform;
      }
    }

    public TransformData ParentRelativeWorldTransform => TransformData.FromWorldTransform(transform);

    #endregion



    private AnchorTransform() {
      _recMan = new RecordedFieldsManager(this, RecordedFieldsManager.InitializationMode.AllFields);
      _recMan.AddRecorder(PathLocalPos);
      _recMan.AddRecorder(PathLocalRot);
      _recMan.AddRecorder(PathLocalSca);
    }



    private void Start() {
      if (AnchorOverride) {
        PushTransform();
      }

      _recMan.RecordAll();
    }



    private void LateUpdate() {
      var propChanges = _recMan.GetChangesAsDictionary();
      if (AnchorOverride) {
        if (PushMembers.Intersect(propChanges.Keys).Any()) {
          if (propChanges.ContainsKey(nameof(_mLocalRotation))) {
            localRotation = Quaternion.Euler(_mLocalRotation);
          }

          PushTransform();
        } else if (PullMembers.Intersect(propChanges.Keys).Any()) {
          PullTransform();
        }
      }

      _recMan.RecordAll();
    }



    private void PullTransform() {
      var (lt, lr, ls) = transform.GetLocalTRSRelativeTo(AnchorOverride);
      //Debug.Log( $"{name}.{nameof( AnchorTransform )}: pull transform: {transform}" );
      localPosition = lt;
      localRotation = lr;
      _mLocalRotation = lr.eulerAngles;
      localScale = ls;
    }



    private void PushTransform() => PushTransform(anchorRelativeWorldTransform);



    /// <summary>
    ///   Apply world coordinate of anchor.transform * script.localTransform to gameObject.worldTransform now
    /// </summary>
    public void ApplyNow() {
      PushTransform();
      _recMan.RecordAll();
    }



    private void PushTransform(TransformData worldTransform) {
      if (LinkPosition) {
        transform.position = worldTransform.position;
      }

      if (LinkRotation) {
        transform.rotation = worldTransform.rotation;
      }

      if (LinkScale) {
        transform.SetLossyScale(worldTransform.scale);
      }

      //Debug.Log( $"{name}.{nameof( AnchorTransform )}: push transform, {transform}" );
    }
  }
}
