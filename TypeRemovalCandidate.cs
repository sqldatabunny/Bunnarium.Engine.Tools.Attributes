namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
    public class TypeRemovalCandidate : Attribute {
        public string Description;
        public TypeRemovalCandidate(string description) => Description = description;
        public TypeRemovalCandidate() => Description = "";
        }
    }