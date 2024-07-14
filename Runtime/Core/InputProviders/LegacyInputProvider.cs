using UnityEngine;

#if ENABLE_LEGACY_INPUT_MANAGER
namespace SadUtils.Core
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
