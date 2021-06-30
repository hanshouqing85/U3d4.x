using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace Bjoy
{
	public class OnagerItem
	{
		public int OnagerID;//xml中的Key，代码中未用
		public int Bolts;
		public int Ctime;
		public int ID;//弩炮ID，代码中用作Key
		public int squamacount;
		public int squamaid;
		public int validlevel;
		public void SetFieldValue(int index,int value)
		{
			switch(index)    
			{         
			case  0:OnagerID=value;break;
			case  1:Bolts=value;break;      
			case  2:Ctime=value;break;
			case  3:ID=value;break;
			case  4:squamacount=value;break;
			case  5:squamaid=value;break;
			case  6:validlevel=value;break;
			default :break;    
			}
		}
	}



	public class Onagerconfig : MonoBehaviour {

		// Use this for initialization
		void Start () {
			string[] strArr = {"Bolts","Ctime","ID","squamacount","squamaid","validlevel"};
			int ArrSize = strArr.Length;
			//Debuger.Log("ArrSize=" + ArrSize);
			string localUrl = Application.dataPath + "/config/Onagerconfig.IntCfg";
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(localUrl);
			m_data = new Dictionary<int, OnagerItem>();
			
			//所有ValueItem节点
			XmlNode ValueItems = xmlDoc.SelectSingleNode("ValueItem");
			
			foreach (XmlNode ValueItem in ValueItems)
			{
				XmlElement _ValueItem = (XmlElement)ValueItem;
				
				OnagerItem d = new OnagerItem();
				d.OnagerID =int.Parse(_ValueItem.GetAttribute("Key"));
				
				for(int i=0;i<ArrSize;i++)
				{
					XmlElement _item=(XmlElement)_ValueItem.ChildNodes.Item(i);
					//Debuger.Log(string.Format("Key = {0},{1}, Value = {2}",d.OnagerID,_item.GetAttribute("Key"),_item.GetAttribute("Value")));
					d.SetFieldValue(i+1,int.Parse(_item.GetAttribute("Value")));
				}
				//m_data.Add(d.OnagerID, d);
				m_data.Add(d.ID, d);
			}
			//Debuger.Log("所有弩炮ID数目" + m_data.Count);
			foreach (KeyValuePair<int, OnagerItem> kvp in m_data)
			{
				//Debuger.Log(string.Format("Key = {0}, Value = {1},{2},{3},{4},{5},{6},{7}",kvp.Key, kvp.Value.OnagerID, kvp.Value.Bolts,kvp.Value.Ctime,kvp.Value.ID, kvp.Value.squamacount, kvp.Value.squamaid,kvp.Value.validlevel));
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		public Dictionary<int, OnagerItem> m_data;
	}
}
