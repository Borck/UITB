#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;



namespace Assets.UITB.Inspector {
  [CustomPropertyDrawer( typeof( ShowPropertyAttribute ) )]
  sealed class ShowPropertyAttributeDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      var showPropertryAttr = (ShowPropertyAttribute)base.attribute;

      EditorGUI.BeginChangeCheck();
      EditorGUI.PropertyField( position, property, label );
      if (EditorGUI.EndChangeCheck()) {
        showPropertryAttr.dirty = true;
      } else if (showPropertryAttr.dirty) {
        var name = property.name;
        var parent = GetParentObject( property.propertyPath, property.serializedObject.targetObject );

        var type = parent.GetType();
        var info = type.GetProperty( name );

        if (info == null)
          Debug.LogError( "Invalid property name \"" + name + "\"" );
        else
          info.SetValue( parent, fieldInfo.GetValue( parent ), null );

        showPropertryAttr.dirty = false;
      }
    }



    public static object GetParentObject(string path, object obj) {
      var fields = path.Split( '.' );

      if (fields.Length == 1)
        return obj;

      var info = obj.GetType()
                    .GetField( fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
      obj = info.GetValue( obj );

      return GetParentObject( string.Join( ".", fields, 1, fields.Length - 1 ), obj );
    }
  }
}
#endif
