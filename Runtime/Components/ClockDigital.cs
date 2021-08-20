using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;



[ExecuteInEditMode]
public class ClockDigital : MonoBehaviour {

  [System.Serializable]
  public class TimeTextEvent : UnityEvent<string> { }
  [SerializeField] private TimeTextEvent tick;



  private void Awake() {
    EditorApplication.update += Update;
  }



  private void Update() {
    var time = DateTime.Now;
    var hour = LeadingZero(time.Hour);
    var minute = LeadingZero(time.Minute);
    var second = LeadingZero(time.Second);
    tick.Invoke(hour + ":" + minute + ":" + second);
  }



  private string LeadingZero(int n) => n.ToString().PadLeft(2, '0');
}
