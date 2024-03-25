namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class SIMDCandidate : Attribute {
        public string Description;
        public SIMDCandidate(string description) => Description = description;
        public SIMDCandidate() => Description = "";
        }


    }