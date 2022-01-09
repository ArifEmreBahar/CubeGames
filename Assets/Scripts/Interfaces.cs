using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    void OnSelected(bool isSelect);
    void SetTarget(Vector3 target);
}
 