using System;
using Android.App.Job;
using Android.Content;
using Java.Lang;

namespace Unlock
{
    public static class ActionUnlock
    {
        public const string Recall = "Android.Content.Intent.ActionScreenOff.Recall";

        public static ComponentName GetComponentNameForJob<T>(this Context context) where T : JobService, new()
        {
            Class javaClass = Class.FromType(typeof(T));
            return new ComponentName(context, javaClass);
        }
    }

}
