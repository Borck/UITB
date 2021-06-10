using Assets.UITB.Extensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Editor {
  public class CombineMeshesWizard : ScriptableWizard {
    #region Members

    [Tooltip("Root GameObject of meshes.")]
    public GameObject SourceRoot;

    [Tooltip("GameObject to save new combine mesh.")]
    public GameObject TargetObject;

    #endregion

    #region Methods and others

    [MenuItem("Window/UITB/3D Object/Combine Meshes")]
    private static void ShowEditor() => DisplayWizard("Combine Meshes", typeof(CombineMeshesWizard), "Combine");



    private void OnWizardUpdate() => isValid = SourceRoot && TargetObject;



    private void OnWizardCreate() {
      var meshFilters = SourceRoot.GetComponentsInChildren<MeshFilter>();
      var combines = new CombineInstance[meshFilters.Length];
      var materialList = new List<Material>();
      for (var i = 0; i < meshFilters.Length; i++) {
        combines[i].mesh = meshFilters[i].sharedMesh;
        combines[i].transform = Matrix4x4.TRS(
          meshFilters[i].transform.position - SourceRoot.transform.position,
          meshFilters[i].transform.rotation,
          meshFilters[i].transform.lossyScale
        );

        materialList.AddRange(meshFilters[i].GetComponent<MeshRenderer>().sharedMaterials);
      }

      var newMesh = new Mesh();
      newMesh.CombineMeshes(combines, false);

      TargetObject.GetOrAddComponent<MeshFilter>().sharedMesh = newMesh;
      TargetObject.GetOrAddComponent<MeshRenderer>().sharedMaterials = materialList.ToArray();
      TargetObject.GetOrAddComponent<MeshCollider>().sharedMesh = newMesh;

      Selection.activeObject = newMesh;
    }

    #endregion
  }
}
