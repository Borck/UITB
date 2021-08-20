using System;
using UnityEngine.UIElements;



namespace Assets.UITB.Common {
  public class DependencyValue<T> {
    private T _value;



    public DependencyValue() => _value = default;



    public DependencyValue(T initialValue) => _value = initialValue;



    public T value {
      get => _value;
      set => SetValueAndNotify(value);
    }


    public event EventHandler<ChangeEvent<T>> ValueUpdated;
    public event EventHandler<ChangeEvent<T>> ValueChanged;



    public void SetValueAndNotify(T newValue) {
      var changed = !Equals(_value, newValue);
      var e = new DependencyValueChangeEvent(_value, newValue);
      _value = newValue;
      ValueUpdated?.Invoke(this, e);
      if (changed) {
        ValueChanged?.Invoke(this, e);
      }
    }



    public void SetValueWithoutNotify(T newValue) => _value = newValue;



    private class DependencyValueChangeEvent : ChangeEvent<T> {
      public DependencyValueChangeEvent(T previousValue, T newValue) {
        this.previousValue = previousValue;
        this.newValue = newValue;
      }
    }
  }
}
