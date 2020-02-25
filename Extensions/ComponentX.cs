using System;
using UnityEngine;



namespace Assets.UITB.Extensions {
  public static class ComponentX {
    public static Component CopyTo(this Component original, GameObject destination) {
      var destinationComp = destination.GetOrAddComponent( original.GetType() );
      original.CopyValuesTo( destinationComp );
      return destinationComp;
    }



    public static T CopyTo<T>(T original, GameObject destination) where T : Component {
      var destinationComp = destination.GetOrAddComponent<T>();
      original.CopyValuesTo( destinationComp );
      return destinationComp;
    }



    public static void CopyValuesTo(this Component original, Component destination) {
      var type = original.GetType();
      var fields = type.GetFields();
      foreach (var field in fields) {
        if (field.IsStatic)
          continue;
        field.SetValue( destination, field.GetValue( original ) );
      }
      var props = type.GetProperties();
      foreach (var prop in props) {
        if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") {
          continue;
        }
        var propValue = prop.IsDefined( typeof( ObsoleteAttribute ), true )
                          ? prop.GetValue( original, null )
                          : default;
        prop.SetValue( destination, propValue, null );
      }

    }

  }
}
