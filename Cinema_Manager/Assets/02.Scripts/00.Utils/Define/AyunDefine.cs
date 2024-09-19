using UnityEngine;

public static class AyunDefine
{
    public static class AnimatorPropertyHash
    {
        // Agent
        public static readonly int IsRun = Animator.StringToHash("IsRun");
    }

    public static class ObjectTagString
    {
        public static readonly string PlayerTag = "Player";
    }

    public static class ObjectPriceInt
    {
        public static readonly int BreadPrice_TakeOut = 5;
        public static readonly int BreadPrice_InStore = 10;
    }
}
