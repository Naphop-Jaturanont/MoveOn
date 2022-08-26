using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extendtion 
{ 
    public static void CrossFade(this Animator animator, crossfangSetting setting)
    {
        animator.CrossFade(setting.stateName, setting.transsitionDuration, setting.layer, setting.timeoffset);
    }

    public static void CrossFadeInFixedTime(this Animator animator,crossfangSetting setting)
    {
        animator.CrossFadeInFixedTime(setting.stateName, setting.transsitionDuration, setting.layer, setting.timeoffset);
    }
}
