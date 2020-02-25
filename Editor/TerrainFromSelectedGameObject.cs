#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  /// <summary>
  ///   https://lmhpoly.com/convert-mesh-to-unity-terrain/
  /// </summary>
  public class TerrainFromSelectedGameObject : EditorWindow {
    [MenuItem( "GameObject/3D Object/Terrain from Selection", false, 10000 )]
    static void OpenWindow() {
      GetWindow<TerrainFromSelectedGameObject>( true );
    }



    private int _resolution = 512;
    private Vector3 _addTerrain;
    int _bottomTopRadioSelected;
    private static readonly string[] BottomTopRadio = { "Bottom Up", "Top Down" };
    private float _shiftHeight;



    void OnGUI() {
      _resolution = EditorGUILayout.IntField( "Resolution", _resolution );
      _addTerrain = EditorGUILayout.Vector3Field( "Add terrain", _addTerrain );
      _shiftHeight = EditorGUILayout.Slider( "Shift height", _shiftHeight, -1f, 1f );
      _bottomTopRadioSelected = GUILayout.SelectionGrid(
        _bottomTopRadioSelected,
        BottomTopRadio,
        BottomTopRadio.Length,
        EditorStyles.radioButton
      );

      if (GUILayout.Button( "Create Terrain" )) {
        if (Selection.activeGameObject == null) {
          EditorUtility.DisplayDialog( "No object selected", "Please select an object.", "Ok" );
        } else {
          CreateTerrain();
        }
      }
    }



    delegate void CleanUp();



    void CreateTerrain() {
      //fire up the progress bar
      ShowProgressBar( 1, 100 );

      var terrain = new TerrainData { heightmapResolution = _resolution };
      var terrainObject = Terrain.CreateTerrainGameObject( terrain );

      Undo.RegisterCreatedObjectUndo( terrainObject, "Object to Terrain" );

      var collider = Selection.activeGameObject.GetComponent<MeshCollider>();
      CleanUp cleanUp = null;

      //Add a collider to our source object if it does not exist.
      //Otherwise raycasting doesn't work.
      if (!collider) {
        collider = Selection.activeGameObject.AddComponent<MeshCollider>();
        cleanUp = () => DestroyImmediate( collider );
      }

      var bounds = collider.bounds;
      var sizeFactor = collider.bounds.size.y / ( collider.bounds.size.y + _addTerrain.y );
      terrain.size = collider.bounds.size + _addTerrain;
      bounds.size = new Vector3( terrain.size.x, collider.bounds.size.y, terrain.size.z );

      // Do raycasting samples over the object to see what terrain heights should be
      var heights = new float[terrain.heightmapResolution, terrain.heightmapResolution];
      var ray = new Ray( new Vector3( bounds.min.x, bounds.max.y + bounds.size.y, bounds.min.z ), -Vector3.up );
      var hit = new RaycastHit();
      var meshHeightInverse = 1 / bounds.size.y;
      var rayOrigin = ray.origin;

      var maxHeight = heights.GetLength( 0 );
      var maxLength = heights.GetLength( 1 );

      var stepXz = new Vector2( bounds.size.x / maxLength, bounds.size.z / maxHeight );

      for (var zCount = 0; zCount < maxHeight; zCount++) {
        ShowProgressBar( zCount, maxHeight );

        for (var xCount = 0; xCount < maxLength; xCount++) {
          var height = 0.0f;

          if (collider.Raycast( ray, out hit, bounds.size.y * 3 )) {
            height = ( hit.point.y - bounds.min.y ) * meshHeightInverse;
            height += _shiftHeight;

            //bottom up
            if (_bottomTopRadioSelected == 0) {
              height *= sizeFactor;
            }

            //clamp
            if (height < 0) {
              height = 0;
            }
          }

          heights[zCount, xCount] = height;
          rayOrigin.x += stepXz[0];
          ray.origin = rayOrigin;
        }

        rayOrigin.z += stepXz[1];
        rayOrigin.x = bounds.min.x;
        ray.origin = rayOrigin;
      }

      terrain.SetHeights( 0, 0, heights );

      EditorUtility.ClearProgressBar();

      cleanUp?.Invoke();
    }



    void ShowProgressBar(float progress, float maxProgress) {
      var p = progress / maxProgress;
      EditorUtility.DisplayProgressBar( "Creating Terrain...", Mathf.RoundToInt( p * 100f ) + " %", p );
    }
  }
}
#endif
