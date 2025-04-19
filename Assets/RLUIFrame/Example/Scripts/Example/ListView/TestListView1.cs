using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace RL
{
    public class TestListView1 : MonoBehaviour
    {
        public InputField FreshNumInput;
        public InputField IndexNumInput;
        public Button FreshButton;
        public ListView ListView1;
    // Start is called before the first frame update
        void Start()
        {
            ListView1.OnInitListItem(InitListItem);
            ListView1.OnFreshListItem(FreshListItem);
            ListView1.SetCount(5);
            FreshButton.onClick.AddListener(OnFreshButton);
        }

        private void InitListItem(int index,ListItem item)
        {
            TestListItem1 testListItem1 = item as TestListItem1;
            testListItem1.OnInit();
        }

        private void FreshListItem(int index, ListItem item)
        {
            TestListItem1 testListItem1 = item as TestListItem1;
            testListItem1.SetData(index);
        }

        private void OnFreshButton()
        {
            string numStr = FreshNumInput.text;
            string indexStr = IndexNumInput.text;
            int num = int.Parse(numStr);
            int index = int.Parse(indexStr);
            ListView1.SetCount(num);
            ListView1.SetIndex(index);
        }
    }
}