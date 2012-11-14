using System;
using System.Collections.Generic;

namespace NumberCruncher
{
	public class NCNode
	{
		static int _totalNodes = 0;
		public int TotalNodes
		{
			get { return _totalNodes; }
		}

		NCNode _parentNode = null;
		List<NCNode> _nodes = new List<NCNode>();
		double _currentNumber = 0;
		double [] _selectedNumbers = null;

		public NCNode(double currentNumber, double [] selectedNumbers, NCNode parentNode)
		{
			if (parentNode == null)
				_totalNodes = 0;

			_currentNumber = currentNumber;
			_selectedNumbers = selectedNumbers;
			_parentNode = parentNode;
			++_totalNodes;
		}

		public void Process()
		{
			if (_selectedNumbers == null)
				return;

			for (int i = 0; i < _selectedNumbers.Length; ++i)
			{
				double [] newNumbers = null;
				if (_selectedNumbers.Length > 0)
				{
					newNumbers = new double[_selectedNumbers.Length - 1];
					Array.Copy(_selectedNumbers, newNumbers, i); 				
					Array.Copy(_selectedNumbers, i + 1, newNumbers, i, _selectedNumbers.Length - i - 1);
				}
				NCNode addNode = new NCNode(_currentNumber + _selectedNumbers[i], newNumbers, this);
				NCNode subNode = new NCNode(_currentNumber - _selectedNumbers[i], newNumbers, this);
				NCNode mulNode = new NCNode(_currentNumber * _selectedNumbers[i], newNumbers, this);
				NCNode divNode = new NCNode(_currentNumber / _selectedNumbers[i], newNumbers, this);

				_nodes.Add(addNode);
				_nodes.Add(subNode);
				_nodes.Add(mulNode);
				_nodes.Add(divNode);
			}

			foreach (NCNode node in _nodes)
			{
				node.Process();
			}
		}
	}
}

