using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public int Cost = 0;
    public See SeeType;
    public Do DoType;
    public bool IsSee;
    private SeeDo _block;

    private bool _isSet = false;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Canvas _canvas;
    
    [SerializeField] private int _range = 20;
    private Vector3 _startVec;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _startVec = transform.position;

        //Canvas variables change so drag is posible
        _canvasGroup.alpha = .6f;
        _canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        bool hitCollider = false;

        //Check hit colliders so we know what to do
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, _range);

        if (hitColliders != null)
        {
            foreach (Collider2D hit in hitColliders)
            {
                if (hit.gameObject.CompareTag(tag) && !_isSet)
                {
                    //Make sure we can pay the price
                    if (GameManager.Instance.CanBuy(Cost))
                    {
                        //Pay the cost
                        GameManager.Instance.AddToCoins(-Cost);

                        //Set ourselves in SeeDo
                        _block = hit.gameObject.GetComponentInParent<SeeDo>();
                        _block.SetItem(this);

                        //Tell it that we have set it to a see or do block
                        _isSet = true;
                        hitCollider = true;

                        //Refill the shop with the same type of object
                        Instantiate(gameObject, _startVec, Quaternion.identity, transform.parent.transform);

                        //Change the position
                        transform.position = hit.transform.position;
                        break;
                    }
                    else
                    {
                        //TODO: show a pop up text says cant buy
                        Debug.Log("Too expensive to buy");
                    }
                    
                }
            }
        }

        if (!_isSet) // If we moved to correct place no need to go to start
        {
            transform.position = _startVec; //Could add a nicer tween
        }
        
        if(!hitCollider && _isSet) //moved from the block to nothing
        {
            //Refund our cost
            GameManager.Instance.AddToCoins(Cost);
            _block.UnsetItem(this);
            Destroy(gameObject);
        }

        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
