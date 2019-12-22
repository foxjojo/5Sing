using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Lrc
{
    [System.Serializable]
    public struct SLrcc
    {
        public int time;
        public string text;
    }
    [System.Serializable]
    public struct SData
    {
        public List<SLrcc> lrc;
    }
    [System.Serializable]
    public struct SLrc
    {
        public SData data;
        public int type;
    }
    [System.Serializable]
    public class LrcJson
    {
        public bool collect;
        public bool isSuccess;
        public bool love;
        public SLrc lrc;
        public string txt;

        public static LrcJson ReadJson(string data)
        {
            return JsonUtility.FromJson<LrcJson>(data);
        }
    }

}