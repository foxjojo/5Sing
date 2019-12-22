using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct SUser
{
    public string I;
    public long ID;
    public string NN;
}
[System.Serializable]
public struct SData
{
    public int DigitalAlbumID;
    public string hqext;//音乐格式
    public string hqsize;//音乐大小
    public string hqurl;//高清资源地址
    public string hqurlmd5;
    public string lqext;
    public string lqsize;
    public string lqurl;
    public string lqurlmd5;
    public string sqext;
    public string sqsize;
    public string squrl;
    public string squrlmd5;
    public string songKind;
    public string songName;
    public long songid;
    public string songtype;
    public SUser user;

}
[System.Serializable]
public class GetSongUrlJson
{
    public int code;
    public SData data;
    public string message;
    public string msg;
    public bool success;

    public static GetSongUrlJson ReadJson(string data)
    {
        return JsonUtility.FromJson<GetSongUrlJson>(data);
    }
}
