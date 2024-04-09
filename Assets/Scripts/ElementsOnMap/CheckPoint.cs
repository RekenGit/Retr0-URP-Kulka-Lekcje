using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject particlesGreen;
    [SerializeField] private GameObject particlesBlue;
    [SerializeField] private Material greenShader;
    [SerializeField] private Material blueShader;
    [SerializeField] private bool isLastCheckPoint = false;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SetNewCheckPoint(this.gameObject);
        SetActiveCheckPoint(true);
        isLastCheckPoint = true;
    }

    private void Update()
    {
        if (GameManager.Instance.GetLastCheckPoint() != this.gameObject && isLastCheckPoint) SetActiveCheckPoint(false);
    }

    public void SetActiveCheckPoint(bool isActive)
    {
        if (!isActive)
        {
            particlesBlue.SetActive(false);
            particlesGreen.SetActive(true);
            GetComponent<MeshRenderer>().material = greenShader;
            isLastCheckPoint = false;
        }
        else
        {
            particlesBlue.SetActive(true);
            particlesGreen.SetActive(false);
            GetComponent<MeshRenderer>().material = blueShader;
        }
    }
}
