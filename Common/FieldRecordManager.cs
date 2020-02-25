using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace Assets.UITB.Common {
  public class FieldRecordManager {
    public readonly object container;

    private readonly IDictionary<string, FieldRecord> _recorders;

    public FieldRecordManager(object obj, AddMode addMode = AddMode.Standard) {
      container = obj;
      _recorders = GetInitialFieldsToRecord( obj, addMode )
        .ToDictionary(
          field => field.Name,
          field => new FieldRecord( obj, field.Name ) );
    }



    private static IEnumerable<FieldInfo> GetInitialFieldsToRecord(object obj, AddMode addMode) {
      switch (addMode) {
        case AddMode.NoFields:
          return Enumerable.Empty<FieldInfo>();
        case AddMode.Standard:
          return obj
            .GetType()
            .GetFields()
            .Where( field => field.GetCustomAttributes<FieldRecordAttribute>( false ).Any() );
        case AddMode.AllFields:
          return obj
                 .GetType()
                 .GetFields();
        default:
          throw new ArgumentOutOfRangeException( nameof( addMode ), addMode, null );
      }
    }



    public void AddRecorder(string relativePath) {
      _recorders.Add( relativePath, new FieldRecord( container, relativePath ) );
    }



    public FieldRecord GetRecorder(string relativePath) {
      return _recorders[relativePath];
    }



    public void RemoveRecorder(string relativePath) {
      _recorders.Remove( relativePath );
    }



    public bool ContainsRecorder(string relativePath) {
      return _recorders.ContainsKey( relativePath );
    }



    public void RecordAll() {
      foreach (var recorder in _recorders.Values) {
        recorder.Record();
      }
    }



    public IDictionary<string, FieldChange> GetChangesAsDictionary()
      => GetChanges()
        .ToDictionary( changeRecord => changeRecord.relativePath );



    public IEnumerable<FieldChange> GetChanges() {
      foreach (var recorder in _recorders.Values) {
        if (recorder.ValidateValueChanged( out var newValue, out var oldValue )) {
          yield return new FieldChange( recorder.relativePath, newValue, oldValue );
        }
      }
    }







    public enum AddMode {
      NoFields,

      /// <summary>
      /// All fields marked with <see cref="FieldRecordAttribute"/>
      /// </summary>
      Standard,
      AllFields
    }
  }
}
