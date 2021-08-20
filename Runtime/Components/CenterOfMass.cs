using UnityEngine;



[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour {
  #region Members

  public Vector3 Center;
  private Rigidbody _rigidbody;

  #endregion

  #region Methods and others

  private void Start() => _rigidbody = GetComponent<Rigidbody>();



  private void Update() {
    _rigidbody.centerOfMass = Center;
    _rigidbody.WakeUp();
  }



  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(transform.position + (transform.rotation * Center), .1f);
  }

  #endregion
}
