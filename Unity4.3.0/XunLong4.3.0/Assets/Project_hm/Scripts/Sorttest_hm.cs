using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sorttest_hm : MonoBehaviour {
public List <int> Players = new List<int>();
	public List <int> Playerssub = new List<int>();
public int temp,max,min,index,tempid;
public List <int> Scoresid = new List<int>();

	// Use this for initialization
	void Start () {
		Players.Add (45);
		Scoresid.Add (0);
		Players.Add (25);
		Scoresid.Add (1);
		Players.Add (55);
		Scoresid.Add (2);
		Players.Add (35);
		Scoresid.Add (3);
		Players.Add (0);
		Scoresid.Add (4);
		Players.Add (0);
		Scoresid.Add (5);
		Players.Add (0);
		Scoresid.Add (6);
		Players.Add (0);
		Scoresid.Add (7);

		Playerssub.AddRange (Players);
	}
	
	// Update is called once per frame
	void Update () {
	//	Players.Sort ();
		//Players.Reverse ();

		BubbleSort ();
		for (int i=0; i<Players.Count; i++) 
		{
			print ("Players"+"["+i+"]="+Players[i]);
		}

		print ("indexof=" + indexCheck (0));

	}



	public void BubbleSort()
	{
		bool exchange ;
	
		
		for (int i=0; i<Players.Count-1; i++) {
			exchange = false;

			for (int j=Players.Count-2;j>=i;j--)
			
				if (Players[j+1]>Players[j])
				{	
					temp=Players[j+1];
				Players[j+1]=Players[j];
				Players[j]=temp;
					exchange=true;

				tempid=Scoresid[j+1];
				Scoresid[j+1]=Scoresid[j];
				Scoresid[j]=tempid;

					max=Players[j+1];
			   }
				if (!exchange)
					return;
					
			}
		}


	public int indexCheck(int i)
	{

			for (int j=0;j<Playerssub.Count;j++)
			if (Playerssub[j]==Players[i])
			{
				 index=j;
			break;
			}
		return index;

	}

	}





