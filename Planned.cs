using System;
namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Planned : Attribute {
		public string Description;
		public Planned(string description) => Description = description;
		public Planned() => Description = String.Empty;
		}
	}