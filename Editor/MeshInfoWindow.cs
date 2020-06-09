#if UNITY_EDITOR

using System;
using System.Linq;
using Assets.UITB.Extensions;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  /// <summary>
  ///   Displays mesh information of the selected GameObject(s)
  /// </summary>
  public class MeshInfoWindow : EditorWindow {
    [MenuItem("Tools/Documentation/Mesh Info", priority = 1000)]
    public static void ShowWindow() {
      var window = GetWindow(typeof(MeshInfoWindow));
      window.titleContent = new GUIContent("Mesh Info");
    }



    private void OnGUI() {
      var transforms = Selection.transforms;

      var totalMeshes = 0;
      var totalVertices = 0;
      var totalTriangles = 0;

      // get all meshes
      var meshFilters = transforms.SelectMany(t => t.GetComponentsInChildren<MeshFilter>());
      foreach (var meshFilter in meshFilters) {
        totalVertices += meshFilter.sharedMesh.vertexCount;
        totalTriangles += meshFilter.sharedMesh.triangles.Length;
        totalMeshes++;
        // yield return null;
      }

      var worldMeshBounds = transforms.SelectMany(t => t.GetComponentsInChildren<MeshRenderer>())
                                      .Select(renderer => renderer.bounds)
                                      .Where(bounds => bounds.size != Vector3.zero)
                                      .EncapsulateAll();
      var localMeshBounds = meshFilters
                            .Select(filter => filter.sharedMesh.bounds)
                            .Where(bounds => bounds.size != Vector3.zero)
                            .EncapsulateAll();

      var meshToTransformOffsetWorld = transforms.Length == 1
                                         ? worldMeshBounds.center - transforms[0].position
                                         : new Vector3(float.NaN, float.NaN, float.NaN);
      var meshToTransformOffsetLocal = localMeshBounds.center;

      // display stats
      EditorGUILayout.SelectableLabel(
        $@"---------- Transform ---------------
{string.Join(Environment.NewLine, transforms.Select(t => t.ToString()))}


---------- Mesh ---------------
Bounds (world): {worldMeshBounds.ToString("F3")}
Bounds (local): {localMeshBounds.ToString("F3")}
Pivot offset (world): {meshToTransformOffsetWorld}
Pivot offset (local): {meshToTransformOffsetLocal}

Selected: {string.Join(",", transforms.Select(t => t.name))}
Meshes: {totalMeshes}
Vertices: {totalVertices}
Triangles: {totalTriangles}

",
        new GUIStyle {clipping = TextClipping.Overflow}
      );
    }



    private void OnSelectionChange() =>
      // force redraw window
      Repaint();
  }
}
#endif
