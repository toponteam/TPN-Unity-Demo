using System;


public class InitSdkHelper
{
    private static bool isInited = false;

    public static bool IsInited {
        get {
            return isInited;
        }
        set {
            isInited = value;
        }
    }
}