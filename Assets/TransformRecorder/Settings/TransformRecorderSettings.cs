using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TransformRecorder
{
    [CreateAssetMenu(fileName = "Settings", menuName = "Transform Recorder/Create Settings")]
    public class TransformRecorderSettings : ScriptableObject
    {
        /// returns animation cliip
        static public AnimationClip GetAnimationClip(uint index)
        {
            if(null == instance)
            {
                return null;
            }

            if(instance.animations.Length <= index)
            {
                return null;
            }
            
            return instance.animations[index];
        }

        static readonly string PathKey = "TR_Key_path";

        class AssetImorter : UnityEditor.AssetPostprocessor
        {
            static readonly string assetPathEnd = ".asset";


            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                if(FindSettingsAssetAndUpdatePath(importedAssets))
                {
                    return;
                }

                if(FindSettingsAssetAndUpdatePath(movedAssets))
                {
                    return;
                }
            }

            static bool FindSettingsAssetAndUpdatePath(string[] paths)
            {
                foreach(var path in paths)
                {
                    if(!path.EndsWith(assetPathEnd))
                    {
                        continue;
                    }
                    
                    var setting = UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(TransformRecorderSettings)) as TransformRecorderSettings;
                    if(null == setting)
                    {
                        continue;
                    }

                    // store path of settings asset to editor prefs.
                    UnityEditor.EditorPrefs.SetString(PathKey, path);

                    return true;
                }

                return false;
            }
        }

        static TransformRecorderSettings instance = null;

        [RuntimeInitializeOnLoadMethod]
        static void OnRuntimeInitialize()
        {
    #if UNITY_EDITOR
            // load settings asset path from editor prefs.
            var settingsAssetPath = UnityEditor.EditorPrefs.GetString(PathKey, string.Empty);
            if(string.IsNullOrEmpty(settingsAssetPath))
            {
                return;
            }

            instance = UnityEditor.AssetDatabase.LoadAssetAtPath(settingsAssetPath, typeof(TransformRecorderSettings)) as TransformRecorderSettings;
    #endif
        }

        public AnimationClip[] animations;
    }
}