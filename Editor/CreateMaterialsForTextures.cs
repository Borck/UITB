// CreateMaterialsForTextures.cs
// C#

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;



public class CreateMaterialsForTextures : Editor {
  #region Methods and others

#if UNITY_URP
  private const string DefaultShaderName = "Universal Render Pipeline/Lit";
#elif UNITY_2019_1_OR_NEWER
  private const string DefaultShaderName = "HDRP/Lit";
#else
  private const string DefaultShaderName = "Standard";
#endif



  [MenuItem("Assets/Create/Material From Selection", priority = 300)]
  private static void CreateMaterials() {
    try {
      AssetDatabase.StartAssetEditing();
      var textures = Selection.GetFiltered(typeof(Texture), SelectionMode.Assets).Cast<Texture>();
      foreach (var tex in textures) {
        var path = AssetDatabase.GetAssetPath(tex);
        path = path.Substring(0, path.LastIndexOf(".", StringComparison.Ordinal)) + ".mat";
        if (AssetDatabase.LoadAssetAtPath(path, typeof(Material)) != null) {
          Debug.LogWarning("Can't create material, it already exists: " + path);
          continue;
        }

        var mat = new Material(Shader.Find(DefaultShaderName)) {mainTexture = tex};
        AssetDatabase.CreateAsset(mat, path);
      }
    } finally {
      AssetDatabase.StopAssetEditing();
      AssetDatabase.SaveAssets();
    }
  }



  [MenuItem("Assets/Create/Material From Selection", true)]
  private static bool ValidateCreateMaterials() => Selection.activeObject is Texture2D;

  #endregion
}
