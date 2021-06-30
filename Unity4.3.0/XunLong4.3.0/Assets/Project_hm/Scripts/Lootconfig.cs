using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class LootItem
	{
		public int LootID;
		public int item1;
		public int item2;
		public int item3;
		public int proba1;
		public int proba2;
		public int proba3;
		public string count1;
		public string count2;
		public string count3;
		public void SetFieldValue(int index,string value)
		{
			switch(index)    
			{         
			case  0:LootID=int.Parse(value);break;
			case  1:item1=int.Parse(value);break;
			case  2:item2=int.Parse(value);break;
			case  3:item3=int.Parse(value);break;
			case  4:proba1=int.Parse(value);break;
			case  5:proba2=int.Parse(value);break;
			case  6:proba3=int.Parse(value);break;
			case  7:count1=value;break;
			case  8:count2=value;break;
			case  9:count3=value;break;
			default :break;
			}
		}
	}

	public class Lootconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
			string[] strArr = {"item1","item2","item3","proba1","proba2","proba3","count1","count2","count3"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Lootconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int,LootItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				LootItem d = new LootItem();
				d.LootID =int.Parse(_ValueItem.GetAttribute("Key"));
				
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
					if ("vtString" == _item.GetAttribute("ValueType"))
					{
						d.SetFieldValue(i+1,_item.InnerText);
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.LootID,_item.GetAttribute("Key"),_item.InnerText));
					}
					else
					{
						d.SetFieldValue(i+1,_item.GetAttribute("Value"));
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.LootID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
					}
				}
				m_data.Add(d.LootID, d);			
			}
			//Debuger.Log("所有掉落方案ID数目" + m_data.Count);
			foreach (KeyValuePair<int,LootItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4},{5},{6},{7},{8},{9},{10}",kvp.Key, kvp.Value.LootID, kvp.Value.item1, kvp.Value.item2, kvp.Value.item3, kvp.Value.proba1, kvp.Value.proba2, kvp.Value.proba3, kvp.Value.count1, kvp.Value.count2, kvp.Value.count3));
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public Dictionary<int,LootItem> m_data;
	}
}
