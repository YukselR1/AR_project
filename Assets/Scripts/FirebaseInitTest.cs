using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseInitTest : MonoBehaviour
{
    async void Start()
    {
        // Firebase baþlat
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            Debug.Log("Firebase baþarýyla baþlatýldý!");

            // Auth örneði
            FirebaseAuth auth = FirebaseAuth.DefaultInstance;

            // Database örneði
            DatabaseReference dbRef = FirebaseDatabase.GetInstance(
    "https://arhikaye-auth-default-rtdb.europe-west1.firebasedatabase.app/"
).RootReference;
            Debug.Log("Firebase Auth ve Database hazýr.");
        }
        else
        {
            Debug.LogError("Firebase baþlatýlamadý: " + dependencyStatus);
        }
    }
}
