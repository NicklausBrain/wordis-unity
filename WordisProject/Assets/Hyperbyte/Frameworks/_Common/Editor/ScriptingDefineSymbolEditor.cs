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
using UnityEditor;
using System.Linq;

namespace Hyperbyte
{
    public static class ScriptingDefineSymbolEditor
    {
        #region Add Symbols
        public static void AddScriptingDefineSymbol(string defineSymbol)
        {
            AddSymbolForTarget(BuildTargetGroup.Android, defineSymbol);
            AddSymbolForTarget(BuildTargetGroup.iOS, defineSymbol);
        }


        private static void AddSymbolForTarget(BuildTargetGroup targetGroup, string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            if (defines.Contains(defineSymbol))
            {
                return;
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, (defines + ";" + defineSymbol));
        }
        #endregion

        #region Remove Symbols
        public static void RemoveScriptingDefineSymbol(string defineSymbol)
        {
            RemoveSymbolForTarget(BuildTargetGroup.Android, defineSymbol);
            RemoveSymbolForTarget(BuildTargetGroup.iOS, defineSymbol);
        }

        private static void RemoveSymbolForTarget(BuildTargetGroup targetGroup, string defineSymbol)
        {
            string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(targetGroup);
            List<string> allSymbols = defines.Split(';').ToList();

            if (allSymbols.Contains(defineSymbol))
            {
                allSymbols.Remove(defineSymbol);
            }

            string newDefine = "";
            foreach (string symbol in allSymbols)
            {
                newDefine = newDefine + ";" + symbol;
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, newDefine);
        }
        #endregion

        public static bool HasDefineSymbol(string defineSymbol)
        {
            string defines = "";

            #if UNITY_IOS
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup.iOS));
            #elif UNITY_ANDROID
            defines = PlayerSettings.GetScriptingDefineSymbolsForGroup((BuildTargetGroup.Android));
            #endif

            if (defines.Contains(defineSymbol))
            {
                return true;
            }
            return false;
        }
    }
}
