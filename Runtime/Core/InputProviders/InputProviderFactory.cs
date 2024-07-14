namespace SadUtils.Core
{
    public static class InputProviderFactory
    {
        public static IMouseInputProvider GetMouseInputProvider()
        {
#if ENABLE_INPUT_SYSTEM
            return new NewInputProvider();
#elif ENABLE_LEGACY_INPUT_MANAGER
            return new LegacyInputProvider();
#else
            throw new System.Exception("No input system enabled.");
#endif
        }
    }
}
