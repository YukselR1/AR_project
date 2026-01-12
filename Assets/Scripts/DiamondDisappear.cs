using System.Collections;
using UnityEngine;

public class DiamondDisappear : MonoBehaviour
{
    public Transform diamond;
    public Transform buryPoint;
    public float speed = 1f;

    void Start()
    {
        StartDisappear();
    }

    public void StartDisappear()
    {
        StartCoroutine(DisappearRoutine());
    }

    IEnumerator DisappearRoutine()
    {
        while (Vector3.Distance(diamond.position, buryPoint.position) > 0.01f)
        {
            diamond.position = Vector3.MoveTowards(
                diamond.position,
                buryPoint.position,
                speed * Time.deltaTime
            );
            yield return null;
        }

        diamond.gameObject.SetActive(false);
    }
}
