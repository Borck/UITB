using System;
using System.Collections.Generic;
using System.Linq;



namespace Assets.UITB.Extensions {
  public static class CollectionX {
    public static (IEnumerable<T> removed, IEnumerable<T> added) Delta<T>(
      this IEnumerable<T> enumerableAfter,
      IEnumerable<T> enumerableBefore) {
      var added = enumerableAfter
                    ?.Except(enumerableBefore ?? Enumerable.Empty<T>()) ??
                  Enumerable.Empty<T>();
      var removed = enumerableBefore
                      ?.Except(enumerableAfter ?? Enumerable.Empty<T>()) ??
                    Enumerable.Empty<T>();

      return (removed, added);
    }



    public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
      foreach (var item in enumerable) {
        action(item);
      }
    }



    public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                                        TKey key)
      where TValue : new() {
      if (!dict.TryGetValue(key, out var val)) {
        val = new TValue();
        dict.Add(key, val);
      }

      return val;
    }



    public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                                        TKey key,
                                                        Func<TValue> valueCtor) {
      if (!dict.TryGetValue(key, out var val)) {
        val = valueCtor();
        dict.Add(key, val);
      }

      return val;
    }



    public static IEnumerable<(T left, T right)> AsTuples<T>(this IEnumerable<T> enumerable) {
      using (var etor = enumerable.GetEnumerator()) {
        while (etor.MoveNext()) {
          var left = etor.Current;
          yield return etor.MoveNext()
                         ? (left, etor.Current)
                         : throw new ArgumentException("Enumerable length is not times of two");
        }
      }
    }



    public static IEnumerable<(T left, T middle, T right)> AsTriples<T>(this IEnumerable<T> enumerable) {
      using (var etor = enumerable.GetEnumerator()) {
        while (etor.MoveNext()) {
          var left = etor.Current;
          if (!etor.MoveNext()) {
            throw new ArgumentException("Enumerable length is not times of three");
          }

          var middle = etor.Current;
          yield return etor.MoveNext()
                         ? (left, middle, etor.Current)
                         : throw new ArgumentException("Enumerable length is not times of three");
        }
      }
    }
  }
}
