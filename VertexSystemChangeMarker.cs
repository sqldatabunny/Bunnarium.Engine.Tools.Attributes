namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
    [Remove]
    public class VertexSystemChangeMarker : Attribute {
        public string Description;
        public VertexSystemChangeMarker(string description) => Description = description;
        public VertexSystemChangeMarker() => Description = string.Empty;
        }

    }