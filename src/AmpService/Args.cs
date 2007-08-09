using ExecuteTemplate;
using System;


namespace AmpService
{
	/// <summary>
	/// Аргументы для передачи в делегат, генерирующий SQL запрос
	/// </summary>
	[Serializable]
	public class FirmNameArgs : ExecuteArgs
	{
		private string[] _firmNames;

		public string[] FirmNames
		{
			get { return _firmNames; }
			set { _firmNames = value; }
		}

		protected FirmNameArgs()
		{ }

		public FirmNameArgs(string[] firmNames)
			: base()
		{
			_firmNames = firmNames;
		}
	}

	[Serializable]
	public class GetOrdersArgs : ExecuteArgs
	{
		private string[] _orderID;
		private int _priceCode;

		public string[] OrderID
		{
			get { return _orderID; }
			set { _orderID = value; }
		}

		protected GetOrdersArgs()
		{ }
		
		public int PriceCode
		{
			get { return _priceCode; }
			set { _priceCode = value; }
		}
		
		public GetOrdersArgs(string[] orderID, int priceCode)
		{
			_orderID = orderID;
			_priceCode = priceCode;
		}
	}

	[Serializable]
	public class GetPricesArgs : ExecuteArgs
	{
		private int _count;
		private int _offset;
		private bool _onlyLeader;
		private bool _newEar;
		private string[] _rangeField;
		private string[] _rangeValue;
		private string[] _sortField;
		private string[] _sortDirection;

		public int Count
		{
			get { return _count; }
			set { _count = value; }
		}

		public int Offset
		{
			get { return _offset; }
			set { _offset = value; }
		}

		public bool OnlyLeader
		{
			get { return _onlyLeader; }
			set { _onlyLeader = value; }
		}

		public bool NewEar
		{
			get { return _newEar; }
			set { _newEar = value; }
		}

		public string[] RangeField
		{
			get { return _rangeField; }
			set { _rangeField = value; }
		}

		public string[] RangeValue
		{
			get { return _rangeValue; }
			set { _rangeValue = value; }
		}

		public string[] SortField
		{
			get { return _sortField; }
			set { _sortField = value; }
		}

		public string[] SortDirection
		{
			get { return _sortDirection; }
			set { _sortDirection = value; }
		}

		protected GetPricesArgs()
		{ }

		public GetPricesArgs(bool onlyLeader, bool newEar, string[] rangeField, string[] rangeValue, string[] sortField, string[] sortDirection, int count, int offset)
			: base()
		{
			_count = count;
			_offset = offset;
			_onlyLeader = onlyLeader;
			_newEar = newEar;
			_rangeField = rangeField;
			_rangeValue = rangeValue;
			_sortField = sortField;
			_sortDirection = sortDirection;
		}
	}

	[Serializable]
	public class PostOrderArgs : ExecuteArgs
	{
		private int[] _quantities;
		private string[] _messages;
		private int[] _synonymCodes;
		private int[] _synonymFirmCrCodes;
		private bool[] _junks;
		private long[] _coreIDs;

		public long[] CoreIDs
		{
			get { return _coreIDs; }
			set { _coreIDs = value; }
		}
		
		public int[] Quantities
		{
			get { return _quantities; }
			set { _quantities = value; }
		}
		
		public string[] Messages
		{
			get { return _messages; }
			set { _messages = value; }
		}

		public int[] SynonymCodes
		{
			get { return _synonymCodes; }
			set { _synonymCodes = value; }
		}

		public int[] SynonymFirmCrCodes
		{
			get { return _synonymFirmCrCodes; }
			set { _synonymFirmCrCodes = value; }
		}

		public bool[] Junks
		{
			get { return _junks; }
			set { _junks = value; }
		}

		protected PostOrderArgs()
		{ }

		public PostOrderArgs(long[] coreIDs, int[] quantities, string[] messages, int[] synonymCodes, int[] synonymFirmCrCodes, bool[] junks)
		{
			_coreIDs = coreIDs;
			_quantities = quantities;
			_messages = messages;
			_synonymCodes = synonymCodes;
			_synonymFirmCrCodes = synonymFirmCrCodes;
			_junks = junks;
		}
	}

}