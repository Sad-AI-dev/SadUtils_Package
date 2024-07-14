#if ENABLE_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace SadUtils.Core
{
    public class NewInputProvider : IMouseInputProvider
    {
        public Vector3 GetMousePosition()
        {
            return Mouse.current.position;
        }
    }
}
#endif
