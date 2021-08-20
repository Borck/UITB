namespace Assets.UITB.Common {
  public class FieldChange {
    public readonly object newValue;
    public readonly object oldValue;

    public readonly string relativePath;



    public FieldChange(string relativePath, object newValue, object oldValue) {
      this.relativePath = relativePath;
      this.newValue = newValue;
      this.oldValue = oldValue;
    }
  }
}
