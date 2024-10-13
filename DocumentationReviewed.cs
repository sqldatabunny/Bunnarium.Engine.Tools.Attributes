namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class DocumentationReviewed : Attribute {
        public string Description;
        public DocumentationReviewed(string description) => Description = description;
        public DocumentationReviewed() => Description = "";
        }
    }