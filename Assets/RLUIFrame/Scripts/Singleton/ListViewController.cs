using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RL
{
    public class ListViewController : Singleton<ListViewController>
    {
        private Transform listItemCache;
        private Dictionary<int,Queue<ListItem>> listItemPool = new Dictionary<int,Queue<ListItem>>();
        protected override void Init()
        {
            GameObject listItemCacheObj = new GameObject("[ListItemCache]");
            GameObject.DontDestroyOnLoad(listItemCacheObj);
            listItemCache = listItemCacheObj.transform;
            listItemCache.position = new Vector3(int.MaxValue,int.MaxValue,int.MaxValue);
        }
        public ListItem GetListItem(int id, ListItem baseItem)
        {
            if(listItemPool.TryGetValue(id,out Queue<ListItem> itemPool))
            {
                if(itemPool.Count > 0)
                    return itemPool.Dequeue();
            }
            else
            {
                listItemPool.Add(id, new Queue<ListItem>());
            }
            return GameObject.Instantiate(baseItem);
        }

        public void PushListItem(int id, ListItem item)
        {
            if (listItemPool.TryGetValue(id, out Queue<ListItem> itemPool))
            {
                item.transform.SetParent(listItemCache,false);
                itemPool.Enqueue(item);
            }
        }
    }
}
