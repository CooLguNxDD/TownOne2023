using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitBehavior
{
    // interface for Unit behavior such as attack and seeking
    public void Attack();

    public void SeekEnemy();

    public void StartNav();
}
