using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    void Activate(IInput pastInput);
    void Deactivate();
}
