using System;
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

    public static class AudioClips
    {
        public static readonly AudioClip Money    = Resources.Load<AudioClip>("Sound/MoneySound");
        public static readonly AudioClip Stack    = Resources.Load<AudioClip>("Sound/StackSound");
        public static readonly AudioClip Trashcan = Resources.Load<AudioClip>("Sound/TrashcanSound");
        public static readonly AudioClip BoxPacking = Resources.Load<AudioClip>("Sound/BoxPacking");

        // Truck
        public static readonly AudioClip TruckStart  = Resources.Load<AudioClip>("Sound/TruckStartSound");
        public static readonly AudioClip TruckStop   = Resources.Load<AudioClip>("Sound/TruckStopSound");
        public static readonly AudioClip TruckEngine = Resources.Load<AudioClip>("Sound/TruckEngineSound");
        public static readonly AudioClip TruckHorn   = Resources.Load<AudioClip>("Sound/TruckHornSound");

        // UI
        public static readonly AudioClip Click = Resources.Load<AudioClip>("Sound/ClickSound");

        // BGM
        public static readonly AudioClip BGM1 = Resources.Load<AudioClip>("Sound/BGM 1");
        public static readonly AudioClip BGM2 = Resources.Load<AudioClip>("Sound/BGM 2");
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

    public enum PoolableType
    {
        None,
        TriangleKimbap, Coke, CupRamen, Beer, Snack,
        Juice, Jelly, Coffee, Bread, Soju,
        Money, Trash,
        Box,

        // Effect
        SmokeEffect,

        // Sound
        SoundObject,
    }
}
