using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject PlayerPanel;
    public GameObject RankPanel;
    public GameObject SearchPanel;

    public Button playerButton;
    public Button rankButton;
    public InputField searchInputField;
    public Button searchButton;

    // Start is called before the first frame update
    void Start()
    {
        playerButton.onClick.AddListener(delegate {
            RankPanel.SetActive(false);
            SearchPanel.SetActive(false);
            PlayerPanel.SetActive(true);
        });

        rankButton.onClick.AddListener(delegate {
            RankPanel.SetActive(true);
            SearchPanel.SetActive(false);
            PlayerPanel.SetActive(false);
        });


        searchButton.onClick.AddListener(delegate {
            RankPanel.SetActive(false);
            SearchPanel.SetActive(true);
            PlayerPanel.SetActive(false);
        });
    }

  
}
