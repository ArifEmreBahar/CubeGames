using UnityEngine;
using UnityEngine.UI;

public class Inputs : MonoBehaviour
{
    private GameManager gameManager;
    private Canvas canvas;
    private Camera mainCamera;

    private RectTransform rectImage;
    private Rect selectionDragRect; 
    private Color32 dragImageColor = new Color32(60, 250, 110, 50);

    private Vector2 dragStartPosition;
    private Vector2 dragEndPosition;

    private int layerMaskClickable = 1 << 8;
    private int layerMaskGround = 1 << 9;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mainCamera = Camera.main;

        //Creating an image that will appear on the screen
        GameObject dragImage = new GameObject("DragImage");
        dragImage.transform.parent = transform;
        Image createdDragImage = dragImage.AddComponent<Image>();
        createdDragImage.raycastTarget = false;
        createdDragImage.color = dragImageColor;
        rectImage = dragImage.GetComponent<RectTransform>();

        dragStartPosition = Vector3.zero;
        dragEndPosition = Vector3.zero;
        DrawDragImage();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        //Mouse left clicks for Targets
        if (Input.GetButtonDown("LeftMouse"))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskClickable))
            {
                gameManager.LeftClickSelect(hit.collider.gameObject);
            }
            else
            {
                gameManager.DeselectAllTargets();
            }
        }

        if (Input.GetButton("LeftMouse"))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskGround))
            {
                gameManager.SetTarget(hit.point);
            }
        }

        //Mouse right clicks for Targets
        if (Input.GetButtonDown("RightMouse"))
        {
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskClickable))
            {
                if (Input.GetButton("LeftShift"))
                {
                    gameManager.ShiftClickSelect(hit.collider.gameObject);
                }
                else
                {
                    gameManager.RightClickSelect(hit.collider.gameObject);
                }
            }
            else
            {
                if (!Input.GetButton("LeftShift"))
                {
                    gameManager.DeselectAllUnits();
                }
            }
        }

        //Drag Selections
        if (Input.GetButtonDown("RightMouse"))
        {
            dragStartPosition = Input.mousePosition;
        }

        if (Input.GetButton("RightMouse"))
        {
            dragEndPosition = Input.mousePosition;
            DrawDragImage();
            DrawDragSelection();
        }

        if (Input.GetButtonUp("RightMouse"))
        {
            gameManager.ShiftDragSelect(selectionDragRect);
            dragStartPosition = Vector3.zero;
            dragEndPosition = Vector3.zero;
            DrawDragImage();
        }
    }


    //Draw drag image by positions
    private void DrawDragImage()
    {
        Vector2 rectStart = dragStartPosition;
        Vector2 rectEnd = dragEndPosition;

        Vector2 rectCenter = (rectStart + rectEnd) / 2;
        rectImage.position = rectCenter;

        Vector2 rectSize = new Vector2(Mathf.Abs(rectStart.x - rectEnd.x), Mathf.Abs(rectStart.y - rectEnd.y));
        rectImage.sizeDelta = rectSize;
    }

    //Redefine the min and max points of the selection to the last movement of the mouse
    private void DrawDragSelection()
    {
        if (Input.mousePosition.x < dragStartPosition.x)
        {
            selectionDragRect.xMin = Input.mousePosition.x;
            selectionDragRect.xMax = dragStartPosition.x;
        }
        else
        {
            selectionDragRect.xMin = dragStartPosition.x;
            selectionDragRect.xMax = Input.mousePosition.x;
            
        }

        if (Input.mousePosition.y < dragStartPosition.y)
        {
            selectionDragRect.yMin = Input.mousePosition.y;
            selectionDragRect.yMax = dragStartPosition.y;

        }
        else
        {
            selectionDragRect.yMin = dragStartPosition.y;
            selectionDragRect.yMax = Input.mousePosition.y;    
        }
    }
}
