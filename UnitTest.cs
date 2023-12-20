namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class UnitTest : Attribute {
		public string Description;
		public UnitTest(string description) => Description = description;
		public UnitTest() => Description = "";
		}
	}