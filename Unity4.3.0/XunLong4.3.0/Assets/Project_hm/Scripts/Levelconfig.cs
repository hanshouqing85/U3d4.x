using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class LevelItem
	{
		public int LevelID;
		public int Dailybolt;
		public int toexp;
		public int upexp;
		public void SetFieldValue(int index,int value)
		{
			switch(index)    
			{         
			case  0:LevelID=value;break;
			case  1:Dailybolt=value;break;
			case  2:toexp=value;break;
			case  3:upexp=value;break;
			default :break;
			}
		}
	}

	public class Levelconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
				string[] strArr = {"Dailybolt","toexp","upexp"};
				int ArrSize = strArr.Length;
				//Debuger.Log("ArrSize=" + ArrSize);
				string localUrl = Application.dataPath + "/config/Levelconfig.IntCfg";
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.Load(localUrl);
				m_data = new Dictionary<int,LevelItem>();
				
				//所有ValueItem节点
				XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
				
				foreach (XmlNode ValueItem in ValueItems)
				{
					XmlElement _ValueItem = (XmlElement)ValueItem;
					
					LevelItem d = new LevelItem();
					d.LevelID =int.Parse(_ValueItem.GetAttribute("Key"));
					
					for(int i=0;i<ArrSize;i++)
					{
						XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.LevelID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
						d.SetFieldValue(i+1,int.Parse(_item.GetAttribute("Value")));
					}
					m_data.Add(d.LevelID, d);			
				}
			    //Debuger.Log("所有等级ID数目" + m_data.Count);
				foreach (KeyValuePair<int,LevelItem> kvp in m_data)
				{
					//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4}",kvp.Key, kvp.Value.LevelID, kvp.Value.Dailybolt, kvp.Value.toexp, kvp.Value.upexp));
				}
		}
		
		// Update is called once per frame
		void Update () {
		
		}
		
		public Dictionary<int, LevelItem> m_data;
	}
}
