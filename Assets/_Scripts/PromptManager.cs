using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    public static PromptManager Instance;
    [SerializeField] private GameObject _promptWindow;
    [SerializeField] private TextMeshProUGUI _promptTMP;

    private Coroutine _popUpCoroutine;

    private void Awake()
    {
        //Stop any duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void PopUpText(string text)
    {
        if (_popUpCoroutine != null)
        {
            StopCoroutine(_popUpCoroutine);
        }

        _popUpCoroutine = StartCoroutine(PopUpCoroutine(text));

    }

    private IEnumerator PopUpCoroutine(string text)
    {
        Hide(false);
        _promptTMP.text = text;
        yield return new WaitForSeconds(2f);
        Hide(true);
    }

    private void Hide(bool hide) //false: will show | true: will hide
    {
        _promptWindow.SetActive(!hide); 
    }

}
