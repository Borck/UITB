using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;



namespace Assets.UITB.Common {
  public class RecordedFieldsManager {
    private readonly IDictionary<string, RecordedFieldHandle> _recorders;
    public readonly object container;



    public RecordedFieldsManager(object obj, InitializationMode addMode = InitializationMode.AllAttributedField) {
      container = obj;
      _recorders = GetInitialFieldsToRecord(obj, addMode)
        .ToDictionary(
          field => field.Name,
          field => new RecordedFieldHandle(obj, field.Name)
        );
    }



    private static IEnumerable<FieldInfo> GetInitialFieldsToRecord(object obj, InitializationMode addMode) {
      switch (addMode) {
        case InitializationMode.NoFields:
          return Enumerable.Empty<FieldInfo>();
        case InitializationMode.AllAttributedField:
          return obj
                 .GetType()
                 .GetFields()
                 .Where(field => field.GetCustomAttributes<RecordedFieldAttribute>(false).Any());
        case InitializationMode.AllFields:
          return obj
                 .GetType()
                 .GetFields();
        default:
          throw new ArgumentOutOfRangeException(nameof(addMode), addMode, null);
      }
    }



    public void AddRecorder(string relativePath) {
      _recorders.Add(relativePath, new RecordedFieldHandle(container, relativePath));
    }



    public RecordedFieldHandle GetRecorder(string relativePath) {
      return _recorders[relativePath];
    }



    public void RemoveRecorder(string relativePath) {
      _recorders.Remove(relativePath);
    }



    public bool ContainsRecorder(string relativePath) {
      return _recorders.ContainsKey(relativePath);
    }



    public void RecordAll() {
      foreach (var recorder in _recorders.Values) {
        recorder.Record();
      }
    }



    public IDictionary<string, FieldChange> GetChangesAsDictionary()
      => GetChanges()
        .ToDictionary(changeRecord => changeRecord.relativePath);



    public IEnumerable<FieldChange> GetChanges() {
      foreach (var recorder in _recorders.Values) {
        if (recorder.ValidateValueChanged(out var newValue, out var oldValue)) {
          yield return new FieldChange(recorder.relativePath, newValue, oldValue);
        }
      }
    }



    public enum InitializationMode {
      NoFields,

      /// <summary>
      ///   All fields marked with <see cref="RecordedFieldAttribute" />
      /// </summary>
      AllAttributedField,
      AllFields
    }
  }
}
