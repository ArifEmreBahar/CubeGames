using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<Text> unitPanelText;
    [HideInInspector] public List<GameObject> units;
    public List<GameObject> unitsSelected;
    [HideInInspector] public List<GameObject> targets;
    public List<GameObject> targetsSelected;

    private Camera mainCamera;

    private enum Tags { Unit, Target };

    private int[] unitsSizes = new int[3];

    private void Start()
    {
        mainCamera = Camera.main;

        units.Clear();
        targets.Clear();

        //Fill units on start
        GameObject[] foundedUnits = GameObject.FindGameObjectsWithTag("Unit");

        foreach (GameObject unit in foundedUnits)
        {
            units.Add(unit);
        }
        //Fill targets on start
        GameObject[] foundedTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in foundedTargets)
        {
            targets.Add(target);
        }
    }

    //Actions that will happen when right-click input comes.
    public void RightClickSelect(GameObject objToAdd)
    {
        if (objToAdd.tag == Tags.Unit.ToString())
        {
            DeselectAllUnits();
            unitsSelected.Add(objToAdd);
            objToAdd.GetComponent<ISelectable>().SetTarget(GetTargetCurrentPosition(0, unitsSelected.IndexOf(objToAdd)));
            objToAdd.GetComponent<ISelectable>().OnSelected(true);
            UpdateUnitPanel();
        }
    }

    //Actions that will happen when left-click input comes.
    public void LeftClickSelect(GameObject objToAdd)
    {
        if (objToAdd.tag == Tags.Target.ToString())
        {
            DeselectAllTargets();
            targetsSelected.Add(objToAdd);
            objToAdd.GetComponent<ISelectable>().OnSelected(true);
             
            foreach (GameObject unit in unitsSelected)
            {
                unit.GetComponent<ISelectable>().SetTarget(objToAdd.transform.position);
            }
        }
        else
        {
            DeselectAllTargets();
        }
    }

    //Unit target setter
    public void SetTarget(Vector3 target)
    {
        if (targetsSelected.Count != 0)
        {
            targetsSelected[0].GetComponent<ISelectable>().SetTarget(target);

            foreach (GameObject unit in unitsSelected)
            {
                unit.GetComponent<ISelectable>().SetTarget(GetTargetCurrentPosition(0, unitsSelected.IndexOf(unit)));
            }
        }
    }

    //Returns selected targets current positions
    private Vector3 GetTargetCurrentPosition(int targetId, int unitId)
    {
        if (targetsSelected.Count != 0)
        {
            return targetsSelected[targetId].transform.position;
        }
        else
        {
            return unitsSelected[unitId].transform.position;         
        }
    }

    //Actions that will happen when shift-click input comes.
    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (unitToAdd.tag == Tags.Unit.ToString())
        {
            if (!unitsSelected.Contains(unitToAdd))
            {
                unitsSelected.Add(unitToAdd);
                unitToAdd.GetComponent<ISelectable>().SetTarget(GetTargetCurrentPosition(0,unitsSelected.IndexOf(unitToAdd)));
                unitToAdd.GetComponent<ISelectable>().OnSelected(true);
                UpdateUnitPanel();
            }
            else
            {
                unitToAdd.GetComponent<ISelectable>().OnSelected(false);
                unitsSelected.Remove(unitToAdd);
                UpdateUnitPanel();
            }
        }
    }

    //Actions that will happen when shift-drag input comes.
    public void ShiftDragSelect(Rect selectionRect)
    {
        foreach (GameObject unit in units)
        {
            if (selectionRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                if (!unitsSelected.Contains(unit))
                {
                    unitsSelected.Add(unit);
                    unit.GetComponent<ISelectable>().SetTarget(GetTargetCurrentPosition(0, unitsSelected.IndexOf(unit)));
                    unit.GetComponent<ISelectable>().OnSelected(true);
                    UpdateUnitPanel();
                }
            }
        }
    }

    //Deselect all units
    public void DeselectAllUnits()
    {
        foreach (GameObject unit in unitsSelected)
        {
            unit.GetComponent<ISelectable>().OnSelected(false);         
        }
        unitsSelected.Clear();
        UpdateUnitPanel();
    }

    //Deselect all targets
    public void DeselectAllTargets()
    {
        foreach (GameObject target in targets)
        {
            target.GetComponent<ISelectable>().OnSelected(false);
        }
        targetsSelected.Clear();
    }

    //Sets unit's speeds
    public void SetUnitsSpeed(float speed)
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<Unit>().SetSpeed(speed);
        }
    }

    //Sets unit's stop distance
    public void SetUnitsStopDistance(float stopDistance)
    {
        foreach (GameObject unit in units)
        {
            unit.GetComponent<Unit>().SetStopDistance(stopDistance);
        }
    }

    //All unit selection button toggle
    public void ToogleAllUnits(bool isSelect)
    {
        if (isSelect)
        {
            foreach (GameObject unit in units)
            {
                if (!unitsSelected.Contains(unit))
                {
                    unitsSelected.Add(unit);
                    unit.GetComponent<ISelectable>().OnSelected(true);
                }
            }
        }
        else
        {
            DeselectAllUnits();
        }
        UpdateUnitPanel();
    }

    //Updates menu panel
    public void UpdateUnitPanel()
    {
        for (int i = 0; i < unitsSizes.Length; i++)
        {
            unitsSizes[i] = 0;
        }

        foreach (GameObject unit in unitsSelected)
        {
            unitsSizes[unit.GetComponent<Unit>().unitId]++;
        }

        if (unitPanelText.Count == unitsSizes.Length)
        {
            for (int j = 0; j < unitsSizes.Length; j++)
            {
                unitPanelText[j].text = unitsSizes[j].ToString();
            }
        }
        else
        {
            Debug.Log("Texts and Units sizes not macthing!");
        }
    }

    //Opens Link
    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }
}
