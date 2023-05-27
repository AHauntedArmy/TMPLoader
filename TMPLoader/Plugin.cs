using System.Reflection;
using System.Collections;
using BepInEx;
using UnityEngine;
using TMPro;

namespace TMPLoader
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class TMPLoader : BaseUnityPlugin
    {
        void Awake()
        {
            StartCoroutine(Delay());
        }

        IEnumerator Delay()
        {
            yield return 0;
            if (TMP_Settings.LoadDefaultSettings() != null)
            {
                Debug.Log("another mod as loaded TextMeshPro");
                yield break;
            }

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TMPLoader.Resources.tmprodefault");
            if (stream == null)
            {
                Debug.LogError("TMPLoader.Resources.tmprodefault resource was not found");
                yield break;
            }

            var tmproBundle = AssetBundle.LoadFromStreamAsync(stream);
            yield return tmproBundle;
            if (tmproBundle == null || tmproBundle.assetBundle == null)
            {
                Debug.LogError("failed to load asset bundle from stream");
                yield break;
            }

            var tmproSettings = tmproBundle.assetBundle.LoadAsset<TMP_Settings>("TMP Settings");
            if (tmproSettings != null)
            {
                typeof(TMP_Settings).GetField("s_Instance", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, tmproSettings);
            }
            else
            {
                Debug.LogError("failed to load asset \"TMP Settings\" from asset bundle");
            }
            
            tmproBundle.assetBundle.Unload(false);
        }
    }
}
