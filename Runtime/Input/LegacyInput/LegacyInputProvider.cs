#if ENABLE_LEGACY_INPUT_MANAGER
using SadUtils.Types;
using UnityEngine;

namespace SadUtils.LegacyInput
{
    public class LegacyInputProvider : IMouseInputProvider
    {
        public Vector3 GetMousePosition()
        {
            return Input.mousePosition;
        }
    }
}
#endif
