using System;

namespace SadUtils.UI
{
    [Flags]
    public enum ButtonTransition
    {
        ColorTint = 1,       // 00001
        SpriteSwap = 2,      // 00010
        Animation = 4,       // 00100
        TextSwap = 8,        // 01000
        TextColorTint = 16,  // 10000
    }
}
