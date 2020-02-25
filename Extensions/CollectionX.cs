using System;
using System.Collections.Generic;
using System.Linq;



namespace Assets.UITB.Extensions {
  public static class CollectionX {
    public static (IEnumerable<T> removed, IEnumerable<T> added) Delta<T>(
      this IEnumerable<T> enumerableAfter,
      IEnumerable<T> enumerableBefore) {
      var added = enumerableAfter
                    ?.Except( enumerableBefore ?? Enumerable.Empty<T>() ) ??
                  Enumerable.Empty<T>();
      var removed = enumerableBefore
                      ?.Except( enumerableAfter ?? Enumerable.Empty<T>() ) ??
                    Enumerable.Empty<T>();

      return (removed, added);
    }



    public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                                        TKey key)
      where TValue : new() {
      if (!dict.TryGetValue( key, out var val )) {
        val = new TValue();
        dict.Add( key, val );
      }

      return val;
    }



    public static TValue GetOrCreateValue<TKey, TValue>(this IDictionary<TKey, TValue> dict,
                                                        TKey key,
                                                        Func<TValue> valueCtor) {
      if (!dict.TryGetValue( key, out var val )) {
        val = valueCtor();
        dict.Add( key, val );
      }

      return val;
    }
  }
}
