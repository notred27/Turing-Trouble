using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class SeeDo : MonoBehaviour
{
    private DragAndDrop _seeItem;
    private DragAndDrop _doItem;
    public See SeeType;
    public Do DoType;

    [SerializeField] private Button addBtn;
    [SerializeField] private Button remBtn;
    [SerializeField] private Canvas _canvas;

    private void Awake()
    {
        //Automatically starts as default states
        remBtn.gameObject.SetActive(true);
        SeeType = See.Nothing;
        DoType = Do.NOTHING;
        _seeItem = null;
        _doItem = null; 
    }

    private void OnDestroy()
    {
        GameManager.Instance.Blocks.Remove(this);
        GameManager.Instance.CheckRunButton();
    }
    private void Start()
    {
        addBtn.onClick.AddListener(() => AddNextSeeDo());
        remBtn.onClick.AddListener(() => RemLastSeeDo());
        if(GameManager.Instance.Blocks.Count == 0) //disabling the minus button if first block
        {
            remBtn.gameObject.SetActive(false);
        }

        GameManager.Instance.Blocks.Add(this);
        
        if (GameManager.Instance.Blocks.Count == GameManager.Instance.MaxBlocks)
        {
            addBtn.gameObject.SetActive(false);
        }
        GameManager.Instance.CheckRunButton();
    }
    public void UnsetItem(DragAndDrop item)
    {
        if (item.IsSee)
        {
            _seeItem = null;
            SeeType = See.Nothing;
        }
        else if (!item.IsSee)
        {
            _doItem = null;
            DoType = Do.NOTHING;
        }
        
        GameManager.Instance.CheckRunButton();

    }

    public void SetItem(DragAndDrop item)
    {

        if (item.IsSee)
        {
            if(_seeItem != null) 
            {
                //Refund the cost 
                GameManager.Instance.AddToCoins(_seeItem.Cost);
                
                //destroy the item in the See Do Block
                Destroy(_seeItem.gameObject);
            }

            _seeItem = item;
            SeeType = item.SeeType;
        }
        else if (!item.IsSee)
        {
            if (_doItem != null)
            {
                //Refund the cost 
                GameManager.Instance.AddToCoins(_doItem.Cost);

                //destroy the item in the See Do Block
                Destroy(_doItem.gameObject);
            }

            _doItem = item;
            DoType = item.DoType;
        }

        GameManager.Instance.CheckRunButton();
    }

    public void AddNextSeeDo() 
    {
        Instantiate(gameObject, transform.position + new Vector3(0, -65 * _canvas.scaleFactor, 0), Quaternion.identity, transform.parent.transform).transform.SetAsFirstSibling();
        addBtn.gameObject.SetActive(false);
        remBtn.gameObject.SetActive(false);
    }

    public void RemLastSeeDo()
    {
        SeeDo prev = GameManager.Instance.Blocks[GameManager.Instance.Blocks.Count-2];
        prev.ShowButtons();
        
        if(_seeItem != null){ Destroy(_seeItem.gameObject);}
        if(_doItem != null) { Destroy(_doItem.gameObject); }
        
        Destroy(gameObject);
    }

    public void ShowButtons(){

        if(GameManager.Instance.Blocks.Count != 2) 
        {
            remBtn.gameObject.SetActive(true);  //make sure this isnt for case = 0
        }
        addBtn.gameObject.SetActive(true);
    }

/// <summary>
/// Return if the individual SeeDo block is filled
/// </summary>
/// <returns>Boolean</returns>
    public bool IsFilled() 
    {
        return  _seeItem != null && _doItem != null;

    }
}
