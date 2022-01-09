using UnityEngine;

public class Target : MonoBehaviour, ISelectable
{
    [HideInInspector] public bool selected;
    [HideInInspector] public Vector3 target;
    private GameObject selectRing;
    private Vector3 targetOffset;

    private void Awake()
    {
        selectRing = transform.Find("SelectRing").gameObject;
    }

    private void Update()
    {
        //Actions to take when Target is selected
        if (selected)
        {
            selectRing.SetActive(true);

            transform.position = new Vector3(target.x, transform.position.y, target.z);
        }
        else
        {
            selectRing.SetActive(false);
        }
    }

    //Toggle select status of unit
    public void OnSelected(bool isSelect)
    {
        selected = isSelect;
    }

    //Set target of target
    public void SetTarget(Vector3 target)
    {
        if (this.target != target)
        {
            this.target = target;
        }
    }
}
