using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class DragonItem
	{
		public int DragonID;
		public int exp;
		public int length;
		public int loot;
		public int score;
		public int speed;
		public int type;
		public string name;
		public void SetFieldValue(int index,int value)
		{
			switch(index)    
			{         
			case  0:DragonID=value;break;
			case  1:exp=value;break;
			case  2:length=value;break;
			case  3:loot=value;break;
			case  4:score=value;break;
			case  5:speed=value;break;
			case  6:type=value;break;
			default :break;
			}
		}
	}

	public class Dragonconfig : MonoBehaviour {



		// Use this for initialization
		void Awake () {
			string[] strArr = {"exp","length","loot","score","speed","type","name"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Dragonconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int,DragonItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				DragonItem d = new DragonItem();
				d.DragonID =int.Parse(_ValueItem.GetAttribute("Key"));
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
					if ("name" == _item.GetAttribute("Key"))
					{
						d.name=_item.InnerText;
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.DragonID,_item.GetAttribute("Key"),_item.InnerText));
					}
					else
					{
						//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.DragonID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
						d.SetFieldValue(i+1,int.Parse(_item.GetAttribute("Value")));
					}
				}
				m_data.Add(d.DragonID, d);			
			}
			//Debuger.Log("所有龙ID数目" + m_data.Count);
			foreach (KeyValuePair<int,DragonItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4},{5},{6},{7},{8}",kvp.Key, kvp.Value.DragonID, kvp.Value.exp, kvp.Value.length, kvp.Value.loot, kvp.Value.score, kvp.Value.speed, kvp.Value.type, kvp.Value.name));
			}		
			DragonItem  temp=m_data[1];
		//	Debuger.Log ("DragonID="+temp.DragonID);
		//	Debuger.Log ("exp="+temp.exp);
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public Dictionary<int, DragonItem> m_data;
	}
}
