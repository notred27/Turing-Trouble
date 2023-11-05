using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MoveToScene : MonoBehaviour
{

    [SerializeField] private string _dst;
    private Button _btn;


    public void Awake() {
        _btn = GetComponent<Button>();
        
    }

    private void Start()
    {
        _btn.onClick.AddListener(() => SceneManager.LoadScene(_dst));
    }
}
