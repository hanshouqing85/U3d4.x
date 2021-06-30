using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class ItemItem
	{
		public int ItemID;//xml中的Key，代码中未用
		public int ID;//代码中用作Key
		public int maxcount;
		public int type;
		public string txt;
		public void SetFieldValue(int index,string value)
		{
			switch(index)    
			{         
			case  0:ItemID=int.Parse(value);break;
			case  1:ID=int.Parse(value);break;
			case  2:maxcount=int.Parse(value);break;
			case  3:type=int.Parse(value);break;
			case  4:txt=value;break;
			default :break;
			}
		}
	}



	public class Itemconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
			string[] strArr = {"ID","maxcount","type","txt"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Itemconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int,ItemItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				ItemItem d = new ItemItem();
				d.ItemID =int.Parse(_ValueItem.GetAttribute("Key"));
				
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
					if ("txt" == _item.GetAttribute("Key"))
					{
						d.SetFieldValue(i+1,_item.InnerText);
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.ItemID,_item.GetAttribute("Key"),_item.InnerText));
					}
					else
					{
						d.SetFieldValue(i+1,_item.GetAttribute("Value"));
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.ItemID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
					}
				}
				//m_data.Add(d.ItemID, d);
				m_data.Add(d.ID, d);
			}
			//Debuger.Log("所有物品ID数目" + m_data.Count);
			foreach (KeyValuePair<int, ItemItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4},{5}",kvp.Key, kvp.Value.ItemID, kvp.Value.ID,kvp.Value.maxcount, kvp.Value.type, kvp.Value.txt));
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public Dictionary<int, ItemItem> m_data;
	}
}
