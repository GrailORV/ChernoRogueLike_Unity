using System;
using UnityEngine;

namespace WindowAnimation
{
    public interface ITweenAnimation
    {
        bool  IsAnimation   { get; }
        float AnimationTime { set; get; }
        float DelayTime     { set; get; }

        void SetUp(RectTransform rectT, float time, float delay);
        void Open (Action onComplete);
        void Close(Action onComplete); 
    }
}
