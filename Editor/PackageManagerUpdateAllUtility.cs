using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.UIElements;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;


//Thanks to https://github.com/mitay-walle/Unity-PackageManager-Update-All/
[InitializeOnLoad]
public static class PackageManagerUpdateAllUtility
{
  #region Constructors

  static PackageManagerUpdateAllUtility()
  {
    PackageManagerExtensions.RegisterExtension(new PmUpdateAllBtnExtension());
  }

  #endregion

  internal class PmUpdateAllBtnExtension : IPackageManagerExtension
  {
    public VisualElement CreateExtensionUI()
    {
      var cntnr1 = new GroupBox();
      var cntnr2 = new GroupBox();
      var cntnr = new Box();

      void SetVisible(bool confirm)
      {
        cntnr.Clear();
        cntnr.Add(confirm ? cntnr2 : cntnr1);
      }

      var btnUpdate = new Button(() => SetVisible(true)) { text = "Update All Packages" };

      var btnOK = new Button(() =>
      {
        StartUpdateAll();
        SetVisible(false);
      }) { text = "OK" };
      var btnCancel = new Button(() => SetVisible(false)) { text = "Cancel" };

      cntnr1.Add(btnUpdate);
      cntnr2.Add(new TextElement { text = "Are you sure you want to update all packages?" });
      cntnr2.Add(btnOK);
      cntnr2.Add(btnCancel);
      cntnr.Add(cntnr1);
      return cntnr;
    }

    public void OnPackageSelectionChange(PackageInfo packageInfo)
    {
    }

    public void OnPackageAddedOrUpdated(PackageInfo packageInfo)
    {
    }

    public void OnPackageRemoved(PackageInfo packageInfo)
    {
    }
  }


  #region Static Members

  private const bool FullLogs = false;

  private static ListRequest _lRequest;
  private static AddRequest _aRequest;
  private static readonly Queue<string> ScheduledPackageUpdates = new Queue<string>();

  #endregion

  #region Methods and others

  [MenuItem("Help/Update All Packages (Experimental)")]
  public static void StartUpdateAll()
  {
    EditorPrefs.SetBool("UpdatePackagesInProgress", true);
    ContinueUpdateAllpackages();
  }


  public static void ContinueUpdateAllpackages()
  {
    _lRequest = Client.List(); // List packages installed for the Project

    EditorApplication.update += ProgressRequestPackageList;
  }


  private static void ForceStop()
  {
    Debug.LogError("something went wrong!");
    EditorApplication.update -= ProgressRequestPackageList;
    EditorPrefs.SetBool("UpdatePackagesInProgress", true);


    EditorPrefs.SetInt("UpdatePackagesCurrentCount", 0);
  }


  private static void ProgressRequestPackageList()
  {
    if (_lRequest == null)
    {
      ForceStop();
      return;
    }

    if (_lRequest.IsCompleted)
    {
      if (_lRequest.Status == StatusCode.Success)
        UpdateAll();
      else if (_lRequest.Status >= StatusCode.Failure) Debug.LogError(_lRequest.Error.message);

      EditorApplication.update -= ProgressRequestPackageList;
    }
  }


  private static void UpdateAll()
  {
    ScheduledPackageUpdates.Clear();
    foreach (var package in _lRequest.Result)
    {
      if (package.version == package.versions.latestCompatible ||
          package.version == package.versions.verified) //Debug.Log($"{package.name} up to date");
        continue;
      if (string.IsNullOrEmpty(package.versions.latestCompatible))
        //Latest version could not be evaluated, maybe on faulty or git packages
        //Debug.Log($"{package.name} up to date");
        continue;

      if (FullLogs)
        Debug.Log(
          $"'{package.name}' need update from: {package.version} to: {package.versions.latestCompatible} verified:{package.versions.verified}");

      ScheduledPackageUpdates.Enqueue(package.name);
    }

    if (ScheduledPackageUpdates.Count > 0)
    {
      Debug.Log($"need to Update {ScheduledPackageUpdates.Count} packages");
      CheckUpdateRequests();
      EditorApplication.update += CheckUpdateRequests;
    }
    else
    {
      Debug.Log("all packages are up to date!");
    }
  }


  private static void CheckUpdateRequests()
  {
    if (_aRequest == null)
    {
      ScheduleNextPackageUpdate();
      return;
    }

    if (!_aRequest.IsCompleted) return;

    //process update result
    if (FullLogs) Debug.Log($"process update result: {_aRequest.Result.name}");

    if (_aRequest.Status == StatusCode.Success)
      Debug.Log($"{_aRequest.Result.name} updated to {_aRequest.Result.version}");

    if (_aRequest.Status >= StatusCode.Failure) Debug.LogError(_aRequest.Error.message);


    if (ScheduledPackageUpdates.Count > 0)
    {
      ScheduleNextPackageUpdate();
    }
    else
    {
      // finish
      Debug.Log("finish updating all packages");
      EditorApplication.update -= CheckUpdateRequests;
      EditorPrefs.SetBool("UpdatePackagesInProgress", false);
    }
  }

  private static void ScheduleNextPackageUpdate()
  {
    var packageToUpdate = ScheduledPackageUpdates.Dequeue();
    if (FullLogs) Debug.Log($"update: {packageToUpdate}");
    _aRequest = Client.Add(packageToUpdate);
  }

  #endregion
}
