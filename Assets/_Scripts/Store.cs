
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Store : MonoBehaviour
{

    public static Store Instance;
    
    private List<DragAndDrop> _seeItemList;
    private List<int> _seeCostList;
    [SerializeField] private List<Transform> _seePos;


    private List<DragAndDrop> _doItemList;
    private List<int> _doCostList;
    [SerializeField] private List<Transform> _doPos;
    [SerializeField] private Canvas _canvas;

    [SerializeField] private GameObject _costPrefab;

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

    public void Init(Level level) 
    {
        //See vars
        _seeItemList = new(level.SeeItemList);
        _seeCostList = new(level.SeeCostList);

        //Do vars
        _doItemList = new(level.DoItemList);
        _doCostList = new(level.DoCostList);

        for (int i = 0; i < _seeItemList.Count; i++)
        {
            _seeItemList[i].Cost = _seeCostList[i];

            //draw the sprites
            Vector3 pos = _seePos[i].position;
            Instantiate(_seeItemList[i], pos, Quaternion.identity, transform);
            GameObject go = Instantiate(_costPrefab, pos + new Vector3(60 * _canvas.scaleFactor, 0, 0), Quaternion.identity, transform);
            go.GetComponentInChildren<TextMeshProUGUI>().text = _seeCostList[i].ToString();

        }



        for (int i = 0; i < _doItemList.Count; i++)
        {
            _doItemList[i].Cost = _doCostList[i];

            //draw the sprites

            
            Vector3 pos = _doPos[i].position;
            DragAndDrop obj = Instantiate(_doItemList[i], pos, Quaternion.identity, transform);
 
            

            GameObject go = Instantiate(_costPrefab, pos + new Vector3(60 * _canvas.scaleFactor, 0, 0), Quaternion.identity, transform);
            go.GetComponentInChildren<TextMeshProUGUI>().text = _doCostList[i].ToString();
        }
    }
    









    
}
