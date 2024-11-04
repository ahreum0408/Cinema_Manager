using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IOpenTarget{
    public bool IsOpen { get; set; }
    public void ActiveObj(bool active);
}
