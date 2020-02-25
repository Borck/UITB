namespace Assets.UITB.Common {
  public class FieldChange {

    public readonly string relativePath;
    public readonly object newValue;
    public readonly object oldValue;
    public FieldChange(string relativePath, object newValue, object oldValue) {
      this.relativePath = relativePath;
      this.newValue = newValue;
      this.oldValue = oldValue;
    }
  }
}
