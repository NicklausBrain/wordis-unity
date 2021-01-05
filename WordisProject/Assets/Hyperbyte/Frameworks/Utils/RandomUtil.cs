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

using System.Collections.Generic;
using UnityEngine;

namespace Hyperbyte.Utils
{
	/// <summary>
    /// Extention class generating random nummbers.
    /// </summary>
	public class RandomUtil : MonoBehaviour {

		// Returns unique random numbers from the given range.
		public static List<int> GetNonRepeatingRandomNumbers(int startRange, int endRange, int noOfRandoms) {
			List<int> randomsList = new List<int>();
			for(int index = startRange; index < endRange; index++) {
				randomsList.Add(index);
			}
			randomsList.Shuffle();
			return randomsList.GetRange(0,noOfRandoms);
		}
	}
}

