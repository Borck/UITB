using System;
using TMPro;
using UnityEditor;
using UnityEngine;



[RequireComponent(typeof(TextMeshProUGUI))]
[ExecuteInEditMode]
public class ClockDigital : MonoBehaviour {
  private TextMeshPro _textClock;



  private void Awake() {
    _textClock = GetComponent<TextMeshPro>();
    EditorApplication.update += Update;
  }



  private void Update() {
    var time = DateTime.Now;
    var hour = LeadingZero(time.Hour);
    var minute = LeadingZero(time.Minute);
    var second = LeadingZero(time.Second);
    _textClock.text = hour + ":" + minute + ":" + second;
  }



  private string LeadingZero(int n) => n.ToString().PadLeft(2, '0');
}
