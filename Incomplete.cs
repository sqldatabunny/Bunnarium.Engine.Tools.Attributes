namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class Incomplete : Attribute {
        public string Description;
        public Incomplete(string description) => Description = description;
        public Incomplete() => Description = "";
        }

    }