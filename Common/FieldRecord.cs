using System;
using System.Linq;



namespace Assets.UITB.Common {

  public sealed class FieldRecord {
    private readonly object _o;

    private readonly string[] _relativePathTokens;
    public string relativePath => string.Join( ".", _relativePathTokens );


    public object value { get; private set; }

    public FieldRecord(object o, string relativePath) {
      _o = o;
      _relativePathTokens = relativePath.Split( '.' );
    }



    public bool ValidateValueChanged(out object newValue, out object oldValue) {
      newValue = GetCurrentValue();
      oldValue = value;
      return !Equals( newValue, oldValue );
    }



    public object Record() {
      return value = GetCurrentValue();
    }


    public object GetCurrentValue() {
      return _relativePathTokens.Aggregate(
        _o,
        GetFieldOrPropertyValue
      );
    }



    private static object GetFieldOrPropertyValue(object obj, string propertyName) {
      var type = obj.GetType();
      var fieldInfo = type.GetField( propertyName );
      if (fieldInfo != default) {
        return fieldInfo.GetValue( obj );
      }

      var propertyInfo = type.GetProperty( propertyName );
      if (propertyInfo != default) {
        return propertyInfo.GetValue( obj );
      }
      throw new InvalidOperationException( $"Property not found: {obj.GetType()}.{propertyName}" );

    }
  }
}
