#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Assets.UITB.Extensions;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  public class DropObjectEditor : EditorWindow {


    /// <summary>
    /// https://forum.unity.com/threads/here-is-an-editor-script-to-help-place-objects-on-ground.38186/
    /// </summary>
    [MenuItem( "GameObject/Layout/Drop Object", false, 10 )]
    private static void Init() {
      // Get existing open window or if none, make a new one:
      var window = GetWindow( typeof( DropObjectEditor ) );
      window.Show();
    }



    void OnGUI() {
      GUILayout.Label( "Drop Using:", EditorStyles.boldLabel );
      GUILayout.BeginHorizontal();

      if (GUILayout.Button( "Bottom" )) {
        DropObjects( BoundsAlign.Min );
      }

      if (GUILayout.Button( "Center" )) {
        DropObjects( BoundsAlign.Center );
      }

      GUILayout.EndHorizontal();
    }



    void DropObjects(BoundsAlign align) {
      DropObjects( align,
                   Selection
                         .transforms
                         .Select( transform => transform.gameObject )
                         .Where( gameObject => gameObject != default ) );
    }



    public static void DropObjects(BoundsAlign align, IEnumerable<UnityEngine.GameObject> gameObjects) {
      // drop multi-selected objects using the right method
      foreach (var go in gameObjects) {

        // override layer so it doesn't hit itself
        var savedLayer = go.layer;
        go.layer = 2; // ignore raycast


        var bounds = go.EncapsulateColliderBounds();

        var boundsAlignPos = bounds.GetAlignedPosition( BoundsAlign.Center, align, BoundsAlign.Center );
        // see if this ray hit something
        if (Physics.Raycast( boundsAlignPos, -Vector3.up, out var hit )) {

          var pivotOffset = bounds.center - go.transform.position;
          go.transform.position = hit.point + boundsAlignPos - pivotOffset;
        }

        // restore layer
        go.layer = savedLayer;
      }
    }



    public enum Positioning {
      Bottom,
      Origin,
      Center
    }
  }
}
#endif
