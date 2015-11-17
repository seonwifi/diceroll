using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;



public   class RandomIndex<T>
{  
	public   class IndexData<T>
	{
		public uint ratio = 0;
		public T   dataValue = default(T);

		public  IndexData(uint r, T d)
		{
			ratio = r;
			dataValue = d;
		}
	}
 

	List<IndexData<T>> m_dataList = new List<IndexData<T>> ();

	public void AddNewValue(uint ratio, T TValue)
	{
		m_dataList.Add (new IndexData<T>(ratio, TValue));
	}

	public T Select()
	{
		if(m_dataList == null)
		{
			Debug.LogError("if(values == null)");
			return default(T);
		}
		
		if(m_dataList.Count == 0)
		{
			Debug.LogError("if(values.Length == 0)");
			return default(T);
		}
		
		int valuesLength = m_dataList.Count;
		
		uint totalValue = 0;
		
		for(int i = 0; i < valuesLength; ++i)
		{
			totalValue += m_dataList[i].ratio;
		}
		
		uint selectValue = (uint)Random.Range ( 0, (int)totalValue);
		
		totalValue = 0;
		 
		IndexData<T> selectData = null;

		for(int i = 0; i < valuesLength; ++i)
		{
			uint currentTotalValue = totalValue + m_dataList[i].ratio;

			if(totalValue <= selectValue && selectValue < currentTotalValue)
			{
				selectData = m_dataList[i]; 
				break;
			}
			
			totalValue = currentTotalValue;
		}

		if(selectData == null)
		{
			return default(T);
		}

		return selectData.dataValue;
	}
}











