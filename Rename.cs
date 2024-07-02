using System;
namespace Bunnarium.Engine.Tools.Attributes {
    [AttributeUsage(AttributeTargets.All)]
	public class Rename : Attribute {
		public string Description;
		public Rename(string description) => Description = description;
		public Rename() => Description = "";

		}
	}