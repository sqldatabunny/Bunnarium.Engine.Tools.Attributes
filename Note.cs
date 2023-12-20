namespace Bunnarium.Engine.Tools.Attributes {

    [AttributeUsage(AttributeTargets.All)]
	public class Note : Attribute {
		public string Description;
		public Note(string description) => Description = description;
		public Note() => Description = "";
		}
	}