using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ElmasBuyutme : MonoBehaviour
{
    // Büyüme hýzý hassasiyeti
    public float hassasiyet = 0.01f;

    // En küçük ve en büyük ne kadar olsun?
    public float minBoyut = 0.05f; // Çok küçülmesin
    public float maxBoyut = 0.5f;  // Çok büyümesin

    void Update()
    {
        // Ekranda tam 2 parmak varsa çalýþýr
        if (Input.touchCount == 2)
        {
            // Ýki parmaðý alalým
            Touch parmak1 = Input.GetTouch(0);
            Touch parmak2 = Input.GetTouch(1);

            // Parmaklarýn önceki pozisyonlarýný buluyoruz
            Vector2 p1OncekiPos = parmak1.position - parmak1.deltaPosition;
            Vector2 p2OncekiPos = parmak2.position - parmak2.deltaPosition;

            // Önceki ve þimdiki mesafeleri ölçüyoruz
            float oncekiMesafe = (p1OncekiPos - p2OncekiPos).magnitude;
            float simdikiMesafe = (parmak1.position - parmak2.position).magnitude;

            // Aradaki farký bul (Açýlýyor mu kapanýyor mu?)
            float fark = simdikiMesafe - oncekiMesafe;

            // Eðer hareket varsa boyutu deðiþtir
            if (Mathf.Abs(fark) > 0.01f) // Titremeyi önlemek için küçük tolerans
            {
                // Mevcut boyutu al
                Vector3 mevcutBoyut = transform.localScale;

                // Farký boyuta ekle (Hassasiyet ile çarparak)
                float degisim = fark * hassasiyet;

                // Yeni boyutu hesapla (X, Y, Z hepsi orantýlý artsýn)
                float yeniX = mevcutBoyut.x + degisim;
                float yeniY = mevcutBoyut.y + degisim;
                float yeniZ = mevcutBoyut.z + degisim;

                // Sýnýrlarý uygula (Mathf.Clamp ile min ve max arasýna sýkýþtýr)
                yeniX = Mathf.Clamp(yeniX, minBoyut, maxBoyut);
                yeniY = Mathf.Clamp(yeniY, minBoyut, maxBoyut);
                yeniZ = Mathf.Clamp(yeniZ, minBoyut, maxBoyut);

                // Yeni boyutu objeye uygula
                transform.localScale = new Vector3(yeniX, yeniY, yeniZ);
            }
        }
    }
}