namespace Bunnarium.Engine.Tools.Attributes {

    [AttributeUsage(AttributeTargets.All)]
	public class Citation : Attribute {
		public string Description;
		public Citation(string description) => Description = description;
		public Citation() => Description = string.Empty;
		}

    }