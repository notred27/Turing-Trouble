using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class LevelSelectBtn : MonoBehaviour
{

    [SerializeField] private string _dst;
    [SerializeField] private Level LevelPointer;
    private Button _btn;

    public void Awake() {
        _btn = GetComponent<Button>();
    }

    private void Start()
    {
        _btn.onClick.AddListener(() =>NextScene());
    }

    private void NextScene()
    {
        SceneLoader.Instance.level = LevelPointer;
        SceneManager.LoadScene(_dst);
    }
}
