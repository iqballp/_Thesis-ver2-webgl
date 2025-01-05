using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

// namespace XCharts.Example
public class LineChartData : MonoBehaviour
{
    private ScoreLoger sl;
    public float[] newpoints;
    // Start is called before the first frame update
    void Start()
    {
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
        // float[] newpoints = new float[sl.jumlahKerjakanSoal];
        for (int i = 0; i < sl.jumlahKerjakanSoal; i++)
        {
            newpoints = AddElementToArray(newpoints, (Mathf.Round(sl.points[i] * 1000.0f) / 1000.0f));
        }

        AddData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private float[] AddElementToArray(float[] array, float element)
    {
        // Buat array baru dengan ukuran lebih besar
        float[] newArray = new float[array.Length + 1];

        // Salin elemen-elemen dari array lama ke array baru
        for (int i = 0; i < array.Length; i++)
        {
            newArray[i] = array[i];
        }

        // Tambahkan elemen baru
        newArray[array.Length] = element;

        return newArray;
    }

    void Awake()
    {
        
    }

    void AddData()
    {
        var chart = gameObject.GetComponent<LineChart>();
        if (chart == null)
        {
            chart = gameObject.AddComponent<LineChart>();
            chart.Init();
        }
        chart.EnsureChartComponent<Title>().show = true;
        chart.EnsureChartComponent<Title>().text = "Grafik Performa Pemain";

        chart.EnsureChartComponent<Tooltip>().show = true;
        chart.EnsureChartComponent<Legend>().show = false;

        var xAxis = chart.EnsureChartComponent<XAxis>();
        var yAxis = chart.EnsureChartComponent<YAxis>();
        xAxis.show = true;
        yAxis.show = true;
        // xAxis.minMaxType = Axis.AxisMinMaxType.Custom;
        // xAxis.min = 1;
        // xAxis.max = sl.jumlahKerjakanSoal;
        xAxis.type = Axis.AxisType.Category;
        yAxis.type = Axis.AxisType.Value;
        

        xAxis.splitNumber = sl.jumlahKerjakanSoal;
        xAxis.boundaryGap = true;

        chart.RemoveData();
        chart.AddSerie<Line>();
        // chart.AddSerie<Line>();
        for (int i = 0; i < sl.jumlahKerjakanSoal; i++)
        {
            chart.AddXAxisData(""+ (i+1));
            chart.AddData(0, Mathf.Round(newpoints[i] * 1000.0f) * 0.001);

            // sl.poin = sl.poin + sl.points[i];
            // chart.AddData(1, Random.Range(10, 20));
        }
    }
}
