namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Optimize : Attribute {
		public string Description;
		public Optimize(string description) => Description = description;
		public Optimize() => Description = "";
		}


    }