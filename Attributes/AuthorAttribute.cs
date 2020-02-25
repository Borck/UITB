using System;



namespace Assets.UITB.Attributes {
  /// <summary>
  /// Represents the author attribute.
  /// </summary>
  public class AuthorAttribute : Attribute {
    /// <summary>
    /// Contains the name.
    /// </summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    /// <summary>
    /// Contains the email.
    /// </Summary>
    /// <value>The email.</value>
    public string EMail { get; set; }

    /// <summary>
    /// Contains the url.
    /// </Summary>
    /// <value>The email.</value>
    public string URL { get; set; }



    /// <summary>
    /// Initializes a new instance of the
    /// AuthorAttribute type.
    /// </summary>
    /// <param name="name">The name.</param>
    public AuthorAttribute(string name) {
      Name = name;
    }



    /// <summary>
    /// Initializes a new instance of the
    /// AuthorAttribute type.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="email">The E-Mail</param>
    public AuthorAttribute(string name, string email) {
      Name = name;
      EMail = email;
    }



    /// <summary>
    /// Initializes a new instance of the
    /// AuthorAttribute type.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="email">The E-Mail</param>
    /// <param name="url">The Url</param>
    public AuthorAttribute(string name, string email, string url) {
      Name = name;
      EMail = email;
      URL = url;
    }


  }
}
