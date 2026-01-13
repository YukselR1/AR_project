using UnityEngine;

public class Scene05Controller : MonoBehaviour
{
    public GameObject sceneRoot;
    public Animator chickenAnimator;
    public AudioSource audioSource;

    void Start()
    {
        sceneRoot.SetActive(false);
    }

    public void OnTargetFound()
    {
        sceneRoot.SetActive(true);
        chickenAnimator.Play(0);
        audioSource.Play();
    }

    public void OnTargetLost()
    {
        audioSource.Stop();
        sceneRoot.SetActive(false);
    }
}
