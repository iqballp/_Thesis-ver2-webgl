using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Script ini digunakan pada setiap buku yang ada
 * bertindak sebagai event handler (drag & drop) pada gameplay
 */

public class DragDrop : MonoBehaviour
{
    //public Vector2 initPos; // untuk menampung nilai posisi awal buku

    public float moveSpeed = 1f;
    public bool dragging = false; // parameter dragaing
    public bool valid = false; // parameter apakah tempat drop valid atau tidak


    private float distance; // variabel penampung jarak

    //menunggu sejenak untuk masukkan nilai initPos setelah gameManager.cs mengacak posisi buku
    //IEnumerator Start()
    //{
    //    yield return StartCoroutine("setInitPos");
    //}

    //IEnumerator setInitPos()
    //{
    //    yield return new WaitForSeconds(0.5f);

    //    // memasaukkan nilai initial position dengan posisi buku setelah diacak
    //    initPos = this.transform.position;
    //}

    void Update()
    {

        // merubah posisi buku saat di drag
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);

            // merubah posisi buku berdasrkan raypoint
            transform.position = rayPoint;
            //transform.position += rayPoint * Time.deltaTime * moveSpeed;
        }
    }



    void OnMouseDown()
    {
        Debug.Log("downed");
        //ketika mouse ditekan pada buku, scale buku bertambah sebgai feedback pada player
       // GetComponent<Transform>().localScale = new Vector2(1.2f, 1.2f);

        // menghitung nilai distance untuk raycas
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        // mengatur parameter draging & valid
        dragging = true;
        valid = false;

    }

    void OnMouseUp()
    {
        //mengembalikan skala buku sebagai feedback pada player
       // GetComponent<Transform>().localScale = new Vector2(1f, 1f);
        dragging = false;

        // jika posisi drop tidak valid, kembalikan posisi buku pada initPos
        if (!valid)
        {
           // gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            //transform.position = initPos;
        }

    }

}
