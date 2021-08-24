using System;
using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif



[ExecuteInEditMode]
public class ClockDigital : MonoBehaviour {
  #region Members

  [SerializeField]
  private TimeTextEvent tick;

  #endregion

  #region Methods and others

#if UNITY_EDITOR
  private void Awake() => EditorApplication.update += Update;
#endif



  private void Update() {
    var time = DateTime.Now;
    var hour = LeadingZero(time.Hour);
    var minute = LeadingZero(time.Minute);
    var second = LeadingZero(time.Second);
    tick.Invoke(hour + ":" + minute + ":" + second);
  }



  private string LeadingZero(int n) => n.ToString().PadLeft(2, '0');

  #endregion

  #region Nested Types

  [Serializable]
  public class TimeTextEvent : UnityEvent<string> { }

  #endregion
}

