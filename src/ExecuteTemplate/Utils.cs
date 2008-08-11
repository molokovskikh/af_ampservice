using System;
using System.Xml.Serialization;
using System.IO;

namespace ExecuteTemplate
{
	internal class Utils
	{
		public static string FormatException<T>(Exception exception, T arguments, string userName) where T : ExecuteArgs
		{
			var serializer = new XmlSerializer(arguments.GetType());
			using (TextWriter srializedArguments = new StringWriter())
			{
				serializer.Serialize(srializedArguments, arguments);
				return String.Format(@"
���������:{0}
���� �������:{1}
��������� ������:{2}
��������:{3}
��� ������������:{4}
���������:{5}
",
								exception.Message,
								exception.StackTrace,
								arguments.DataAdapter != null && arguments.DataAdapter.SelectCommand != null 
																	? arguments.DataAdapter.SelectCommand.CommandText 
																	: String.Empty,
								exception.Source,
								userName,
								srializedArguments.ToString());
			}
		}
	}
}
