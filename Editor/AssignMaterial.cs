#if UNITY_EDITOR
using Assets.UITB.Extensions;
using System.Linq;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  public class AssignMaterial : ScriptableWizard {
    #region Members

    public bool ApplyRecursive;
    public Material MaterialToApply;

    #endregion

    #region Methods and others

    private void OnWizardUpdate() {
      helpString = "Select Game Objects";
      isValid = MaterialToApply != null;
    }



    private void OnWizardCreate() {
      var gos = Selection.gameObjects;
      var renderers = ApplyRecursive
                        ? gos.SelectMany(go => go.GetComponentsInThisAndChildren<Renderer>())
                        : gos
                          .Select(go => go.TryGetComponent(out Renderer renderer) ? renderer : default)
                          .Where(renderer => renderer != default);

      foreach (var renderer in renderers) {
        var go = renderer.gameObject;
        var materials = renderer.sharedMaterials;
        for (var i = 0; i < materials.Length; i++) {
          materials[i] = MaterialToApply;
        }

        go.GetComponent<Renderer>().sharedMaterials = materials;

        materials = go.GetComponent<Renderer>().materials;
        for (var i = 0; i < materials.Length; i++) {
          materials[i] = MaterialToApply;
        }

        go.GetComponent<Renderer>().materials = materials;
      }
    }



    [MenuItem("Window/UITB/3D Object/Assign Material", false)]
    public static void CreateWindow() => DisplayWizard("Assign Material", typeof(AssignMaterial), "Assign");

    #endregion
  }
}
#endif
