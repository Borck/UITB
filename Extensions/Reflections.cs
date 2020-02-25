using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class Reflections {
    // Gets value from SerializedProperty - even if value is nested
#if UNITY_EDITOR
    public static object GetValue(this UnityEditor.SerializedProperty property) {
      return GetValue(
        property.serializedObject.targetObject,
        property.propertyPath
      );
    }
#endif
    public static object GetValue(Object @object, string path) {
      object obj = @object;

      foreach (var pathToken in path.Split( '.' )) {
        var type = obj.GetType();
        var field = type.GetField( pathToken );
        obj = field.GetValue( obj );
      }

      return obj;
    }



    // Sets value from SerializedProperty - even if value is nested
#if UNITY_EDITOR
    public static void SetValue(this UnityEditor.SerializedProperty property, object val) {
      object obj = property.serializedObject.targetObject;

      var list = new List<KeyValuePair<FieldInfo, object>>();

      foreach (var path in property.propertyPath.Split( '.' )) {
        var type = obj.GetType();
        var field = type.GetField( path );
        list.Add( new KeyValuePair<FieldInfo, object>( field, obj ) );
        obj = field.GetValue( obj );
      }

      // Now set values of all objects, from child to parent
      for (var i = list.Count - 1; i >= 0; --i) {
        list[i].Key.SetValue( list[i].Value, val );
        // New 'val' object will be parent of current 'val' object
        val = list[i].Value;
      }
    }
#endif
  }
}
