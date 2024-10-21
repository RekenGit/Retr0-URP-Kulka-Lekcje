using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private int levelsCount = 1;
    [SerializeField] private Transform levelSelectPanel;
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private GameObject cheatsGO;
    static bool isCheatsOn = false;
    private int page = 0;
    private int lastActiveLevel = 1;
    private Animator animator;
    private float[] xCorginates = new float[] { -3.5f, -1.75f, 0f, 1.75f, 3.5f };

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        lastActiveLevel = PlayerPrefs.GetInt("LastSavedLevel") != 0 ? PlayerPrefs.GetInt("LastSavedLevel") : 1;
        animator = GetComponent<Animator>();
        if (isCheatsOn)
            lastActiveLevel = 100;
        StartCoroutine(CreateLevelButtons());
    }

    public void CheatsOn()
    {
        cheatsGO.GetComponent<Image>().color = Color.red;
        isCheatsOn = true;
        lastActiveLevel = 100;
        StartCoroutine(CreateLevelButtons());
    }

    public void MoveToLevelPick(bool value) => animator.SetBool("IsInLvlPick", value);

    public void ChangePage(bool addPage = true)
    {
        int _page = page;
        _page = addPage ? _page + 1 : _page - 1;
        if (_page < 0 || (float)levelsCount / 10f <= _page) return;
        page = _page;
        StartCoroutine(CreateLevelButtons(page));
    }

    private IEnumerator CreateLevelButtons(int page = 0)
    {
        foreach (Transform child in levelSelectPanel) Destroy(child.gameObject);
        int firstPage = (10 * page) + 1;
        int rowCell = 0;
        for (int i = firstPage; i <= levelsCount; i++)
        {
            if (i < firstPage + 10)
            {
                GameObject button = Instantiate(levelButtonPrefab);
                button.GetComponent<ButtonLevelSelect>().LevelNumber = i;
                button.transform.SetParent(levelSelectPanel);
                bool posFirstRow = rowCell < 5;
                button.transform.localPosition = new Vector3(xCorginates[posFirstRow ? rowCell : rowCell - 5], (posFirstRow ? 1f : -1f), 0f);
                if (i > lastActiveLevel) button.GetComponent<Button>().interactable = false;
                rowCell++;
            }
            else yield break;
            yield return null;
        }
    }
}
