using System.Collections.Generic;



namespace Assets.UITB.Extensions {
  public static class ListX {
    public static void AddRange<T>(this List<T> list, params T[] items)
      => list.AddRange(items);
  }
}
