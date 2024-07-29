namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Deferred : Attribute {
		public string Description;
		public Deferred(string description) => Description = description;
		public Deferred() => Description = "";
		}

    }