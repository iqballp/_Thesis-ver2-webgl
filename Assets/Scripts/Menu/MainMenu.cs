using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Animator tirai;
    public GameObject inputPanel;
    public GameObject Popup;
    public GameObject PopupUjian;
    public GameObject PopupPersetujuan;
    public GameObject playerNameObj;
    // public GameObject inputField;
    public TMP_Text playerNameInputField;
    public TMP_Text playerNameText;

    public GameLoger loger;
    private ScoreLoger sl;

    IEnumerator nextScene(string sceneTarget, float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneTarget);
    }

    public void toTutorial()
    {
        loger.isDoneTutorial = true;
        tirai.SetTrigger("close");
        StartCoroutine(nextScene("tutorial1", 2f));
    }

    public void toUjian()
    {
        tirai.SetTrigger("close");
        StartCoroutine(nextScene("MainScene", 2f));
    }

    public void UjianBtn()
    {
        if (loger.isDoneTutorial == false)
        {
            Popup.SetActive(true);
            PopupUjian.SetActive(true);
        }
        else
        {
            toUjian();
        }

    }

    public void SetujuBtn()
    {
        PopupPersetujuan.SetActive(false);
        loger.isSetuju = true;
    }

    public void toLatihan()
    {
        tirai.SetTrigger("close");
        StartCoroutine(nextScene("LatihanScene", 2f));
    }

    public void InitNama()
    {
        // string nama;
        // nama = inputfield.GetComponent<Text>().text;
        loger.pemain = playerNameInputField.text;
        inputPanel.SetActive(false);
        playerNameText.text = loger.pemain;
        // if(nama != null || nama != "")
        // {
        //     loger.pemain = nama;
        //     inputPanel.SetActive(false);
        // }
    }

    void Start()
    {
        loger = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        Popup.SetActive(false);
        PopupUjian.SetActive(false);
        playerNameObj.SetActive(false);

        if (loger.isSetuju)
        {
            PopupPersetujuan.SetActive(false);
        }
        else
        {
            PopupPersetujuan.SetActive(true);
            inputPanel.SetActive(false);
        }

    }

    void Update()
    {
        if (loger.isSetuju)
        {
            if (!string.IsNullOrEmpty(loger.pemain))
            {
                inputPanel.SetActive(false);
                playerNameText.text = loger.pemain;

                playerNameObj.SetActive(true);
            }
            else
            {
                inputPanel.SetActive(true);
            }
        }

    }
}
