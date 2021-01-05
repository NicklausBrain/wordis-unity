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

namespace Hyperbyte.UITween 
{
    // Ease type.
    public enum Ease
    {
        Linear,
        EaseIn,
        EaseOut,
        Custom
	}

    // Loop Type of Tween.
    public enum LoopType {
        PingPong,
        Loop
    }

    // Tween Type.
    public enum AnimationType {
        AnchorX,
        AnchorY,
        AnchorZ,
        AnchoredPosition,
        AnchoredPosition3D,

        PositionX,
        PositionY,
        PositionZ,
        Position,

        LocalPositionX,
        LocalPositionY,
        LocalPositionZ,
        LocalPosition,

        LocalRotationTo,
        LocalRotationToX,
        LocalRotationToY,
        LocalRotationToZ,

        LocalScaleX,
        LocalScaleY,
        LocalScaleZ,
        LocalScale,

        RotateByX,
        RotateByY,
        RotateByZ,

        RotateToX,
        RotateToY,
        RotateToZ,

        RotateBy,
        RotateTo,

        Alpha,
        Color,
        FillAmount
    }

    public enum ImageType {
        Image,
        SpriteRenderer
    }
}
