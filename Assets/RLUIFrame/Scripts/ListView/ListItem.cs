using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RL
{
    public class ListItem : MonoBehaviour
    {
        private RectTransform itemRT;
        //注意这个Index不能重新设置值
        public int ItemIndex { get; private set; }
        public void SetItemIndex(int index)
        {
            this.ItemIndex = index;
        }
        //初始化
        public virtual void OnInit() { }
        //移除
        public virtual void OnRemove() { }
        private void Awake()
        {
            itemRT = (RectTransform)this.gameObject.transform;
            OnInit();
        }

        public void SetPositionY(float y)
        { 
            itemRT.anchoredPosition = new Vector2(itemRT.anchoredPosition.x, y);
        } 

        public float GetHeight()
        { 
            return itemRT.rect.height;
        }

        public Vector2 GetPivot()
        { 
            return itemRT.pivot;
        }
    }
}