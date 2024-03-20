
namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class SemiAutoPropertyCandidate : Attribute {
        public string Description;
        public SemiAutoPropertyCandidate(string description) => Description = description;
        public SemiAutoPropertyCandidate() => Description = "";
        }
    }