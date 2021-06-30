using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class CatchItem
	{
		public int KeyID;
		public int ID;
		public int arrow;
		public int value;
		public void SetFieldValue(int index,int value)
		{
			switch(index)    
			{         
			case  0:KeyID=value;break;
			case  1:ID=value;break;
			case  2:arrow=value;break;
			case  3:this.value=value;break;
			default :break;
			}
		}
	}

	public class Catchconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
			string[] strArr = {"ID","arrow","value"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Catchconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int,CatchItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				CatchItem d = new CatchItem();
				d.KeyID =int.Parse(_ValueItem.GetAttribute("Key"));
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
				//	Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.KeyID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
					d.SetFieldValue(i+1,int.Parse(_item.GetAttribute("Value")));
				}
				m_data.Add(d.KeyID, d);			
			}
			//Debuger.Log("弩箭捕获效用表所有龙ID+箭ID数目" + m_data.Count);
			foreach (KeyValuePair<int,CatchItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4}",kvp.Key, kvp.Value.KeyID, kvp.Value.ID, kvp.Value.arrow, kvp.Value.value));
			}		
		}
	

		public int GetValue (int ID, int arrow)
		{
			foreach (KeyValuePair<int,CatchItem> kvp in m_data) {
				if (kvp.Value.ID == ID && kvp.Value.arrow == arrow)
					return kvp.Value.value;
			}
			return 0;
		}

		public Dictionary<int, CatchItem> m_data;
	}
}
