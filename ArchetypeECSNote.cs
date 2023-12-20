namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    public class ArchetypeECSNote : Attribute {
        public string Description;
        public ArchetypeECSNote(string description) => Description = description;
        public ArchetypeECSNote() => Description = "";
        }


    }