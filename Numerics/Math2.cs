namespace Assets.UITB.Numerics {
  public static class Math2 {
    public static int Clamp(int value, int lowerInclusive, int upperExclusive) {
      return value < lowerInclusive
               ? lowerInclusive
               : value >= upperExclusive
                 ? upperExclusive - 1
                 : value;
    }
  }
}
