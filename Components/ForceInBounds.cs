using System;
using Assets.UITB.Extensions;
using UnityEngine;



public class ForceInBounds : MonoBehaviour {
  public Collider Bounds;

  private Vector3 _startPos;
  private Quaternion _startRot;
  private Vector3 _startSca;



  // Start is called before the first frame update
  private void Start() {
    if (!IsInBounds()) {
      enabled = false;
      Console.Error.WriteLine("GameObject must be initially inside bounds to get the reset position");
    }
  }



  public void UpdateStartTransform() => (_startPos, _startRot, _startSca) = transform.GetTRS();



  private void LateUpdate() {
    if (!IsInBounds()) {
      ResetTransform();
    }
  }



  private bool IsInBounds() => Bounds.bounds.Contains(transform.position);

  private void ResetTransform() => transform.SetTRS(_startPos, _startRot, _startSca);
}
