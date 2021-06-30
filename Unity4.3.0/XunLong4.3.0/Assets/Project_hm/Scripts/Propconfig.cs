using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class PropItem
	{
		public int PropID;
		public int prize;
		public void SetFieldValue(int index,int value)
		{
			switch(index)    
			{         
			case  0:PropID=value;break;
			case  1:prize=value;break;
			default :break;  
			}
		}
	}

	public class Propconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
			string[] strArr = {"prize"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Propconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int, PropItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				PropItem d = new PropItem();
				d.PropID =int.Parse(_ValueItem.GetAttribute("Key"));
				
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
					//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.PropID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
					d.SetFieldValue(i+1,int.Parse(_item.GetAttribute("Value")));
				}
				m_data.Add(d.PropID, d);			
			}
			//Debuger.Log("所有道具ID数目" + m_data.Count);
			foreach (KeyValuePair<int, PropItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2}",kvp.Key, kvp.Value.PropID, kvp.Value.prize));
			}		
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public Dictionary<int, PropItem> m_data;
	}
}
