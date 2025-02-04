using Io.AppMetrica;
using UnityEngine;

public static class AppMetricaActivator
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate()
    {
        AppMetrica.Activate(new AppMetricaConfig("161039fe-d723-4335-ad28-382f294bd183")
        {
            FirstActivationAsUpdate = !IsFirstLaunch(),
        });
    }

    private static bool IsFirstLaunch()
    {
        // Implement logic to detect whether the app is opening for the first time.
        // For example, you can check for files (settings, databases, and so on),
        // which the app creates on its first launch.
        return true;
    }
}
