namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.Class)]
    public class ArchetypeNeedsReview : Attribute {
        public string Description;
        public ArchetypeNeedsReview(string description) => Description = description;
        public ArchetypeNeedsReview() => Description = "";
        }

    }