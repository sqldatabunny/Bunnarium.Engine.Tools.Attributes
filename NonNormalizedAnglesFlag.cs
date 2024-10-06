namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class NonNormalizedAnglesFlag : Attribute {
        public string Description;
        public NonNormalizedAnglesFlag(string description) => Description = description;
        public NonNormalizedAnglesFlag() => Description = "";
        }
    }