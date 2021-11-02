using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global {
    /// tags
    public static string tag_environment = "environment";
    /// layers
    /// pause system
    private static float pre_pause_time_scale = 1; // ! used to reset time scale to what it was before pausing the game, in case we changed it for slow mo effects
    public static void set_pause(bool value) {
        if (value) {
            pre_pause_time_scale = Time.timeScale;
            Time.timeScale = 0;
        }
        else Time.timeScale = pre_pause_time_scale;
    }
}
