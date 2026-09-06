using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public static class RunnerInput
{
    public static bool JumpPressedThisFrame()
    {
        bool pressed = false;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
        {
            pressed |= keyboard.spaceKey.wasPressedThisFrame;
            pressed |= keyboard.upArrowKey.wasPressedThisFrame;
            pressed |= keyboard.wKey.wasPressedThisFrame;
        }
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        pressed |= Input.GetKeyDown(KeyCode.Space);
        pressed |= Input.GetKeyDown(KeyCode.UpArrow);
        pressed |= Input.GetKeyDown(KeyCode.W);
#endif

        return pressed;
    }

    public static bool RestartPressedThisFrame()
    {
        bool pressed = false;

#if ENABLE_INPUT_SYSTEM
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null)
            pressed |= keyboard.rKey.wasPressedThisFrame;
#endif

#if ENABLE_LEGACY_INPUT_MANAGER
        pressed |= Input.GetKeyDown(KeyCode.R);
#endif

        return pressed;
    }
}
