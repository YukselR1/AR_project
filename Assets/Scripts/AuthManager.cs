using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions; // Bu kütüphaneyi ekledik (Daha güvenli)
using UnityEngine.UI;
using System;

public class AuthManager : MonoBehaviour
{
    // BURAYA KENDÝ LÝNKÝNÝ YAPIÞTIR!
    private const string DATABASE_URL = "https://arhikaye-auth-default-rtdb.europe-west1.firebasedatabase.app/";

    [Header("Input Fields")]
    public InputField inputLoginName;
    public InputField inputLoginPassword;
    public InputField inputRegisterName;
    public InputField inputRegisterPassword;

    [Header("Canvas")]
    public GameObject girisCanvas;
    public GameObject kaydolCanvas;
    public GameObject anaCanvas;

    [Header("Ana Sayfa")]
    public Text textWelcome;

    private DatabaseReference dbRef;
    private bool firebaseHazirMi = false;

    void Start()
    {
        // Önce Firebase'in baðýmlýlýklarýný kontrol ediyoruz (Çökmemesi için)
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Her þey yolunda, veritabanýný baþlatabiliriz
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Firebase hatasý: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        // Veritabaný referansýný alýyoruz
        dbRef = FirebaseDatabase.GetInstance(DATABASE_URL).RootReference;
        firebaseHazirMi = true;
        Debug.Log("Firebase baþarýyla baðlandý!");
    }

    // ------------------- Kaydol -------------------
    public void Kaydol()
    {
        if (!firebaseHazirMi) return;

        string isim = inputRegisterName.text;
        string sifre = inputRegisterPassword.text;

        if (string.IsNullOrEmpty(isim) || string.IsNullOrEmpty(sifre))
        {
            Debug.Log("Ýsim ve þifre boþ olamaz!");
            return;
        }

        string key = dbRef.Child("Users").Push().Key;
        User yeniUser = new User(isim, sifre);
        string json = JsonUtility.ToJson(yeniUser);

        // ContinueWithOnMainThread kullanarak UI hatasýný önlüyoruz
        dbRef.Child("Users").Child(key).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                Debug.Log("Kayýt baþarýlý!");
                kaydolCanvas.SetActive(false);
                girisCanvas.SetActive(true);
            }
            else
            {
                Debug.LogError("Kayýt hatasý: " + task.Exception);
            }
        });
    }

    // ------------------- Giriþ -------------------
    public void GirisYap()
    {
        if (!firebaseHazirMi) return;

        string isim = inputLoginName.text;
        string sifre = inputLoginPassword.text;

        if (string.IsNullOrEmpty(isim) || string.IsNullOrEmpty(sifre)) return;

        dbRef.Child("Users").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("Veri okuma hatasý!");
                return;
            }

            DataSnapshot snapshot = task.Result;
            bool girisBasarili = false;

            foreach (var child in snapshot.Children)
            {
                // Veritabanýndaki alan adlarýnla buradakiler ayný olmalý (isim, sifre)
                string dbIsim = child.Child("isim").Value != null ? child.Child("isim").Value.ToString() : "";
                string dbSifre = child.Child("sifre").Value != null ? child.Child("sifre").Value.ToString() : "";

                if (dbIsim == isim && dbSifre == sifre)
                {
                    girisBasarili = true;
                    break;
                }
            }

            if (girisBasarili)
            {
                Debug.Log("Giriþ baþarýlý!");
                girisCanvas.SetActive(false);
                anaCanvas.SetActive(true);
                textWelcome.text = "Hoþgeldin " + isim;
            }
            else
            {
                Debug.Log("Kullanýcý adý veya þifre yanlýþ!");
            }
        });
    }

    // Canvas Geçiþleri
    public void GeriGirisSayfasi()
    {
        kaydolCanvas.SetActive(false);
        girisCanvas.SetActive(true);
    }

    public void GotoKaydolCanvas()
    {
        girisCanvas.SetActive(false);
        kaydolCanvas.SetActive(true);
    }
}

// Verileri düzgün paketlemek için küçük bir sýnýf
public class User
{
    public string isim;
    public string sifre;

    public User(string isim, string sifre)
    {
        this.isim = isim;
        this.sifre = sifre;
    }
}