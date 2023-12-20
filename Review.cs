namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Review : Attribute {
		public string Description;
		public Review(string description) => Description = description;
		public Review() => Description = "";
		}
	}