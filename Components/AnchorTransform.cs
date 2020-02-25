using System.Collections.Generic;
using System.Linq;
using Assets.UITB.Attributes;
using Assets.UITB.Common;
using Assets.UITB.Extensions;
using UnityEngine;



namespace Assets.UITB.Components {
  /// <summary>
  /// Applies a transform relatively to its defined parent, like it is a direct child
  /// </summary>
  ///https://answers.unity.com/questions/932677/how-to-check-if-inspector-object-field-was-changed.html
  ///https://answers.unity.com/questions/1206632/trigger-event-on-variable-change.html
  ///https://answers.unity.com/questions/59983/callback-on-variable-change-inside-editor.html
  ///
  [Author( "Christian Borck", "christian.borck@b-tu.de", "https://www.b-tu.de/fg-automatisierungstechnik" )]
  [ExecuteInEditMode]
  public class AnchorTransform : MonoBehaviour {
    #region static members

    private const string PATH_LOCAL_POS = "transform.localPosition";
    private const string PATH_LOCAL_ROT = "transform.localRotation";
    private const string PATH_LOCAL_SCA = "transform.localScale";

    private static readonly ISet<string> PushMembers = new HashSet<string> {
      nameof(localPosition),
      nameof(localRotation),
      nameof(localScale),
      nameof(m_localRotation),
      nameof(anchor)
    };

    private static readonly ISet<string> PullMembers = new HashSet<string> {
      PATH_LOCAL_POS, PATH_LOCAL_ROT, PATH_LOCAL_SCA
    };


    #endregion


    #region members



    private readonly FieldRecordManager _recMan;
    public bool linkPosition = true;
    /// <summary>
    /// Link local position
    /// </summary>
    [SerializeField]
    public Vector3 localPosition = Vector3.zero;

    public bool linkRotation = true;
    /// <summary>
    /// Link local rotation as euler angles, only for inspector
    /// </summary>
    [SerializeField]
    private Vector3 m_localRotation = Vector3.zero;

    /// <summary>
    /// Link local rotation
    /// </summary>
    [SerializeField]
    [HideInInspector]
    public Quaternion localRotation = Quaternion.identity;


    public bool linkScale = true;
    /// <summary>
    /// Link local scale
    /// </summary>
    [SerializeField]
    public Vector3 localScale = Vector3.one;



    /// <summary>
    /// Linked parent as transform reference
    /// </summary>
    public Transform anchor;


    /// <summary>
    /// World transform relative applying <see cref="localPosition"/>, <see cref="localRotation"/> and <see cref="localScale"/> on anchor transform
    /// </summary>
    public TransformData anchorRelativeWorldTransform {
      get {
        var localTfBackup = TransformData.FromLocalTransform( transform );

        transform.SetLocalTRSRelativeTo( anchor,
                                         localPosition,
                                         localRotation,
                                         localScale );

        var anchorRelativeWorldTransform = TransformData.FromWorldTransform( transform );
        localTfBackup.ApplyLocalTRSTo( transform );
        return anchorRelativeWorldTransform;
      }
    }

    public TransformData ParentRelativeWorldTransform => TransformData.FromWorldTransform( transform );


    #endregion



    private AnchorTransform() {
      _recMan = new FieldRecordManager( this, FieldRecordManager.AddMode.AllFields );
      _recMan.AddRecorder( PATH_LOCAL_POS );
      _recMan.AddRecorder( PATH_LOCAL_ROT );
      _recMan.AddRecorder( PATH_LOCAL_SCA );
    }



    private void Start() {
      if (anchor) {
        PushTransform();
      }
      _recMan.RecordAll();
    }


    private void LateUpdate() {
      var propChanges = _recMan.GetChangesAsDictionary();
      if (anchor) {
        if (PushMembers.Intersect( propChanges.Keys ).Any()) {
          if (propChanges.ContainsKey( nameof( m_localRotation ) )) {
            localRotation = Quaternion.Euler( m_localRotation );
          }
          PushTransform();
        } else if (PullMembers.Intersect( propChanges.Keys ).Any()) {
          PullTransform();
        }
      }
      _recMan.RecordAll();
    }



    private void PullTransform() {
      var (lt, lr, ls) = transform.GetLocalTRSRelativeTo( anchor );
      //Debug.Log( $"{name}.{nameof( AnchorTransform )}: pull transform: {transform}" );
      localPosition = lt;
      localRotation = lr;
      m_localRotation = lr.eulerAngles;
      localScale = ls;
    }



    private void PushTransform() {
      PushTransform( anchorRelativeWorldTransform );
    }


    /// <summary>
    /// Apply world coordinate of anchor.transform * script.localTransform to gameObject.worldTransform now
    /// </summary>
    public void ApplyNow() {
      PushTransform();
      _recMan.RecordAll();
    }



    private void PushTransform(TransformData worldTransform) {
      if (linkPosition) {
        transform.position = worldTransform.position;
      }
      if (linkRotation) {
        transform.rotation = worldTransform.rotation;
      }
      if (linkScale) {
        transform.SetLossyScale( worldTransform.scale );
      }
      //Debug.Log( $"{name}.{nameof( AnchorTransform )}: push transform, {transform}" );
    }
  }
}
