using UnityEngine;
using System.Collections;
[AddComponentMenu("gametest/SuperLongtest01")]
public class SuperLongtest01 :Longtest01 {

	protected override void UpdateMove()
	{
		//前进
		m_transform.Translate (new Vector3(m_StepX,m_StepY,m_StepZ));
	}
}
