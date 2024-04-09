using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerPlatforms : MonoBehaviour
{
    [SerializeField] public TriggerType triggerType;
    private PlayerMovement playerScript;
    private Material objectMaterial;
    private bool isActive = true;

    //Disolve
    [SerializeField] private float timeToDisapear = 1;
    [SerializeField] private float timeToApear = 4;
    [SerializeField] private GameObject objectToDisapear;

    //Button
    [SerializeField] private bool isToggle;
    [SerializeField] private List<GameObject> targetObjects;

    public enum TriggerType
    {
        JumpBoost = 0,
        SpeedBoost,
        DisolveFloor,
        Button,
        LevelEnd,
    }

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        if (objectToDisapear != null ) objectMaterial = objectToDisapear.GetComponent<Renderer>().material;
        if (triggerType == TriggerType.DisolveFloor) isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (triggerType)
        {
            case TriggerType.JumpBoost:
                other.GetComponent<PlayerMovement>().isOnJumpBoost = true;
                break;

            case TriggerType.SpeedBoost:
                other.GetComponent<PlayerMovement>().isOnSpeedBoost = true;
                break;

            case TriggerType.DisolveFloor:
                if (!isActive) StartCoroutine(Dissolve());
                break;

            case TriggerType.Button:
                GetComponent<Animator>().SetBool("IsPressed", true);
                foreach (GameObject _object in targetObjects)
                {
                    if (isToggle) _object.GetComponent<IInteractibleObiects>().ObiectToggle();
                    else _object.GetComponent<IInteractibleObiects>().ObiectInteract();
                }
                break;

            case TriggerType.LevelEnd:
                int actualLevel = Int32.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
                if (PlayerPrefs.GetInt("LastSavedLevel") < actualLevel) PlayerPrefs.SetInt("LastSavedLevel", actualLevel + 1);
                SceneManager.LoadScene("Lobby");
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        switch (triggerType)
        {
            case TriggerType.JumpBoost:
                other.GetComponent<PlayerMovement>().isOnJumpBoost = false;
                break;

            case TriggerType.SpeedBoost:
                other.GetComponent<PlayerMovement>().isOnSpeedBoost = false;
                break;
        }
    }

    #region Disolve
    private IEnumerator Dissolve()
    {
        isActive = true;
        float time = 0;
        float dissolveStrength;
        while (time < timeToDisapear)
        {
            time += Time.deltaTime;
            dissolveStrength = Mathf.Lerp(0, 1, time / timeToDisapear);
            objectMaterial.SetFloat("_TreshHold", dissolveStrength);
            if (objectMaterial.GetFloat("_TreshHold") > 0.7) StartCoroutine(DisapearAndApear());
            yield return null;
        }
    }

    private IEnumerator DisapearAndApear()
    {
        objectToDisapear.GetComponent<BoxCollider>().enabled = false;

        float time = 0;
        while (time < timeToApear)
        {
            time += Time.deltaTime;
            if (Mathf.Lerp(0, 1, time / timeToApear) > 0.8)
            {
                objectMaterial.SetFloat("_TreshHold", 0);
                objectToDisapear.GetComponent<BoxCollider>().enabled = true;
            }
            yield return null;
        }
        isActive = false;
    }
    #endregion
}
