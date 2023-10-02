using UnityEngine;

namespace Core
{
    // todo: find a poetools home
    public class InputUtil
    {
        public static bool TryGetAlphaKeyDown(out int key)
        {
            key = -1;

            if (Input.GetKeyDown(KeyCode.Alpha0)) key = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1)) key = 1;
            if (Input.GetKeyDown(KeyCode.Alpha2)) key = 2;
            if (Input.GetKeyDown(KeyCode.Alpha3)) key = 3;
            if (Input.GetKeyDown(KeyCode.Alpha4)) key = 4;
            if (Input.GetKeyDown(KeyCode.Alpha5)) key = 5;
            if (Input.GetKeyDown(KeyCode.Alpha6)) key = 6;
            if (Input.GetKeyDown(KeyCode.Alpha7)) key = 7;
            if (Input.GetKeyDown(KeyCode.Alpha8)) key = 8;
            if (Input.GetKeyDown(KeyCode.Alpha9)) key = 9;

            return key != -1;
        }
    }
}
