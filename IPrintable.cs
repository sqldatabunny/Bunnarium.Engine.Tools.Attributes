using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunnarium.Engine.Tools.Attributes {
    public interface IPrintable {

        ///  <returns> A string summarizing this shape's properties.
        /// </returns>
        public string ToString(byte digits, int integerLength, int padToLength);

        }
    }
