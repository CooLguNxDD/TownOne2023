using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IHasHpBar
{
    public event EventHandler<OnHpChangedEventArgs> OnHpChanged;
    public class OnHpChangedEventArgs : EventArgs{
        public float HpNormalized;
    }

}
