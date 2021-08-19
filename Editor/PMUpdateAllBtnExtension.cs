using UnityEditor.PackageManager;
using UnityEditor.PackageManager.UI;
using UnityEngine.UIElements;



internal class PMUpdateAllBtnExtension : IPackageManagerExtension {
  public VisualElement CreateExtensionUI() {
    var btn = new Button(PackageManagerUpdateAllUtility.StartUpdateAll);
    btn.text = btn.name = "Update All Packages (Experimental)";
    return btn;
  }

  public void OnPackageSelectionChange(PackageInfo packageInfo) {

  }

  public void OnPackageAddedOrUpdated(PackageInfo packageInfo) {

  }

  public void OnPackageRemoved(PackageInfo packageInfo) {

  }
}
