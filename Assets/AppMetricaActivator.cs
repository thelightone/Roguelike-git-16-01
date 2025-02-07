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
        Debug.Log("File EXISTS " + JSONSaver.FileExists());
     
        return !JSONSaver.FileExists();
    }
}
