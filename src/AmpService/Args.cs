using ExecuteTemplate;


namespace AMPWebService
{
	/// <summary>
	/// Аргументы для передачи в делегат, генерирующий SQL запрос
	/// </summary>
	internal class FirmNameArgs : ExecuteArgs
	{

		private string[] _firmNames;

		public string[] FirmNames
		{
			get { return _firmNames; }
			set { _firmNames = value; }
		}

		public FirmNameArgs(string[] firmNames)
			: base()
		{
			_firmNames = firmNames;
		}
	}
	
	internal class GetOrdersArgs : ExecuteArgs
	{
		private string _orderID;
		private int _priceCode;

		public string OrderID
		{
			get { return _orderID; }
			set { _orderID = value; }
		}
		
		public int PriceCode
		{
			get { return _priceCode; }
			set { _priceCode = value; }
		}
		
		public GetOrdersArgs(string orderID, int priceCode)
		{
			_orderID = orderID;
			_priceCode = priceCode;
		}
	}

	internal class GetPricesArgs : ExecuteArgs
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
}