// display selected gameobject mesh stats (should work on prefabs,models in project window also)
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  /// <summary>
  ///   https://github.com/UnityCommunity/UnityLibrary/blob/master/Assets/Scripts/Editor/Tools/GetSelectedMeshInfo.cs
  /// </summary>
  public class SelectedMeshInfo : EditorWindow {
    [MenuItem( "Tools/Documentation/Mesh Info", priority = 1000 )]
    public static void ShowWindow() {
      var window = GetWindow( typeof( SelectedMeshInfo ) );
      window.titleContent = new GUIContent( "Mesh Info" );
    }



    private void OnGUI() {
      var selection = Selection.activeGameObject;

      if (!selection) {
        return;
      }

      var totalMeshes = 0;
      var totalVertices = 0;
      var totalTriangles = 0;

      // get all meshes
      var meshes = selection.GetComponentsInChildren<MeshFilter>();
      for (int i = 0,
               length = meshes.Length;
           i < length;
           i++) {
        totalVertices += meshes[i].sharedMesh.vertexCount;
        totalTriangles += meshes[i].sharedMesh.triangles.Length;
        totalMeshes++;
      }

      // display stats
      EditorGUILayout.SelectableLabel(
        $"Selected: {selection.name}\nMeshes: {totalMeshes}\nVertices: {totalVertices}\nTriangles: {totalTriangles}",
        new GUIStyle() { clipping = TextClipping.Overflow }
      );
    }



    void OnSelectionChange() {
      // force redraw window
      Repaint();
    }
  }
}
#endif
