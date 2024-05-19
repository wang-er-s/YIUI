using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ET
{
    [RequireComponent(typeof(Renderer))]
    [ExecuteInEditMode]
    public class OrderInLayer : MonoBehaviour
    {
        private int order = int.MinValue;

        public int Order
        {
            get
            {
                if (this.order == int.MinValue)
                {
                    this.order = this.GetComponent<Renderer>().sortingOrder;
                }

                return this.order;
            }
        }

        public void SetOrder(int order)
        {
            this.GetComponent<Renderer>().sortingOrder = order;
        }
    }
}