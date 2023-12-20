namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Remove : Attribute {
		public string Description;
		public Remove(string description) => Description = description;
		public Remove() => Description = "";
		}
    }