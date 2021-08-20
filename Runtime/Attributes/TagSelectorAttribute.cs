using UnityEngine;



namespace Assets.UITB.Inspector {
  /// <summary>
  ///   Using a this attribute to visualize our string as a drop-down popup that lists all the available tags.
  /// </summary>
  public class TagSelectorAttribute : PropertyAttribute {
    public bool UseDefaultTagFieldDrawer = false;
  }
}
