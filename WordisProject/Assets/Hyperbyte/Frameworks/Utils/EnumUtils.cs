// ©2019 - 2020 HYPERBYTE STUDIOS LLP
// All rights reserved
// Redistribution of this software is strictly not allowed.
// Copy of this software can be obtained from unity asset store only.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Hyperbyte.Utils
{
	/// <summary>
    /// Extention class Enum utils.
    /// </summary>
	public static class EnumUtils
	{
		// Returns all values of enum as IEnumerable.
    	public static IEnumerable<T> GetValues<T>() {
        	return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    	}

		// Returns all values of enum as List.
		public static List<T> GetValuesAsList<T>() {
        	return Enum.GetValues(typeof(T)).Cast<T>().ToList();
    	}	

		/// <summary>
		/// Returns string array of all elements of enum.
		/// </summary>
		public static string[] GetValuesAsStringArray<T>() {
        	List<T> values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

			string[] allElements = new string[values.Count];
			int index = 0;

			foreach (T t in values) {
				allElements[index] = t.ToString();
				index++;
			}	
			return allElements;
    	}	
	}
}
