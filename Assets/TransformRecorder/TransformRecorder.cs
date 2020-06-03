using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

namespace TransformRecorder
{
    public class TransformRecorder : MonoBehaviour
    {
        // Record or Playback
        public enum Mode
        {
            /// start recording transform animation when button is triggered.
            Record,

            /// playback recorded animation
            Playback,
        }

        // Start Recording
        public void StartRecording()
        {
            if (mode != Mode.Record)
            {
                return;
            }

            recorder = new GameObjectRecorder(gameObject);
            recorder.BindComponentsOfType<Transform>(gameObject, true);
        }

        public void StopRecording()
        {
            if (recorder == null)
            {
                return;
            }

            if (clip == null)
            {
                return;
            }

            recorder.SaveToClip(clip);
            recorder = null;
        }

        public bool IsRecording()
        {
            return (recorder != null);
        }

        public void StartPlayback()
        {
            var anim = GetComponent<Animation>();
            if(anim == null)
            {
                return;
            }
            anim.Stop();
            anim.Play();
        }



        [SerializeField]
        uint animationClipIndex;

        [SerializeField]
        public Mode mode;

        GameObjectRecorder recorder;
        
        AnimationClip clip;

        void Start()
        {
            clip = TransformRecorderSettings.GetAnimationClip(animationClipIndex);
            if(null == clip)
            {
                Destroy(this);
                return;
            }

            switch(mode)
            {
                case Mode.Record:
                    break;
                case Mode.Playback:
                    var anim = gameObject.AddComponent<Animation>();
                    anim.clip = clip;
                    anim.AddClip(clip, clip.name);
                    anim.Play();
                    break;
            }
        }

        void LateUpdate()
        {
            if (clip == null)
            {
                return;
            }

            if (recorder != null)
            {
                recorder.TakeSnapshot(Time.deltaTime);
            }
        }

        void OnDisable()
        {
            if (clip == null)
            {
                return;
            }

            if ((recorder != null ) && (recorder.isRecording))
            {
                recorder.SaveToClip(clip);
            }
        }
    }
}