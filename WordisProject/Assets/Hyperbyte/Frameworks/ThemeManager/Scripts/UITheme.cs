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

/// <summary>
/// UI Theme scriptable instance store all the tags and associated colors or sprite to form a  theme. 
/// This settings can be configured from Hyperbyte -> Theme Settings menu item.
/// </summary>
public class UITheme : ScriptableObject
{
    public ColorThemeTag[] colorTags;
    public SpriteThemeTag[] spriteTags;
}

/// <summary>
/// Color tags attached with theme.
/// </summary>
[System.Serializable]
public class ColorThemeTag
{
    // Tag name.
    public string tagName;

    // Color attached with tag.
    public Color tagColor;
}

/// <summary>
/// Sprite tags attached with theme.
/// </summary>
[System.Serializable]
public class SpriteThemeTag
{
    // Tag name.
    public string tagName;

    // Sprite attached with tag.
    public Sprite tagSprite;
}