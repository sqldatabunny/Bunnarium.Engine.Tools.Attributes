namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Migrate : Attribute {
		public string Description;
		public Migrate(string description) => Description = description;
		public Migrate() => Description = "";
		}
	}