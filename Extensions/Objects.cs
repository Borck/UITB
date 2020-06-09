namespace Assets.UITB.Extensions {
  public static class Objects {
    /// <summary>
    ///   Swapping the values of two references
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    public static void Swap<T>(ref T lhs, ref T rhs) {
      var temp = lhs;
      lhs = rhs;
      rhs = temp;
    }
  }
}
