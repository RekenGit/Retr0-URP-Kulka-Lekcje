using System.Collections;
using UnityEngine;

public class DisolveUI : MonoBehaviour
{
    [SerializeField] private float disolveDurationTime;
    private Material UIDisolveObject;

    void Start()
    {
        UIDisolveObject = GetComponent<Renderer>().material;
        StartCoroutine(StartUIDisolve());
    }
    private IEnumerator StartUIDisolve()
    {
        float time = 0;
        float strenght;
        while (time < disolveDurationTime)
        {
            time += Time.deltaTime;
            strenght = Mathf.Lerp(0, 1, time / disolveDurationTime);
            UIDisolveObject.SetFloat("_TreshHold", strenght);
            if (strenght > 0.9) transform.gameObject.SetActive(false);
            yield return null;
        }
    }
}
