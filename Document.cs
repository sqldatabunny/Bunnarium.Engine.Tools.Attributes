namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Document : Attribute {
		public string Description;
		public Document(string description) => Description = description;
		public Document() => Description = "";
		}
	}