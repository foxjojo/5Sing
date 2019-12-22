using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InitPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject lrcText;
    public RawImage MonthRawImage;
    public Text SongName;
    public Text Author;


    private List<Lrc.SLrcc> lrcList;
    int mark;
    private void OnEnable()
    {
        StartCoroutine(GetLrcJsonEnumerator(PlayerPrefs.GetString("songId"), PlayerPrefs.GetString("songType")));
        StartCoroutine(GetJsonEnumerator(PlayerPrefs.GetString("songId")));
    }
    private IEnumerator GetLrcJsonEnumerator(string songId, string songType)
    {
        mark = 0;
        Debug.LogError(audioSource.clip.length);
        WWWForm form = new WWWForm();
        form.AddField("songId", songId);
        form.AddField("songType", songType);
        var request = UnityWebRequest.Post("http://5sing.kugou.com/fm/m/json/lrc", form);
        yield return request.SendWebRequest();
        Lrc.LrcJson json = Lrc.LrcJson.ReadJson(request.downloadHandler.text);
        GameObject temp;
        Debug.Log(request.downloadHandler.text);
        lrcList = json.lrc.data.lrc;
       
        //清空歌词
        foreach (var item in lrcText.transform.parent.GetComponentsInChildren<Text>())
        {
            Destroy(item.gameObject);
        }

        if(json.lrc.data.lrc.Count == 0)
        {
            temp = Instantiate(lrcText);
            temp.SetActive(true);
            temp.transform.SetParent(lrcText.transform.parent);
            temp.GetComponent<Text>().text = "********该歌词不支持滚动********";
            foreach (var item in json.txt.Split('\n'))
            {
                temp = Instantiate(lrcText);
                temp.SetActive(true);
                temp.transform.SetParent(lrcText.transform.parent);
                temp.GetComponent<Text>().text = item;
            }
          
        }
        else
        {
            temp = Instantiate(lrcText);
            temp.SetActive(true);
            temp.transform.SetParent(lrcText.transform.parent);
            temp.GetComponent<Text>().text = "****************";
            foreach (var item in json.lrc.data.lrc)
            {
                temp = Instantiate(lrcText);
                temp.SetActive(true);
                temp.transform.SetParent(lrcText.transform.parent);
                temp.GetComponent<Text>().text = item.text;
            }
        }
    }

    private IEnumerator GetJsonEnumerator(string songId)
    {
        WWWForm form = new WWWForm();
        form.AddField("songid", songId);
        var request = UnityWebRequest.Post("http://service.5sing.kugou.com/song/getsongurl", form);
        yield return request.SendWebRequest();
        GetSongUrlJson json = GetSongUrlJson.ReadJson(request.downloadHandler.text);

        Author.text = json.data.user.NN;
        SongName.text = json.data.songName;

        request = UnityWebRequestTexture.GetTexture(json.data.user.I);
        yield return request.SendWebRequest();
        MonthRawImage.texture = DownloadHandlerTexture.GetContent(request);
    }
    // Update is called once per frame
   
    private void FixedUpdate()
    {

        
        if (audioSource.clip.length < audioSource.time)
        {
            audioSource.time = 0;
            mark = 0;
        }
        if (audioSource.time * 1000 > lrcList[mark].time)
        {

            mark++;
            lrcText.transform.parent.transform.localPosition = new Vector3(0, 30 * mark, 0);
        }

    }
   
}
