using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Tetris.IO
{
	// Serializacja   - klasa => string
	// Deserializacja - string => klasa

	public static class Serializer
	{
		public static string Serialize<T>(T obj)
		{
			var sb = new StringBuilder();

			if (typeof(T).GetInterface(nameof(IEnumerable)) != null)
			{
				IEnumerable set = (IEnumerable)obj;

				int c = 0;
				foreach (object i in set)
				{
					if (i is null)
						continue;

					sb.AppendLine(string.Concat("[", i.GetType().Name, " ", c++, "]"));
					sb.AppendLine(Serialize(i));
				}
			}
			else
			{
				foreach (var i in obj
					.GetType()
					.GetProperties()
					.Where(prop => prop.PropertyType.IsValueType))
				{
					sb.AppendLine(i.Name + "=" + i.GetValue(obj)?.ToString());
				}
			}

			return sb.ToString();
		}

		public static T Deserialize<T>(string text)
		{


			return default;
		}
	}
}
