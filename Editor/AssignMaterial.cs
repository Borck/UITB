#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  public class AssignMaterial : ScriptableWizard {
    public Material materialToApply;



    void OnWizardUpdate() {
      helpString = "Select Game Objects";
      isValid = ( materialToApply != null );
    }



    void OnWizardCreate() {
      var gos = Selection.gameObjects;
      foreach (var go in gos) {
        var materials = go.GetComponent<Renderer>().sharedMaterials;
        for (var i = 0; i < materials.Length; i++)
          materials[i] = materialToApply;
        go.GetComponent<Renderer>().sharedMaterials = materials;

        materials = go.GetComponent<Renderer>().materials;
        for (var i = 0; i < materials.Length; i++)
          materials[i] = materialToApply;
        go.GetComponent<Renderer>().materials = materials;
      }
    }



    [MenuItem( "GameObject/Material/Assign", false, 4 )]
    public static void CreateWindow() {
      DisplayWizard( "Assign Material", typeof( AssignMaterial ), "Assign" );
    }
  }
}
#endif
