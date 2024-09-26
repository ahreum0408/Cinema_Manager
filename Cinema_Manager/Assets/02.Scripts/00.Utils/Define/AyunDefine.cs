using Unity.VisualScripting;
using UnityEditor;
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
        public static readonly string InteractionableTag = "Interactionable";
    }

    public static class ObjectPriceInt
    {
        public static readonly int FoodPrice_TakeOut = 5;
        public static readonly int FoodPrice_InStore = 10;
    }

    public static class UpgradeValueInt
    {
        public static readonly int TriangularGimbapNum_Create = 3; // »ï°¢±è¹ä
        public static readonly int CupRamenNum_Create = 3; // ÄÅ¶ó¸é
        public static readonly int BreadNum_Create = 3; // »§
        public static readonly int SnackNum_Create = 3; // °úÀÚ
        public static readonly int JellyNum_Create = 3; // Á©¸®

        public static readonly int CoffeeNum_Create = 3; // Ä¿ÇÇ
        public static readonly int CokeNum_Create = 3; // ÄÝ¶ó
        public static readonly int JuiceNum_Create = 3; // ÁÖ½º
        public static readonly int BeerNum_Create = 3; // ¸ÆÁÖ
        public static readonly int SojuNum_Create = 3; // ¼ÒÁÖ
    }
}
