using UnityEngine;
using UnityEngine.UI;
namespace RL
{
    public class TestListItem1 : ListItem
    {
        public Image Bg;
        public Text IndexText;
        public Button btn;
        public void SetData(int index)
        {
            IndexText.text = $"Index:{index}";
            int val1 = ((index + 1) % 255);
            int val2 = ((index + 1) % 128);
            Bg.color = new Color32((byte)val1, 255, (byte)val2, 255);
        }

        public override void OnInit()
        {
            RLLog.Log("---TestListItem1:"+ "OnInit初始化调用");
            btn.AddListener(OnBtn);
        }

        public override void OnRemove()
        {
            RLLog.Log("---TestListItem1:" + "OnRemove移除调用");
            btn.RemoveListener(OnBtn);
        }

        private void OnBtn()
        {
            RLLog.Log("---:"+this.ItemIndex+"，被点击了。");
        }
    }
}
