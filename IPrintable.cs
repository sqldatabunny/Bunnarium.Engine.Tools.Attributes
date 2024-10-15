namespace Bunnarium.Engine.Tools.Attributes {
    public interface IPrintable {

        ///  <returns> A formatted string summarizing this item's state.
        /// </returns>
        public string ToString(byte digits, int integerLength, int padToLength);

        }
    }
