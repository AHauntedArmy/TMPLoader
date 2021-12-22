using System;
using System.IO;
using System.Reflection;

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
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TMPLoader.Resources.tmprodefault");
            if (stream == null)
            {
                Debug.LogError("TMPLoader.Resources.tmprodefault resource was not found");
                return;
            }

            var tmproBundle = AssetBundle.LoadFromStream(stream);
            if (tmproBundle == null)
            {
                Debug.LogError("failed to load asset bundle from stream");
                return;
            }

            var tmproSettings = tmproBundle.LoadAsset<TMP_Settings>("TMP Settings");
            if (tmproSettings == null)
            {
                Debug.LogError("failed to load asset \"TMP Settings\" from asset bundle");
                return;
            }

            typeof(TMP_Settings).GetField("s_Instance", BindingFlags.NonPublic | BindingFlags.Static)?.SetValue(null, tmproSettings);
        }
    }
}
