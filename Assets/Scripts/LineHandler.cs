using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHandler : MonoBehaviour
{
    public UILineRenderer lr;
    public int jarakDot;
    private RectTransform rt;
    private ScoreLoger sl;

    float jumlahPanjang;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        rt = GameObject.Find("LineRenderer").GetComponent<RectTransform>();
        sl = GameObject.Find("ScoreLoger").GetComponent<ScoreLoger>();
        lr.PointCount = sl.jumlahKerjakanSoal;
        for(int i= 0; i < lr.PointCount; i++)
        {
            float x = jarakDot * i;
            float y = sl.points[i];
            jumlahPanjang = jumlahPanjang + jarakDot;

            sl.poin = sl.poin + sl.points[i];

            lr.SetPoint(i, new Vector2(x,y));
        }
        
        rt.sizeDelta = new Vector2 (jumlahPanjang-jarakDot, rt.sizeDelta.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
