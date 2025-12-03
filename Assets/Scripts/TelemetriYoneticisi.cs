using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using Unity.Services.CloudDiagnostics;

public class TelemetriYoneticisi : MonoBehaviour
{
    public static TelemetriYoneticisi Instance;

    async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        try
        {
            // Servisleri başlat
            await UnityServices.InitializeAsync();

            UnityEngine.Debug.Log("✅ Unity Servisleri Başladı! (Analitik Hazır)");

            // Veri toplamaya başla
            AnalyticsService.Instance.StartDataCollection();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("❌ Başlatma Hatası: " + e);
        }
    }

    public void OlayYolla(string olayAdi)
    {
        // 1. Önce butonun fiziksel olarak çalışıp çalışmadığını test edelim
        UnityEngine.Debug.Log("🟢 BUTONA BASILDI. Gönderilecek Veri: " + olayAdi);

        // 2. Servis durumunu kontrol edip yollayalım
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            AnalyticsService.Instance.RecordEvent(olayAdi);
            AnalyticsService.Instance.Flush();
            UnityEngine.Debug.Log("📤 BAŞARILI: Olay Unity Cloud'a yollandı -> " + olayAdi);
        }
        else
        {
            // Eğer servis hazır değilse bunu bize söylesin
            UnityEngine.Debug.LogError("🔴 HATA: Servisler henüz HAZIR DEĞİL! Olay gidemedi.");
        }
    }
}
