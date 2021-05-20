using System;
using System.Collections;
using System.Linq;
using System.Reflection;
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
			string[] lines = text.Split(Environment.NewLine);

			Type type = typeof(T);
			string name = type.Name;

			string header = lines[0];
			if (header.StartsWith('[') &&
				header.EndsWith(']') &&
				header.Trim('[', ']').Split().First() == name)
			{
				T item = Activator.CreateInstance<T>();

				var propValues = lines
					.Skip(1)
					.Select(i => i.Split('='))
					.ToDictionary(i => i[0], i => i[1]);

				foreach (PropertyInfo prop in type.GetProperties())
					if (propValues.TryGetValue(prop.Name, out string value))
					{
						prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
					}
			}
			else throw new InvalidCastException(name);

			return default;
		}
	}
}
