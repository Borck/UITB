using System;
using System.Collections.Generic;
using Assets.UITB.Extensions;
using UnityEngine;



namespace Assets.UITB.Components {
  /// <summary>
  ///   This script allows to create a <see cref="ExplodeView" /> of a assigned <see cref="GameObject" />.
  ///   Example: <see href="https://i.stack.imgur.com/9vo0y.jpg" />
  /// </summary>
  public class ExplodeView : MonoBehaviour {
    private readonly IList<(Transform transform, Vector3 pivot_rootLocal)> _cache =
      new List<(Transform transform, Vector3 pivot_rootLocal)>();

    private float _amountUpdated = -1;

    private Vector3 _validPivot_rootLocal;

    /// <summary>
    ///   The amount of the explosion. The minimum is zero and provide the <see cref="GameObject" /> as original.
    /// </summary>
    [Range(0f, 10f)]
    [Tooltip("The amount of the explosion")]
    public float Amount;

    public Transform AnchorOverride;



    // Start is called before the first frame update
    private void Start() => UpdateTransformCache(false);



    // Update is called once per frame
    private void Update() {
      var nextAmount = Amount;
      //    if (nextAmount < 0) {
      //      nextAmount = 0;
      //      Amount = 0;
      //    }

      var rootTransform = GetRootTransform();
      var pivot_rootLocal = GetPivotPoint(rootTransform);

      if (IsTransformCacheInvalid()) {
        //TODO update transform here will give wrong 'center_rootLocal' in '_transformCache'
        //UpdateTransformCache();
      } else {
        if (Math.Abs(_amountUpdated - nextAmount) < 0.0005 &&
            (pivot_rootLocal - _validPivot_rootLocal).magnitude < 0.0005) {
          return; //nothing changed -> skip
        }
      }

      foreach (var childMeta in _cache) {
        var pos_rootLocal = (childMeta.pivot_rootLocal - pivot_rootLocal) * nextAmount;
        var pos_parentLocal = rootTransform.TransformPoint(pos_rootLocal, childMeta.transform.parent);
        childMeta.transform.localPosition = pos_parentLocal;
      }

      Amount = nextAmount;
      _amountUpdated = nextAmount;
      _validPivot_rootLocal = pivot_rootLocal;
    }



    private bool IsTransformCacheInvalid() =>
      //TODO extend if gameObject will change in space or structure
      false;



    private void UpdateTransformCache(bool clear = true) {
      if (clear) {
        _cache.Clear();
      }

      var rootTransform = GetRootTransform();

      foreach (Transform childTransform in gameObject.transform) {
        var childRenderer = childTransform.gameObject.GetComponent<Renderer>();
        if (childRenderer == null) {
          continue;
        }

        var childCenter_rootLocal = rootTransform.InverseTransformPoint(childRenderer.bounds.center);
        _cache.Add((childTransform, childCenter_rootLocal));
      }
    }



    private Transform GetRootTransform() => gameObject.transform;



    private Vector3 GetPivotPoint(Transform rootTransform) {
      var anchorTransform = GetAnchorTransform();
      return anchorTransform.TransformPoint(anchorTransform.localPosition, rootTransform);
    }



    private Transform GetAnchorTransform() => AnchorOverride != null ? AnchorOverride : gameObject.transform;
  }
}
