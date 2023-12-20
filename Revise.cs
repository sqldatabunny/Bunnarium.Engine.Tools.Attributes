namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Revise : Attribute {
		public string Description;
		public Revise(string description) => Description = description;
		public Revise() => Description = "";
		}
	}