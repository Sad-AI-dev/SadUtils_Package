#if ENABLE_INPUT_SYSTEM
using SadUtils.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SadUtils.NewInputSystem
{
    public class NewInputProvider : IMouseInputProvider
    {
        public Vector3 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }
    }
}
#endif
