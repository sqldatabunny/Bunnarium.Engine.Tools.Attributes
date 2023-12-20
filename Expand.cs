namespace Bunnarium.Engine.Tools.Attributes {
    ///  <summary> Means to manually inline the code from the functions within this method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
	public class Expand : Attribute {
		public string Description;
		public Expand(string description) => Description = description;
		public Expand() => Description = "";
		}
	}