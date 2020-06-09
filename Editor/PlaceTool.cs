#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Assets.UITB.Extensions;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  public class PlaceTool : EditorWindow {
    /// <summary>
    ///   Tool for placing and layout GameObjects depending on their environment.
    /// </summary>
    [MenuItem("Tools/3D Object/Place Tool", false, 10)]
    private static void CreateWindow() {
      // Get existing open window or if none, make a new one:
      var window = GetWindow(typeof(PlaceTool), false, "Place Tool");
      window.Show();
    }



    private readonly Vector3 _direction = Vector3.zero;



    private void OnGUI() {
      GUILayout.Label("Drop:", EditorStyles.boldLabel);
      GUILayout.BeginHorizontal();

      var dropInfos = GetDropInfos(_direction);
      if (GUILayout.Button("Drop")) {
        foreach (var dropInfo in dropInfos) {
          dropInfo.transform.position = dropInfo.newPosition;
        }
      }

      GUILayout.EndHorizontal();
    }



    private IEnumerable<(Transform transform, Vector3 newPosition, Vector3 origin, RaycastHit hit)> GetDropInfos(
      Vector3 direction) {
      foreach (var transform in Selection.transforms) {
        if (TryGetDropInfo(transform, direction, out var dropPosition, out var origin, out var hit)) {
          yield return (transform, dropPosition, origin, hit);
        }
      }
    }



    /// <summary>
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="direction">anchor point of the bounds to use as ray start</param>
    /// <returns></returns>
    private bool TryGetDropInfo(Transform transform,
                                Vector3 direction,
                                out Vector3 dropPosition,
                                out Vector3 origin,
                                out RaycastHit hit) {
      var gameObject = transform.gameObject;
      var savedLayer = gameObject.layer;
      gameObject.layer = 2; // ignore raycast

      // var bounds = transform.GetComponent<MeshRenderer>().bounds;
      var renderers = transform
        .GetComponentsInChildren<MeshRenderer>();
      if (!renderers.Any()) {
        dropPosition = default;
        origin = default;
        hit = default;
        return false;
      }

      var bounds = transform
                   .GetComponentsInChildren<MeshRenderer>()
                   .Select(render => render.bounds)
                   .Aggregate(
                     (b1, b2) => {
                       b1.Encapsulate(b2);
                       return b1;
                     }
                   );
      var scaledExtends = bounds.extents.ComponentWiseMultiply(direction);
      origin = bounds.center + scaledExtends;

      var dirNorm = direction != Vector3.zero ? direction : Vector3.down;
      var isHit = Physics.Raycast(origin, dirNorm, out hit);
      dropPosition = isHit
                       ? hit.point - bounds.extents.ComponentWiseMultiply(dirNorm)
                       : transform.position;

      gameObject.layer = savedLayer;
      return isHit;
    }



    private void OnDrawGizmos() {
      var dropInfos = GetDropInfos(_direction);
      Gizmos.color = new Color(1, 1, 0, .5f);
      foreach (var dropInfo in dropInfos) {
        Gizmos.DrawLine(dropInfo.origin, dropInfo.hit.point);
      }
    }
  }
}
#endif
