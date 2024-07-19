using SadUtils.Data;
using SadUtils.NewInputSystem;

namespace SadUtils.Core
{
    public static class InputProviderFactory
    {
#if ENABLE_INPUT_SYSTEM
        private static NewInputProvider newInputProvider;
#elif ENABLE_LEGACY_INPUT_MANAGER
        private static LegacyInputProvider legacyInputProvider;
#endif

        public static IMouseInputProvider GetMouseInputProvider()
        {
#if ENABLE_INPUT_SYSTEM
            newInputProvider ??= new();
            return newInputProvider;
#elif ENABLE_LEGACY_INPUT_MANAGER
            legacyInputProvider ??= new();
            return legacyInputProvider;
#else
            throw new System.Exception("No input system enabled.");
#endif
        }
    }
}
