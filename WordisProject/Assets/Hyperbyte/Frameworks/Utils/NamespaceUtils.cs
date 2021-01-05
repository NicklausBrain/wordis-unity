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
    /// This namespace utils works to detect the given namespace exists in the project or not.
    /// </summary>
    public static class NamespaceUtils
    {
        /// <summary>
        /// Checks if given namespace exists or not.
        /// </summary>
        
        public static bool CheckNamespacesExists(string requiredNameSpace)
        {
            HashSet<string> existingIdentifiers = new HashSet<string>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                System.Reflection.Assembly assembly = assemblies[i];
                System.Type[] types = assembly.GetTypes();

                for (int j = 0; j < types.Length; j++)
                {
                    System.Type type = types[j];

                    string typeNamespace = type.Namespace;
                    existingIdentifiers.Add(typeNamespace);
                }
            }
            return (existingIdentifiers.Contains(requiredNameSpace));
        }


        /// <summary>
        /// Prints all the available namespaces.
        /// </summary>
        public static void PrintAllNameSpaces()
        {
            #if UNITY_EDITOR
            HashSet<string> existingIdentifiers = new HashSet<string>();
            System.Reflection.Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            for (int i = 0; i < assemblies.Length; i++)
            {
                System.Reflection.Assembly assembly = assemblies[i];
                System.Type[] types = assembly.GetTypes();

                for (int j = 0; j < types.Length; j++)
                {
                    System.Type type = types[j];

                    string typeNamespace = type.Namespace;
                    existingIdentifiers.Add(typeNamespace);


                    Debug.Log(typeNamespace);
                }
            }
            #endif
        }
    }

    public class SDKInfo
    {
        public string sdkName { get; set; }
        public string sdkNameSpace { get; set; }
        public string sdkScriptingDefineSymbol { get; set; }

        public SDKInfo(string _sdkName, string _sdkNameSpace, string _sdkScriptingDefineSymbol)
        {
            this.sdkName = _sdkName;
            this.sdkNameSpace = _sdkNameSpace;
            this.sdkScriptingDefineSymbol = _sdkScriptingDefineSymbol;
        }
    }
}
