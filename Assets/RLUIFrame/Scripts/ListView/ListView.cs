using System; 
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
namespace RL
{
    public class ListView : MonoBehaviour
    {
        public bool CacheListItem = true;                           //是否缓存，即便界面释放也依然缓存,用于下次快速创建
        public ListItem BaseListItem;                               //复用行
        private ListViewController listViewController;
        private Action<int, ListItem> onInitCall;                   //第一次被初始化调用
        private Action<int, ListItem> onRemoveCall;                 //被移除的时候调用
        private Action<int, ListItem> onFreshCall;                  //每次循环到这个Item调用
        private int count;                                          //设置的最大数量
        private int createCount;                                    //滑动列表创建数量
        private ScrollRect scrollRect;
        private List<ListItem> listItems = new List<ListItem>();
        private Vector2 scrollPosition;                             //每次滑动位置记录0-1f
        private int itemInstanceID;
        private float baseItemHeight;                               //设置行的高度
        private Vector2 baseItemPivot;
        private int itemMoveIndex;                                  //Item滑动到哪一行了
        private int lastScrollItemIndex;                            //上次滑动行的index记录
        float contentHeight;
        float viewportHeight;
        float scrollHeight;
        public void OnInitListItem(Action<int, ListItem> initCall)
        {
            this.onInitCall = initCall;
        }

        public void OnRemoveListItem(Action<int, ListItem> removeCall)
        {
            this.onRemoveCall = removeCall;
        }

        public void OnFreshListItem(Action<int, ListItem> freshCall)
        {
            this.onFreshCall = freshCall;
        }

        //设置数量
        public void SetCount(int count)
        { 
            this.count = count;
            SetContentHeight();
            FreshListView();
        }

        //刷新所有行
        public void FreshAllItem()
        {
            foreach (var listItem in listItems)
            {
                this.onFreshCall?.Invoke(listItem.ItemIndex, listItem);
            }
        }

        //设置下标
        public void SetIndex(int index)
        {
            float scrollPosY = GetScrollPosY(index);
            if (scrollPosY == scrollPosition.y)
                return;
            scrollRect.verticalNormalizedPosition = GetScrollPosY(index);
            Vector2 pos = scrollRect.normalizedPosition;
            OnScrollPositionChanged(pos);
            //设置content滑动位置
            float maxSizeY = (scrollHeight) / 2f;
            float contentPosY = index * baseItemHeight - maxSizeY;
            scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, 0);
            if (contentHeight > viewportHeight)
            {
                if (contentPosY > maxSizeY)
                    contentPosY = maxSizeY;
                scrollRect.content.anchoredPosition = new Vector2(scrollRect.content.anchoredPosition.x, contentPosY);
            }
        }

        //刷新滑动行
        private void FreshListView()
        {
            foreach (var item in listItems)
            {
                this.onRemoveCall?.Invoke(item.ItemIndex, item);
                listViewController.PushListItem(itemInstanceID, item);
            }
            listItems.Clear(); 
            //需要多一个用于备用 
            createCount = (int)(viewportHeight / baseItemHeight) + 1; 
            if(createCount >= this.count)
                createCount = this.count - 1;
            //滑动行开始位置
            float startPosY = contentHeight * scrollRect.content.pivot.y;
            float pivotY = baseItemPivot.y;
            for (int i = createCount; i >= 0 ;i--)
            {
                int index = createCount - i;
                ListItem listItem = listViewController.GetListItem(itemInstanceID, BaseListItem); 
                listItem.gameObject.SetActive(true);
                listItem.transform.SetParent(scrollRect.content, false);
                listItem.OnInit();
                float itemPosY = GetItemPositionY(index, startPosY, baseItemHeight, pivotY);
                listItem.SetPositionY(itemPosY);
                listItem.name = $"Item{index}";
                listItem.SetItemIndex(index);
                listItems.Add(listItem); 
                this.onInitCall?.Invoke(index, listItem);
                this.onFreshCall?.Invoke(index, listItem); 
            }
            //重新初始化数据
            lastScrollItemIndex = 0;
            itemMoveIndex = 0;
            scrollPosition = Vector2.one * int.MaxValue;
        }

        //设置滑动行高度
        private void SetContentHeight()
        {
            float heigth = this.count * baseItemHeight;
            if (scrollRect.viewport.rect.height >= heigth)
            {
                scrollRect.content.sizeDelta = scrollRect.viewport.sizeDelta;
                contentHeight = scrollRect.content.rect.height;
                viewportHeight = scrollRect.viewport.rect.height;
                scrollHeight = contentHeight - viewportHeight;
                return;
            }
            float needHeight = (scrollRect.content.rect.height - heigth);
            Vector2 contentSize = scrollRect.content.sizeDelta;
            contentSize.y = contentSize.y - needHeight;
            scrollRect.content.sizeDelta = contentSize;
            contentHeight = scrollRect.content.rect.height;
            viewportHeight = scrollRect.viewport.rect.height;
            scrollHeight = contentHeight - viewportHeight;
        }

        private float GetScrollPosY(int index)
        {
            float scrollItemHeight = index * baseItemHeight;
            if (scrollItemHeight > scrollHeight)
                scrollItemHeight = scrollHeight;
            float scrollPosY = 1f - scrollItemHeight / scrollHeight;
            return scrollPosY;
        }

        private void Awake()
        {
            itemInstanceID = BaseListItem.GetInstanceID();
            listViewController = ListViewController.Instance;
            BaseListItem.gameObject.SetActive(false);
            scrollRect = this.gameObject.GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener(OnScrollPositionChanged);
            baseItemHeight = BaseListItem.GetHeight();
            baseItemPivot = BaseListItem.GetPivot();
        }

        //通过参数获取行高
        private float GetItemPositionY(int index,float startPosY, float itemHeight, float pivotY)
        {
            return startPosY - index * itemHeight - itemHeight * pivotY;
        }

        private void OnScrollPositionChanged(Vector2 position)
        {
            if (scrollPosition == position)
                return;
            scrollPosition = position;
            //防止越界
            if (scrollPosition.y > 1f)
                scrollPosition.y = 1f;
            else if(scrollPosition.y < 0f)
                scrollPosition.y = 0f; 
            float y = (contentHeight - viewportHeight) / baseItemHeight;
            float itemCount = 1f / y; 
            //计算滑动的ItemIndex
            int index = (int)((1f - scrollPosition.y) / itemCount);
            if (index < 0)
                return;
            int nextIndex = this.createCount + index;
            if (nextIndex >= this.count)
                nextIndex = this.count - 1;
            if (lastScrollItemIndex == index)
                return;
            float pivotY = baseItemPivot.y;
            float startPosY = contentHeight * scrollRect.content.pivot.y;
            if (lastScrollItemIndex > index)
            { 
                for (int i = lastScrollItemIndex - 1; i >= index; i--)
                {
                    itemMoveIndex -= 1;
                    float itemPosY = GetItemPositionY(i, startPosY, baseItemHeight, pivotY);
                    ListItem listItem = listItems[itemMoveIndex % listItems.Count];
                    listItem.SetPositionY(itemPosY);
                    listItem.SetItemIndex(i);
                    this.onFreshCall?.Invoke(i, listItem); 
                }
            }
            else
            {
                for (int i = lastScrollItemIndex + 1; i <= index; i++)
                {
                    nextIndex = this.createCount + i;
                    if (nextIndex >= this.count)
                        continue;
                    float itemPosY = GetItemPositionY(nextIndex, startPosY, baseItemHeight, pivotY);
                    ListItem listItem = listItems[itemMoveIndex % listItems.Count];
                    listItem.SetPositionY(itemPosY);
                    listItem.SetItemIndex(nextIndex);
                    this.onFreshCall?.Invoke(nextIndex, listItem); 
                    itemMoveIndex += 1; 
                }
            }
            lastScrollItemIndex = itemMoveIndex;
        }

        private void OnDestroy()
        {
            itemMoveIndex = 0;
            scrollRect.onValueChanged.RemoveListener(OnScrollPositionChanged);
            if(CacheListItem)
            {
                foreach (var item in listItems)
                    listViewController.PushListItem(itemInstanceID, item);
            }
            listItems.Clear();
            listItems = null;
        }
    }
}