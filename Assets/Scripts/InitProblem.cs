using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using HtmlAgilityPack;
using UnityEngine.UI;
using System.IO;
using NAudio.Wave;

public class InitProblem : MonoBehaviour
{
  
    public AudioSource audioSource; 
    public GameObject cell;
    public Image loadImage;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(GetJsonEnumerator("4011741"));

        //StartCoroutine(GetLrcJsonEnumerator("4011741", "yc"));

        StartCoroutine(RankEnumerator());
    }

    private void CreateCell(string name, string author, string parameter)
    {
        GameObject tempCell = Instantiate(cell);
        tempCell.SetActive(true);
        tempCell.transform.SetParent(cell.transform.parent);
        tempCell.transform.Find("Name").GetComponent<Text>().text = name;
        tempCell.transform.Find("Author").GetComponent<Text>().text = author;
        tempCell.transform.Find("PlayButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            StartCoroutine(GetJsonEnumerator(parameter.Split('$')[0]));
            PlayerPrefs.SetString("songId", parameter.Split('$')[0]);
            PlayerPrefs.SetString("songType", parameter.Split('$')[1]);

        });


    }
    private IEnumerator GetJsonEnumerator(string songId)
    {
        WWWForm form = new WWWForm();
        form.AddField("songid",songId);
        var request = UnityWebRequest.Post("http://service.5sing.kugou.com/song/getsongurl",form);
        yield return request.SendWebRequest();
        GetSongUrlJson json = GetSongUrlJson.ReadJson(request.downloadHandler.text);
       
        string wavFileTempPath = Application.persistentDataPath + "/temp.wav";
        if (json.data.lqext == "mp3") 
        {
            Debug.Log("mp3");

            request = UnityWebRequest.Get(json.data.lqurl);
            string mp3FileTempPath = Application.persistentDataPath + "/temp." + json.data.lqext;

            request.downloadHandler = new DownloadHandlerFile(mp3FileTempPath, true);
            request.SendWebRequest();

            while (!request.isDone)
            {
                loadImage.fillAmount = 1.0f - request.downloadProgress;
                yield return null;
            }
            //var stream = File.Open(mp3FileTempPath, FileMode.Open);
            //var reader = new Mp3FileReader(stream);
            //WaveFileWriter.CreateWaveFile(wavFileTempPath, reader);
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(mp3FileTempPath, AudioType.MPEG))
            {
                yield return uwr.Send();
                if (uwr.isNetworkError)
                {
                    Debug.LogError(uwr.error);
                    yield break;
                }
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                // use audio clip
                audioSource.clip = clip;
                audioSource.Play();
            }




        } else if (json.data.lqext == "wav")
        {
            Debug.Log("wav");
            request = UnityWebRequest.Get(json.data.lqurl);
            request.downloadHandler = new DownloadHandlerFile(wavFileTempPath, true);
            yield return request.SendWebRequest();
        request = UnityWebRequestMultimedia.GetAudioClip(wavFileTempPath, AudioType.WAV);
        yield return request.SendWebRequest();
        audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
   
        audioSource.Play();
        }


    }



    private IEnumerator RankEnumerator()
    {
        var request = UnityWebRequest.Get("http://5sing.kugou.com/top/");
        yield return request.SendWebRequest();
        var doc = new HtmlDocument();
        doc.LoadHtml(request.downloadHandler.text);
        var temp = doc.DocumentNode.SelectSingleNode("/html/body/div[3]/div[2]/div[2]/div[2]/table").SelectNodes("//tr");
        temp.Remove(0);
        foreach (var item in temp)
        {
            string para = item.ChildNodes[0].LastChild.Attributes["value"].Value;
            string name = item.ChildNodes[2].LastChild.InnerText;//歌名
            string author = item.ChildNodes[3].LastChild.InnerText;//作者
            CreateCell(name, author, para);
        }
            
        
    }

}
