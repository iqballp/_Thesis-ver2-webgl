using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

public class DebugManager : MonoBehaviour
{
    // log versi upload
    private StringBuilder logData = new StringBuilder();
    // log versi download
    private StringBuilder logDownloadData = new StringBuilder();
    private static DebugManager instance;
    readonly bool savelog = true;

    public string filepath;
    public GameLoger gameLogerScript;
    StreamWriter sw;

    private void Awake()
    {
        gameLogerScript = GameObject.Find("GameLoger").GetComponent<GameLoger>();
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(transform.gameObject);
            InitLogFile();
        }

    }

    void InitLogFile()
    {
        try
        {
            filepath = Application.persistentDataPath + Path.DirectorySeparatorChar + gameLogerScript.pemain + "-[bilblok].txt";

            Log("attempting writing log file: " + filepath);

            File.WriteAllText(filepath, System.DateTime.Now.ToString() + "\n\n");

            Log("log file created: " + filepath);
        }
        catch (System.Exception e)
        {
            Log("failed to create log file\n" + e);
        }
    }

    // fungsi untuk menyimpan informasi log yang nantinya diupload ke firebase
    public void AddLogEntry(string logEntry)
    {
        logData.AppendLine(logEntry);
    }
    public string GetLogData()
    {
        return logData.ToString();
    }
    public void ClearLogData()
    {
        logData.Clear();
    }

    // fungsi untuk menyimpan info log yang nanti di download menjadi txt
    public void AddLogDownloadEntry(string logEntry)
    {
        logDownloadData.AppendLine(logEntry);
    }

    public string GetLogDownloadData()
    {
        return logDownloadData.ToString();
    }

    public void Log(object message)
    {
        if (savelog)
        {
            sw = File.AppendText(filepath);
            sw.WriteLine(System.DateTime.Now.ToString() + " | " + message);
            sw.Close();
        }

        Debug.Log(message);
    }


}