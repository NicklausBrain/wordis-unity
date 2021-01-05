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

using UnityEngine;
using UnityEditor;

public static class DocumentationEditorMenu 
{    
    static string gameDocName = "https://drive.google.com/file/d/1WUMsVeeWNDIJpulC0eDUN0l0HGAwDWSE/preview";
    static string adDocName = "https://drive.google.com/file/d/1Jt4h9HrW7AzyyntVYeh9jDY16nZIO-Lv/preview"; 
    static string iapDocName = "https://drive.google.com/file/d/1mfH2avIJwqI3fEc_D4nQN8nBsftmkoY7/preview"; 
    static string localizationDocName = "https://drive.google.com/file/d/1Aw0e9W_alo2fei_LXYGM2CwZbVaAuM5U/preview"; 
    static string themeDocName = "https://drive.google.com/file/d/1161rAdjfXVfiMk_YccGUf2poTuYyIAW7/preview"; 

    [MenuItem("Hyperbyte/Documentation/GamePlay Setup", false, 00)]
    public static void OpenGameSettingDocumentation() {
        Application.OpenURL(gameDocName);
    }

    [MenuItem("Hyperbyte/Documentation/Ad Network Setup", false, 10)]
    public static void OpenAdSetUpDocumentation() {
        Application.OpenURL(adDocName);
    }

    [MenuItem("Hyperbyte/Documentation/Unity IAP Setup", false, 11)]
    public static void OpenIAPSetUpDocumentation() {
        Application.OpenURL(iapDocName);
    }

    [MenuItem("Hyperbyte/Documentation/Localization Setup", false, 20)]
    public static void OpenLocalizationDocumentation() {
        Application.OpenURL(localizationDocName);
    }

    [MenuItem("Hyperbyte/Documentation/UI Theme Setup", false, 21)]
    public static void OpenUIThemeDocumentation() {
        Application.OpenURL(themeDocName);
    }
}
