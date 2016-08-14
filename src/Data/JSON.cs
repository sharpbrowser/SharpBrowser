using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;
using System.Data;
using System.Xml;


#region JSON

public delegate string Serialize(object data);
public delegate object Deserialize(string data);

public class JSONParameters
{
	/// <summary>
	/// Use the optimized fast Dataset Schema format (dfault = True)
	/// </summary>
	public bool UseOptimizedDatasetSchema = true;
	/// <summary>
	/// Use the fast GUID format (default = True)
	/// </summary>
	public bool UseFastGuid = true;
	/// <summary>
	/// Serialize null values to the output (default = True)
	/// </summary>
	public bool SerializeNullValues = true;
	/// <summary>
	/// Use the UTC date format (default = True)
	/// </summary>
	public bool UseUTCDateTime = true;
	/// <summary>
	/// Show the readonly properties of types in the output (default = False)
	/// </summary>
	public bool ShowReadOnlyProperties = false;
	/// <summary>
	/// Use the $types extension to optimise the output json (default = True)
	/// </summary>
	public bool UsingGlobalTypes = true;
	/// <summary>
	/// 
	/// </summary>
	public bool IgnoreCaseOnDeserialize = false;
	/// <summary>
	/// Anonymous types have read only properties 
	/// </summary>
	public bool EnableAnonymousTypes = false;
	/// <summary>
	/// Enable fastJSON extensions $types, $type, $map (default = True)
	/// </summary>
	public bool UseExtensions = true;
}

public class JSON
{
	public readonly static JSON Instance = new JSON();

	private JSON()
	{
	}
	/// <summary>
	/// You can set these paramters globally for all calls
	/// </summary>
	public JSONParameters Parameters = new JSONParameters();
	private JSONParameters _params;
	// FIX : extensions off should not output $types 
	public string ToJSON(object obj)
	{
		_params = Parameters;
		Reflection.Instance.ShowReadOnlyProperties = _params.ShowReadOnlyProperties;
		return ToJSON(obj, Parameters);
	}

	public string ToJSON(object obj, JSONParameters param)
	{
		_params = param;
		Reflection.Instance.ShowReadOnlyProperties = _params.ShowReadOnlyProperties;
		// FEATURE : enable extensions when you can deserialize anon types
		if (_params.EnableAnonymousTypes) { _params.UseExtensions = false; _params.UsingGlobalTypes = false; }
		return new JSONSerializer(param).ConvertToJSON(obj);
	}

	public object Parse(string json)
	{
		_params = Parameters;
		Reflection.Instance.ShowReadOnlyProperties = _params.ShowReadOnlyProperties;
		return new JsonParser(json, Parameters.IgnoreCaseOnDeserialize).Decode();
	}

	public T ToObject<T>(string json)
	{
		return (T)ToObject(json, typeof(T));
	}

	public object ToObject(string json)
	{
		return ToObject(json, null);
	}

	public object ToObject(string json, Type type)
	{
		_params = Parameters;
		Reflection.Instance.ShowReadOnlyProperties = _params.ShowReadOnlyProperties;
		Dictionary<string, object> ht = new JsonParser(json, Parameters.IgnoreCaseOnDeserialize).Decode() as Dictionary<string, object>;
		if (ht == null) return null;
		return ParseDictionary(ht, null, type, null);
	}

	public string Beautify(string input)
	{
		return Formatter.PrettyPrint(input);
	}

	public object FillObject(object input, string json)
	{
		_params = Parameters;
		Reflection.Instance.ShowReadOnlyProperties = _params.ShowReadOnlyProperties;
		Dictionary<string, object> ht = new JsonParser(json, Parameters.IgnoreCaseOnDeserialize).Decode() as Dictionary<string, object>;
		if (ht == null) return null;
		return ParseDictionary(ht, null, input.GetType(), input);
	}

	public object DeepCopy(object obj)
	{
		return ToObject(ToJSON(obj));
	}

	public T DeepCopy<T>(T obj)
	{
		return ToObject<T>(ToJSON(obj));
	}

	#region [   JSON specific reflection   ]

	private struct myPropInfo
	{
		public bool filled;
		public Type pt;
		public Type bt;
		public Type changeType;
		public bool isDictionary;
		public bool isValueType;
		public bool isGenericType;
		public bool isArray;
		public bool isByteArray;
		public bool isGuid;
		public bool isDataSet;
		public bool isDataTable;
		public bool isHashtable;
		public Reflection.GenericSetter setter;
		public bool isEnum;
		public bool isDateTime;
		public Type[] GenericTypes;
		public bool isInt;
		public bool isLong;
		public bool isString;
		public bool isBool;
		public bool isClass;
		public Reflection.GenericGetter getter;
		public bool isStringDictionary;
		public string Name;
		public bool CanWrite;
	}

	SafeDictionary<string, SafeDictionary<string, myPropInfo>> _propertycache = new SafeDictionary<string, SafeDictionary<string, myPropInfo>>();
	private SafeDictionary<string, myPropInfo> Getproperties(Type type, string typename)
	{
		SafeDictionary<string, myPropInfo> sd = null;
		if (_propertycache.TryGetValue(typename, out sd))
		{
			return sd;
		}
		else
		{
			sd = new SafeDictionary<string, myPropInfo>();
			PropertyInfo[] pr = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
			foreach (PropertyInfo p in pr)
			{
				myPropInfo d = CreateMyProp(p.PropertyType, p.Name);
				d.CanWrite = p.CanWrite;
				d.setter = Reflection.CreateSetMethod(type, p);
				d.getter = Reflection.CreateGetMethod(type, p);
				sd.Add(p.Name, d);
			}
			FieldInfo[] fi = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
			foreach (FieldInfo f in fi)
			{
				myPropInfo d = CreateMyProp(f.FieldType, f.Name);
				d.setter = Reflection.CreateSetField(type, f);
				d.getter = Reflection.CreateGetField(type, f);
				sd.Add(f.Name, d);
			}

			_propertycache.Add(typename, sd);
			return sd;
		}
	}

	private myPropInfo CreateMyProp(Type t, string name)
	{
		myPropInfo d = new myPropInfo();
		d.filled = true;
		d.CanWrite = true;
		d.pt = t;
		d.Name = name;
		d.isDictionary = t.Name.Contains("Dictionary");
		if (d.isDictionary)
			d.GenericTypes = t.GetGenericArguments();
		d.isValueType = t.IsValueType;
		d.isGenericType = t.IsGenericType;
		d.isArray = t.IsArray;
		if (d.isArray)
			d.bt = t.GetElementType();
		if (d.isGenericType)
			d.bt = t.GetGenericArguments()[0];
		d.isByteArray = t == typeof(byte[]);
		d.isGuid = (t == typeof(Guid) || t == typeof(Guid?));
		d.isHashtable = t == typeof(Hashtable);
		d.isDataSet = t == typeof(DataSet);
		d.isDataTable = t == typeof(DataTable);

		d.changeType = GetChangeType(t);
		d.isEnum = t.IsEnum;
		d.isDateTime = t == typeof(DateTime) || t == typeof(DateTime?);
		d.isInt = t == typeof(int) || t == typeof(int?);
		d.isLong = t == typeof(long) || t == typeof(long?);
		d.isString = t == typeof(string);
		d.isBool = t == typeof(bool) || t == typeof(bool?);
		d.isClass = t.IsClass;

		if (d.isDictionary && d.GenericTypes.Length > 0 && d.GenericTypes[0] == typeof(string))
			d.isStringDictionary = true;

		return d;
	}

	private object ChangeType(object value, Type conversionType)
	{
		if (conversionType == typeof(int))
			return (int)CreateLong((string)value);

		else if (conversionType == typeof(long))
			return CreateLong((string)value);

		else if (conversionType == typeof(string))
			return (string)value;

		else if (conversionType == typeof(Guid))
			return CreateGuid((string)value);

		else if (conversionType.IsEnum)
			return CreateEnum(conversionType, (string)value);

		return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
	}
	#endregion

	#region [   p r i v a t e   m e t h o d s   ]
	bool usingglobals = false;
	private object ParseDictionary(Dictionary<string, object> d, Dictionary<string, object> globaltypes, Type type, object input)
	{
		object tn = "";

		if (d.TryGetValue("$types", out tn))
		{
			usingglobals = true;
			globaltypes = new Dictionary<string, object>();
			foreach (var kv in (Dictionary<string, object>)tn)
			{
				globaltypes.Add((string)kv.Value, kv.Key);
			}
		}

		bool found = d.TryGetValue("$type", out tn);
		if (found == false && type == typeof(System.Object))
		{
			return CreateDataset(d, globaltypes);
		}
		
		if (found)
		{
			if (usingglobals)
			{
				object tname = "";
				if (globaltypes.TryGetValue((string)tn, out tname))
					tn = tname;
			}
			type = Reflection.Instance.GetTypeFromCache((string)tn);
		}

		if (type == null)
			throw new Exception("Cannot determine type");

		string typename = type.FullName;
		object o = input;
		if (o == null)
			o = Reflection.Instance.FastCreateInstance(type);
		SafeDictionary<string, myPropInfo> props = Getproperties(type, typename);
		foreach (string n in d.Keys)
		{
			string name = n;
			if (_params.IgnoreCaseOnDeserialize) name = name.ToLower();
			if (name == "$map")
			{
				ProcessMap(o, props, (Dictionary<string, object>)d[name]);
				continue;
			}
			myPropInfo pi;
			if (props.TryGetValue(name, out pi) == false)
				continue;
			if (pi.filled == true)
			{
				object v = d[name];

				if (v != null)
				{
					object oset = null;

					if (pi.isInt)
						oset = (int)CreateLong((string)v);
						
					else if (pi.isLong)
						oset = CreateLong((string)v);

					else if (pi.isString)
						oset = (string)v;

					else if (pi.isBool)
						oset = (bool)v;

					else if (pi.isGenericType && pi.isValueType == false && pi.isDictionary == false)
						oset = CreateGenericList((ArrayList)v, pi.pt, pi.bt, globaltypes);
						
					else if (pi.isByteArray)
						oset = Convert.FromBase64String((string)v);

					else if (pi.isArray && pi.isValueType == false)
						oset = CreateArray((ArrayList)v, pi.pt, pi.bt, globaltypes);
						
					else if (pi.isGuid)
						oset = CreateGuid((string)v);
						
					else if (pi.isDataSet)
						oset = CreateDataset((Dictionary<string, object>)v, globaltypes);

					else if (pi.isDataTable)
						oset = this.CreateDataTable((Dictionary<string, object>)v, globaltypes);

					else if (pi.isStringDictionary)
						oset = CreateStringKeyDictionary((Dictionary<string, object>)v, pi.pt, pi.GenericTypes, globaltypes);

					else if (pi.isDictionary || pi.isHashtable)
						oset = CreateDictionary((ArrayList)v, pi.pt, pi.GenericTypes, globaltypes);

					else if (pi.isEnum)
						oset = CreateEnum(pi.pt, (string)v);

					else if (pi.isDateTime)
						oset = CreateDateTime((string)v);

					else if (pi.isClass && v is Dictionary<string, object>)
						oset = ParseDictionary((Dictionary<string, object>)v, globaltypes, pi.pt, null);

					else if (pi.isValueType)
						oset = ChangeType(v, pi.changeType);

					else if (v is ArrayList)
						oset = CreateArray((ArrayList)v, pi.pt, typeof(object), globaltypes);
					else
						oset = v;

					if (pi.CanWrite)
						o = pi.setter(o, oset);
				}
			}
		}
		return o;
	}

	private void ProcessMap(object obj, SafeDictionary<string, JSON.myPropInfo> props, Dictionary<string, object> dic)
	{
		foreach (KeyValuePair<string, object> kv in dic)
		{
			myPropInfo p = props[kv.Key];
			object o = p.getter(obj);
			Type t = Type.GetType((string)kv.Value);
			if (t == typeof(Guid))
				p.setter(obj, CreateGuid((string)o));
		}
	}

	private long CreateLong(string s)
	{
		long num = 0;
		bool neg = false;
		foreach (char cc in s)
		{
			if (cc == '-')
				neg = true;
			else if (cc == '+')
				neg = false;
			else
			{
				num *= 10;
				num += (int)(cc - '0');
			}
		}

		return neg ? -num : num;
	}

	private object CreateEnum(Type pt, string v)
	{
		// TODO : optimize create enum
		return Enum.Parse(pt, v);
	}

	private Guid CreateGuid(string s)
	{
		if (s.Length > 30)
			return new Guid(s);
		else
			return new Guid(Convert.FromBase64String(s));
	}

	private DateTime CreateDateTime(string value)
	{
		bool utc = false;
		//                   0123456789012345678
		// datetime format = yyyy-MM-dd HH:mm:ss
		int year = (int)CreateLong(value.Substring(0, 4));
		int month = (int)CreateLong(value.Substring(5, 2));
		int day = (int)CreateLong(value.Substring(8, 2));
		int hour = (int)CreateLong(value.Substring(11, 2));
		int min = (int)CreateLong(value.Substring(14, 2));
		int sec = (int)CreateLong(value.Substring(17, 2));

		if (value.EndsWith("Z"))
			utc = true;

		if (_params.UseUTCDateTime == false && utc == false)
			return new DateTime(year, month, day, hour, min, sec);
		else
			return new DateTime(year, month, day, hour, min, sec, DateTimeKind.Utc).ToLocalTime();
	}

	private object CreateArray(ArrayList data, Type pt, Type bt, Dictionary<string, object> globalTypes)
	{
		ArrayList col = new ArrayList();
		// create an array of objects
		foreach (object ob in data)
		{
			if (ob is IDictionary)
				col.Add(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null));
			else
				col.Add(ChangeType(ob, bt));
		}
		return col.ToArray(bt);
	}


	private object CreateGenericList(ArrayList data, Type pt, Type bt, Dictionary<string, object> globalTypes)
	{
		IList col = (IList)Reflection.Instance.FastCreateInstance(pt);
		// create an array of objects
		foreach (object ob in data)
		{
			if (ob is IDictionary)
				col.Add(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null));
			else if (ob is ArrayList)
				col.Add(((ArrayList)ob).ToArray());
			else
				col.Add(ChangeType(ob, bt));
		}
		return col;
	}

	private object CreateStringKeyDictionary(Dictionary<string, object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
	{
		var col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
		Type t1 = null;
		Type t2 = null;
		if (types != null)
		{
			t1 = types[0];
			t2 = types[1];
		}

		foreach (KeyValuePair<string, object> values in reader)
		{
			var key = values.Key;//ChangeType(values.Key, t1);
			object val = null;
			if (values.Value is Dictionary<string, object>)
				val = ParseDictionary((Dictionary<string, object>)values.Value, globalTypes, t2, null);
			else
				val = ChangeType(values.Value, t2);
			col.Add(key, val);
		}

		return col;
	}

	private object CreateDictionary(ArrayList reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
	{
		IDictionary col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
		Type t1 = null;
		Type t2 = null;
		if (types != null)
		{
			t1 = types[0];
			t2 = types[1];
		}

		foreach (Dictionary<string, object> values in reader)
		{
			object key = values["k"];
			object val = values["v"];

			if (key is Dictionary<string, object>)
				key = ParseDictionary((Dictionary<string, object>)key, globalTypes, t1, null);
			else
				key = ChangeType(key, t1);

			if (val is Dictionary<string, object>)
				val = ParseDictionary((Dictionary<string, object>)val, globalTypes, t2, null);
			else
				val = ChangeType(val, t2);

			col.Add(key, val);
		}

		return col;
	}

	private Type GetChangeType(Type conversionType)
	{
		if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
			return conversionType.GetGenericArguments()[0];

		return conversionType;
	}
	
	private DataSet CreateDataset(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
	{
		DataSet ds = new DataSet();
		ds.EnforceConstraints = false;
		ds.BeginInit();

		// read dataset schema here
		ReadSchema(reader, ds, globalTypes);

		foreach (KeyValuePair<string, object> pair in reader)
		{
			if (pair.Key == "$type" || pair.Key == "$schema") continue;

			ArrayList rows = (ArrayList)pair.Value;
			if (rows == null) continue;

			DataTable dt = ds.Tables[pair.Key];
			ReadDataTable(rows, dt);
		}

		ds.EndInit();

		return ds;
	}

	private void ReadSchema(Dictionary<string, object> reader, DataSet ds, Dictionary<string, object> globalTypes)
	{
		var schema = reader["$schema"];

		if (schema is string)
		{
			TextReader tr = new StringReader((string)schema);
			ds.ReadXmlSchema(tr);
		}
		else
		{
			DatasetSchema ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
			ds.DataSetName = ms.Name;
			for (int i = 0; i < ms.Info.Count; i += 3)
			{
				if (ds.Tables.Contains(ms.Info[i]) == false)
					ds.Tables.Add(ms.Info[i]);
				ds.Tables[ms.Info[i]].Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
			}
		}
	}

	private void ReadDataTable(ArrayList rows, DataTable dt)
	{
		dt.BeginInit();
		dt.BeginLoadData();
		List<int> guidcols = new List<int>();
		List<int> datecol = new List<int>();

		foreach (DataColumn c in dt.Columns)
		{
			if (c.DataType == typeof(Guid) || c.DataType == typeof(Guid?))
				guidcols.Add(c.Ordinal);
			if (_params.UseUTCDateTime && (c.DataType == typeof(DateTime) || c.DataType == typeof(DateTime?)))
				datecol.Add(c.Ordinal);
		}

		foreach (ArrayList row in rows)
		{
			object[] v = new object[row.Count];
			row.CopyTo(v, 0);
			foreach (int i in guidcols)
			{
				string s = (string)v[i];
				if (s != null && s.Length < 36)
					v[i] = new Guid(Convert.FromBase64String(s));
			}
			if (_params.UseUTCDateTime)
			{
				foreach (int i in datecol)
				{
					string s = (string)v[i];
					if (s != null)
						v[i] = CreateDateTime(s);
				}
			}
			dt.Rows.Add(v);
		}

		dt.EndLoadData();
		dt.EndInit();
	}

	DataTable CreateDataTable(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
	{
		var dt = new DataTable();

		// read dataset schema here
		var schema = reader["$schema"];

		if (schema is string)
		{
			TextReader tr = new StringReader((string)schema);
			dt.ReadXmlSchema(tr);
		}
		else
		{
			var ms = (DatasetSchema)this.ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
			dt.TableName = ms.Info[0];
			for (int i = 0; i < ms.Info.Count; i += 3)
			{
				dt.Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
			}
		}

		foreach (var pair in reader)
		{
			if (pair.Key == "$type" || pair.Key == "$schema")
				continue;

			var rows = (ArrayList)pair.Value;
			if (rows == null)
				continue;

			if (!dt.TableName.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase))
				continue;

			ReadDataTable(rows, dt);
		}

		return dt;
	}
	
	#endregion
}

#endregion





#region Serializer
internal class JSONSerializer
{
	private readonly StringBuilder _output = new StringBuilder();
	readonly int _MAX_DEPTH = 10;
	int _current_depth = 0;
	private Dictionary<string, int> _globalTypes = new Dictionary<string, int>();
	private JSONParameters _params;

	internal JSONSerializer(JSONParameters param)
	{
		_params = param;
	}

	internal string ConvertToJSON(object obj)
	{
		WriteValue(obj);

		string str = "";
		if (_params.UsingGlobalTypes)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("\"$types\":{");
			bool pendingSeparator = false;
			foreach (var kv in _globalTypes)
			{
				if (pendingSeparator) sb.Append(',');
				pendingSeparator = true;
				sb.Append("\"");
				sb.Append(kv.Key);
				sb.Append("\":\"");
				sb.Append(kv.Value);
				sb.Append("\"");
			}
			sb.Append("},");
			str = _output.Replace("$types$", sb.ToString()).ToString();
		}
		else
			str = _output.ToString();

		return str;
	}

	private void WriteValue(object obj)
	{
		if (obj == null || obj is DBNull)
			_output.Append("null");

		else if (obj is string || obj is char)
			WriteString(obj.ToString());

		else if (obj is Guid)
			WriteGuid((Guid)obj);

		else if (obj is bool)
			_output.Append(((bool)obj) ? "true" : "false"); // conform to standard

		else if (
			obj is int || obj is long || obj is double ||
			obj is decimal || obj is float ||
			obj is byte || obj is short ||
			obj is sbyte || obj is ushort ||
			obj is uint || obj is ulong
		)
			_output.Append(((IConvertible)obj).ToString(NumberFormatInfo.InvariantInfo));

		else if (obj is DateTime)
			WriteDateTime((DateTime)obj);

		else if (obj is IDictionary && obj.GetType().IsGenericType && obj.GetType().GetGenericArguments()[0] == typeof(string))
			WriteStringDictionary((IDictionary)obj);

		else if (obj is IDictionary)
			WriteDictionary((IDictionary)obj);

		else if (obj is DataSet)
			WriteDataset((DataSet)obj);

		else if (obj is DataTable)
			this.WriteDataTable((DataTable)obj);

		else if (obj is byte[])
			WriteBytes((byte[])obj);

		else if (obj is Array || obj is IList || obj is ICollection)
			WriteArray((IEnumerable)obj);

		else if (obj is Enum)
			WriteEnum((Enum)obj);

		else
			WriteObject(obj);
	}

	private void WriteEnum(Enum e)
	{
		// TODO : optimize enum write
		WriteStringFast(e.ToString());
	}

	private void WriteGuid(Guid g)
	{
		if (_params.UseFastGuid == false)
			WriteStringFast(g.ToString());
		else
			WriteBytes(g.ToByteArray());
	}

	private void WriteBytes(byte[] bytes)
	{
		WriteStringFast(Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
	}

	private void WriteDateTime(DateTime dateTime)
	{
		// datetime format standard : yyyy-MM-dd HH:mm:ss
		DateTime dt = dateTime;
		if (_params.UseUTCDateTime)
			dt = dateTime.ToUniversalTime();

		_output.Append("\"");
		_output.Append(dt.Year.ToString("0000", NumberFormatInfo.InvariantInfo));
		_output.Append("-");
		_output.Append(dt.Month.ToString("00", NumberFormatInfo.InvariantInfo));
		_output.Append("-");
		_output.Append(dt.Day.ToString("00", NumberFormatInfo.InvariantInfo));
		_output.Append(" ");
		_output.Append(dt.Hour.ToString("00", NumberFormatInfo.InvariantInfo));
		_output.Append(":");
		_output.Append(dt.Minute.ToString("00", NumberFormatInfo.InvariantInfo));
		_output.Append(":");
		_output.Append(dt.Second.ToString("00", NumberFormatInfo.InvariantInfo));

		if (_params.UseUTCDateTime)
			_output.Append("Z");

		_output.Append("\"");
	}
	private DatasetSchema GetSchema(DataTable ds)
	{
		if (ds == null) return null;

		DatasetSchema m = new DatasetSchema();
		m.Info = new List<string>();
		m.Name = ds.TableName;

		foreach (DataColumn c in ds.Columns)
		{
			m.Info.Add(ds.TableName);
			m.Info.Add(c.ColumnName);
			m.Info.Add(c.DataType.ToString());
		}
		// FEATURE : serialize relations and constraints here

		return m;
	}

	private DatasetSchema GetSchema(DataSet ds)
	{
		if (ds == null) return null;

		DatasetSchema m = new DatasetSchema();
		m.Info = new List<string>();
		m.Name = ds.DataSetName;

		foreach (DataTable t in ds.Tables)
		{
			foreach (DataColumn c in t.Columns)
			{
				m.Info.Add(t.TableName);
				m.Info.Add(c.ColumnName);
				m.Info.Add(c.DataType.ToString());
			}
		}
		// FEATURE : serialize relations and constraints here

		return m;
	}

	private string GetXmlSchema(DataTable dt)
	{
		using (var writer = new StringWriter())
		{
			dt.WriteXmlSchema(writer);
			return dt.ToString();
		}
	}

	private void WriteDataset(DataSet ds)
	{
		_output.Append('{');
		if ( _params.UseExtensions)
		{
			WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)GetSchema(ds) : ds.GetXmlSchema());
			_output.Append(',');
		}
		bool tablesep = false;
		foreach (DataTable table in ds.Tables)
		{
			if (tablesep) _output.Append(",");
			tablesep = true;
			WriteDataTableData(table);
		}
		// end dataset
		_output.Append('}');
	}

	private void WriteDataTableData(DataTable table)
	{
		_output.Append('\"');
		_output.Append(table.TableName);
		_output.Append("\":[");
		DataColumnCollection cols = table.Columns;
		bool rowseparator = false;
		foreach (DataRow row in table.Rows)
		{
			if (rowseparator) _output.Append(",");
			rowseparator = true;
			_output.Append('[');

			bool pendingSeperator = false;
			foreach (DataColumn column in cols)
			{
				if (pendingSeperator) _output.Append(',');
				WriteValue(row[column]);
				pendingSeperator = true;
			}
			_output.Append(']');
		}

		_output.Append(']');
	}

	void WriteDataTable(DataTable dt)
	{
		this._output.Append('{');
		if (_params.UseExtensions)
		{
			this.WritePair("$schema", _params.UseOptimizedDatasetSchema ? (object)this.GetSchema(dt) : this.GetXmlSchema(dt));
			this._output.Append(',');
		}

		WriteDataTableData(dt);

		// end datatable
		this._output.Append('}');
	}
	bool _TypesWritten = false;
	private void WriteObject(object obj)
	{
		if (_params.UsingGlobalTypes == false)
			_output.Append('{');
		else
		{
			if (_TypesWritten== false)
				_output.Append("{$types$");
			else
				_output.Append("{");
		}
		_TypesWritten = true;
		_current_depth++;
		if (_current_depth > _MAX_DEPTH)
			throw new Exception("Serializer encountered maximum depth of " + _MAX_DEPTH);


		Dictionary<string, string> map = new Dictionary<string, string>();
		Type t = obj.GetType();
		bool append = false;
		if (_params.UseExtensions)
		{
			if (_params.UsingGlobalTypes == false)
				WritePairFast("$type", Reflection.Instance.GetTypeAssemblyName(t));
			else
			{
				int dt = 0;
				string ct = Reflection.Instance.GetTypeAssemblyName(t);
				if (_globalTypes.TryGetValue(ct, out dt) == false)
				{
					dt = _globalTypes.Count + 1;
					_globalTypes.Add(ct, dt);
				}
				WritePairFast("$type", dt.ToString());
			}
			append = true;
		}

		List<Getters> g = Reflection.Instance.GetGetters(t);
		foreach (var p in g)
		{
			if (append)
				_output.Append(',');
			object o = p.Getter(obj);
			if ((o == null || o is DBNull) && _params.SerializeNullValues == false)
				append = false;
			else
			{
				WritePair(p.Name, o);
				if (o != null && _params.UseExtensions)
				{
					Type tt = o.GetType();
					if (tt == typeof(System.Object))
						map.Add(p.Name, tt.ToString());
				}
				append = true;
			}
		}
		if (map.Count > 0 && _params.UseExtensions)
		{
			_output.Append(",\"$map\":");
			WriteStringDictionary(map);
		}
		_current_depth--;
		_output.Append('}');
		_current_depth--;

	}

	private void WritePairFast(string name, string value)
	{
		if ((value == null) && _params.SerializeNullValues == false)
			return;
		WriteStringFast(name);

		_output.Append(':');

		WriteStringFast(value);
	}

	private void WritePair(string name, object value)
	{
		if ((value == null || value is DBNull) && _params.SerializeNullValues == false)
			return;
		WriteStringFast(name);

		_output.Append(':');

		WriteValue(value);
	}

	private void WriteArray(IEnumerable array)
	{
		_output.Append('[');

		bool pendingSeperator = false;

		foreach (object obj in array)
		{
			if (pendingSeperator) _output.Append(',');

			WriteValue(obj);

			pendingSeperator = true;
		}
		_output.Append(']');
	}

	private void WriteStringDictionary(IDictionary dic)
	{
		_output.Append('{');

		bool pendingSeparator = false;

		foreach (DictionaryEntry entry in dic)
		{
			if (pendingSeparator) _output.Append(',');

			WritePair((string)entry.Key, entry.Value);

			pendingSeparator = true;
		}
		_output.Append('}');
	}

	private void WriteDictionary(IDictionary dic)
	{
		_output.Append('[');

		bool pendingSeparator = false;

		foreach (DictionaryEntry entry in dic)
		{
			if (pendingSeparator) _output.Append(',');
			_output.Append('{');
			WritePair("k", entry.Key);
			_output.Append(",");
			WritePair("v", entry.Value);
			_output.Append('}');

			pendingSeparator = true;
		}
		_output.Append(']');
	}

	private void WriteStringFast(string s)
	{
		_output.Append('\"');
		_output.Append(s);
		_output.Append('\"');
	}

	private void WriteString(string s)
	{
		_output.Append('\"');

		int runIndex = -1;

		for (var index = 0; index < s.Length; ++index)
		{
			var c = s[index];

			if (c >= ' ' && c < 128 && c != '\"' && c != '\\')
			{
				if (runIndex == -1)
				{
					runIndex = index;
				}

				continue;
			}

			if (runIndex != -1)
			{
				_output.Append(s, runIndex, index - runIndex);
				runIndex = -1;
			}

			switch (c)
			{
				case '\t': _output.Append("\\t"); break;
				case '\r': _output.Append("\\r"); break;
				case '\n': _output.Append("\\n"); break;
				case '"':
				case '\\': _output.Append('\\'); _output.Append(c); break;
				default:
					_output.Append("\\u");
					_output.Append(((int)c).ToString("X4", NumberFormatInfo.InvariantInfo));
					break;
			}
		}

		if (runIndex != -1)
		{
			_output.Append(s, runIndex, s.Length - runIndex);
		}

		_output.Append('\"');
	}
}


#endregion





#region Reflection
internal class Getters
{
	public string Name;
	public Reflection.GenericGetter Getter;
	public Type propertyType;
}

internal class Reflection
{
	public readonly static Reflection Instance = new Reflection();
	private Reflection()
	{
	}

	public bool ShowReadOnlyProperties = false;
	internal delegate object GenericSetter(object target, object value);
	internal delegate object GenericGetter(object obj);
	private delegate object CreateObject();
	
	private SafeDictionary<Type, string> _tyname = new SafeDictionary<Type, string>();
	private SafeDictionary<string, Type> _typecache = new SafeDictionary<string, Type>();
	private SafeDictionary<Type, CreateObject> _constrcache = new SafeDictionary<Type, CreateObject>();
	private SafeDictionary<Type, List<Getters>> _getterscache = new SafeDictionary<Type, List<Getters>>();

	#region [   PROPERTY GET SET   ]
	internal string GetTypeAssemblyName(Type t)
	{
		string val = "";
		if (_tyname.TryGetValue(t, out val))
			return val;
		else
		{
			string s = t.AssemblyQualifiedName;
			_tyname.Add(t, s);
			return s;
		}
	}

	internal Type GetTypeFromCache(string typename)
	{
		Type val = null;
		if (_typecache.TryGetValue(typename, out val))
			return val;
		else
		{
			Type t = Type.GetType(typename);
			_typecache.Add(typename, t);
			return t;
		}
	}

	internal object FastCreateInstance(Type objtype)
	{
		try
		{
			CreateObject c = null;
			if (_constrcache.TryGetValue(objtype, out c))
			{
				return c();
			}
			else
			{
				DynamicMethod dynMethod = new DynamicMethod("_",
					MethodAttributes.Public | MethodAttributes.Static,
					CallingConventions.Standard,
					typeof(object),
					null,
					objtype, false);
				ILGenerator ilGen = dynMethod.GetILGenerator();

				if (objtype.IsClass) 
				{
					ilGen.Emit(OpCodes.Newobj, objtype.GetConstructor(Type.EmptyTypes));
					ilGen.Emit(OpCodes.Ret);
					c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
					_constrcache.Add(objtype, c);
				}
				else // structs
				{
					var lv = ilGen.DeclareLocal(objtype);
					ilGen.Emit(OpCodes.Ldloca_S, lv);
					ilGen.Emit(OpCodes.Initobj, objtype);
					ilGen.Emit(OpCodes.Ldloc_0);
					ilGen.Emit(OpCodes.Box, objtype);
					ilGen.Emit(OpCodes.Ret);
					c = (CreateObject)dynMethod.CreateDelegate(typeof(CreateObject));
					_constrcache.Add(objtype, c);
				}
				return c();
			}
		}
		catch (Exception exc)
		{
			throw new Exception(string.Format("Failed to fast create instance for type '{0}' from assemebly '{1}'",
				objtype.FullName, objtype.AssemblyQualifiedName), exc);
		}
	}

	internal static GenericSetter CreateSetField(Type type, FieldInfo fieldInfo)
	{
		Type[] arguments = new Type[2];
		arguments[0] = arguments[1] = typeof(object);

		DynamicMethod dynamicSet = new DynamicMethod("_", typeof(object), arguments, type, true);
		ILGenerator il = dynamicSet.GetILGenerator();

		if (!type.IsClass) // structs
		{
			var lv = il.DeclareLocal(type); 
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Unbox_Any, type);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Ldloca_S, lv);
			il.Emit(OpCodes.Ldarg_1);
			if (fieldInfo.FieldType.IsClass)
				il.Emit(OpCodes.Castclass, fieldInfo.FieldType);
			else
				il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
			il.Emit(OpCodes.Stfld, fieldInfo);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Box, type);
			il.Emit(OpCodes.Ret);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldarg_1);
			if (fieldInfo.FieldType.IsValueType)
				il.Emit(OpCodes.Unbox_Any, fieldInfo.FieldType);
			il.Emit(OpCodes.Stfld, fieldInfo);
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ret);
		}
		return (GenericSetter)dynamicSet.CreateDelegate(typeof(GenericSetter));
	}

	internal static GenericSetter CreateSetMethod(Type type, PropertyInfo propertyInfo)
	{
		MethodInfo setMethod = propertyInfo.GetSetMethod();
		if (setMethod == null)
			return null;

		Type[] arguments = new Type[2];
		arguments[0] = arguments[1] = typeof(object);

		DynamicMethod setter = new DynamicMethod("_", typeof(object), arguments, type);
		ILGenerator il = setter.GetILGenerator();

		if (!type.IsClass) // structs
		{
			var lv = il.DeclareLocal(type); 
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Unbox_Any, type);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Ldloca_S, lv);
			il.Emit(OpCodes.Ldarg_1);
			if (propertyInfo.PropertyType.IsClass)
				il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
			else
				il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
			il.EmitCall(OpCodes.Call, setMethod, null);
			il.Emit(OpCodes.Ldloc_0);
			il.Emit(OpCodes.Box, type);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
			il.Emit(OpCodes.Ldarg_1);
			if (propertyInfo.PropertyType.IsClass)
				il.Emit(OpCodes.Castclass, propertyInfo.PropertyType);
			else
				il.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
			il.EmitCall(OpCodes.Callvirt, setMethod, null);
			il.Emit(OpCodes.Ldarg_0);
		}

		il.Emit(OpCodes.Ret);

		return (GenericSetter)setter.CreateDelegate(typeof(GenericSetter));
	}

	internal static GenericGetter CreateGetField(Type type, FieldInfo fieldInfo)
	{
		DynamicMethod dynamicGet = new DynamicMethod("_", typeof(object), new Type[] { typeof(object) }, type, true);
		ILGenerator il = dynamicGet.GetILGenerator();

		if (!type.IsClass) // structs
		{
			var lv = il.DeclareLocal(type);
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Unbox_Any, type);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Ldloca_S, lv);
			il.Emit(OpCodes.Ldfld, fieldInfo);
			if (fieldInfo.FieldType.IsValueType)
				il.Emit(OpCodes.Box, fieldInfo.FieldType);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Ldfld, fieldInfo);
			if (fieldInfo.FieldType.IsValueType)
				il.Emit(OpCodes.Box, fieldInfo.FieldType);
		}

		il.Emit(OpCodes.Ret);

		return (GenericGetter)dynamicGet.CreateDelegate(typeof(GenericGetter));
	}

	internal static GenericGetter CreateGetMethod(Type type, PropertyInfo propertyInfo)
	{
		MethodInfo getMethod = propertyInfo.GetGetMethod();
		if (getMethod == null)
			return null;

		Type[] arguments = new Type[1];
		arguments[0] = typeof(object);

		DynamicMethod getter = new DynamicMethod("_", typeof(object), arguments, type);
		ILGenerator il = getter.GetILGenerator();
		
		if (!type.IsClass) // structs
		{
			var lv = il.DeclareLocal(type);
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Unbox_Any, type);
			il.Emit(OpCodes.Stloc_0);
			il.Emit(OpCodes.Ldloca_S, lv);
			il.EmitCall(OpCodes.Call, getMethod, null);
			if (propertyInfo.PropertyType.IsValueType)
				il.Emit(OpCodes.Box, propertyInfo.PropertyType);
		}
		else
		{
			il.Emit(OpCodes.Ldarg_0);
			il.Emit(OpCodes.Castclass, propertyInfo.DeclaringType);
			il.EmitCall(OpCodes.Callvirt, getMethod, null);
			if (propertyInfo.PropertyType.IsValueType)
				il.Emit(OpCodes.Box, propertyInfo.PropertyType);
		}

		il.Emit(OpCodes.Ret);

		return (GenericGetter)getter.CreateDelegate(typeof(GenericGetter));
	}

	internal List<Getters> GetGetters(Type type)
	{
		List<Getters> val = null;
		if (_getterscache.TryGetValue(type, out val))
			return val;

		PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
		List<Getters> getters = new List<Getters>();
		foreach (PropertyInfo p in props)
		{
			if (!p.CanWrite && ShowReadOnlyProperties == false) continue;

			object[] att = p.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), false);
			if (att != null && att.Length > 0)
				continue;

			GenericGetter g = CreateGetMethod(type, p);
			if (g != null)
			{
				Getters gg = new Getters();
				gg.Name = p.Name;
				gg.Getter = g;
				gg.propertyType = p.PropertyType;
				getters.Add(gg);
			}
		}

		FieldInfo[] fi = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
		foreach (var f in fi)
		{
			object[] att = f.GetCustomAttributes(typeof(System.Xml.Serialization.XmlIgnoreAttribute), false);
			if (att != null && att.Length > 0)
				continue;

			GenericGetter g = CreateGetField(type, f);
			if (g != null)
			{
				Getters gg = new Getters();
				gg.Name = f.Name;
				gg.Getter = g;
				gg.propertyType = f.FieldType;
				getters.Add(gg);
			}
		}

		_getterscache.Add(type, getters);
		return getters;
	}

	#endregion
}
 #endregion
 
 
 
 
 
 #region SafeDictionary
internal class SafeDictionary<TKey, TValue>
{
	private readonly object _Padlock = new object();
	private readonly Dictionary<TKey, TValue> _Dictionary;

	public SafeDictionary(int capacity)
	{
		_Dictionary = new Dictionary<TKey, TValue>(capacity);
	}

	public SafeDictionary()
	{
		_Dictionary = new Dictionary<TKey, TValue>();
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		lock (_Padlock)
			return _Dictionary.TryGetValue(key, out value);
	}

	public TValue this[TKey key]
	{
		get
		{
			lock (_Padlock)
				return _Dictionary[key];
		}
		set
		{
			lock (_Padlock)
				_Dictionary[key] = value;
		}
	}

	public void Add(TKey key, TValue value)
	{
		lock (_Padlock)
		{
			if (_Dictionary.ContainsKey(key) == false)
				_Dictionary.Add(key, value);
		}
	}
}
#endregion




#region Formatter
internal static class Formatter
{
	public static string Indent = "    ";

	public static void AppendIndent(StringBuilder sb, int count)
	{
		for (; count > 0; --count) sb.Append(Indent);
	}

	public static bool IsEscaped(StringBuilder sb, int index)
	{
		bool escaped = false;
		while (index > 0 && sb[--index] == '\\') escaped = !escaped;
		return escaped;
	}

	public static string PrettyPrint(string input)
	{
		var output = new StringBuilder(input.Length * 2);
		char? quote = null;
		int depth = 0;

		for (int i = 0; i < input.Length; ++i)
		{
			char ch = input[i];

			switch (ch)
			{
				case '{':
				case '[':
					output.Append(ch);
					if (!quote.HasValue)
					{
						output.AppendLine();
						AppendIndent(output, ++depth);
					}
					break;
				case '}':
				case ']':
					if (quote.HasValue)
						output.Append(ch);
					else
					{
						output.AppendLine();
						AppendIndent(output, --depth);
						output.Append(ch);
					}
					break;
				case '"':
				case '\'':
					output.Append(ch);
					if (quote.HasValue)
					{
						if (!IsEscaped(output, i))
							quote = null;
					}
					else quote = ch;
					break;
				case ',':
					output.Append(ch);
					if (!quote.HasValue)
					{
						output.AppendLine();
						AppendIndent(output, depth);
					}
					break;
				case ':':
					if (quote.HasValue) output.Append(ch);
					else output.Append(" : ");
					break;
				default:
					if (quote.HasValue || !char.IsWhiteSpace(ch))
						output.Append(ch);
					break;
			}
		}

		return output.ToString();
	}
}
#endregion




#region Getters
 public class DatasetSchema
{
	public List<string> Info { get; set; }
	public string Name { get; set; }
}
#endregion






#region Parser

/// <summary>
/// This class encodes and decodes JSON strings.
/// Spec. details, see http://www.json.org/
/// 
/// JSON uses Arrays and Objects. These correspond here to the datatypes ArrayList and Hashtable.
/// All numbers are parsed to doubles.
/// </summary>
internal class JsonParser
{
	enum Token
	{
		None = -1,           // Used to denote no Lookahead available
		Curly_Open,
		Curly_Close,
		Squared_Open,
		Squared_Close,
		Colon,
		Comma,
		String,
		Number,
		True,
		False,
		Null
	}

	readonly char[] json;
	readonly StringBuilder s = new StringBuilder();
	Token lookAheadToken = Token.None;
	int index;
	bool _ignorecase = false;


	internal JsonParser(string json, bool ignorecase)
	{
		this.json = json.ToCharArray();
		_ignorecase = ignorecase;
	}

	public object Decode()
	{
		return ParseValue();
	}

	private Dictionary<string, object> ParseObject()
	{
		Dictionary<string, object> table = new Dictionary<string, object>();

		ConsumeToken(); // {

		while (true)
		{
			switch (LookAhead())
			{

				case Token.Comma:
					ConsumeToken();
					break;

				case Token.Curly_Close:
					ConsumeToken();
					return table;

				default:
					{

						// name
						string name = ParseString();
						if (_ignorecase)
							name = name.ToLower();

						// :
						if (NextToken() != Token.Colon)
						{
							throw new Exception("Expected colon at index " + index);
						}

						// value
						object value = ParseValue();

						table[name] = value;
					}
					break;
			}
		}
	}

	private ArrayList ParseArray()
	{
		ArrayList array = new ArrayList();
		
		ConsumeToken(); // [

		while (true)
		{
			switch (LookAhead())
			{

				case Token.Comma:
					ConsumeToken();
					break;

				case Token.Squared_Close:
					ConsumeToken();
					return array;

				default:
					{
						array.Add(ParseValue());
					}
					break;
			}
		}
	}

	private object ParseValue()
	{
		switch (LookAhead())
		{
			case Token.Number:
				return ParseNumber();

			case Token.String:
				return ParseString();

			case Token.Curly_Open:
				return ParseObject();

			case Token.Squared_Open:
				return ParseArray();

			case Token.True:
				ConsumeToken();
				return true;

			case Token.False:
				ConsumeToken();
				return false;

			case Token.Null:
				ConsumeToken();
				return null;
		}

		throw new Exception("Unrecognized token at index" + index);
	}

	private string ParseString()
	{
		ConsumeToken(); // "

		s.Length = 0;

		int runIndex = -1;

		while (index < json.Length)
		{
			var c = json[index++];

			if (c == '"')
			{
				if (runIndex != -1)
				{
					if (s.Length == 0)
						return new string(json, runIndex, index - runIndex - 1);

					s.Append(json, runIndex, index - runIndex - 1);
				}
				return s.ToString();
			}

			if (c != '\\')
			{
				if (runIndex == -1)
					runIndex = index - 1;

				continue;
			}

			if (index == json.Length) break;

			if (runIndex != -1)
			{
				s.Append(json, runIndex, index - runIndex - 1);
				runIndex = -1;
			}

			switch (json[index++])
			{
				case '"':
					s.Append('"');
					break;

				case '\\':
					s.Append('\\');
					break;

				case '/':
					s.Append('/');
					break;

				case 'b':
					s.Append('\b');
					break;

				case 'f':
					s.Append('\f');
					break;

				case 'n':
					s.Append('\n');
					break;

				case 'r':
					s.Append('\r');
					break;

				case 't':
					s.Append('\t');
					break;

				case 'u':
					{
						int remainingLength = json.Length - index;
						if (remainingLength < 4) break;

						// parse the 32 bit hex into an integer codepoint
						uint codePoint = ParseUnicode(json[index], json[index + 1], json[index + 2], json[index + 3]);
						s.Append((char)codePoint);

						// skip 4 chars
						index += 4;
					}
					break;
			}
		}

		throw new Exception("Unexpectedly reached end of string");
	}

	private uint ParseSingleChar(char c1, uint multipliyer)
	{
		uint p1 = 0;
		if (c1 >= '0' && c1 <= '9')
			p1 = (uint)(c1 - '0') * multipliyer;
		else if (c1 >= 'A' && c1 <= 'F')
			p1 = (uint)((c1 - 'A') + 10) * multipliyer;
		else if (c1 >= 'a' && c1 <= 'f')
			p1 = (uint)((c1 - 'a') + 10) * multipliyer;
		return p1;
	}

	private uint ParseUnicode(char c1, char c2, char c3, char c4)
	{
		uint p1 = ParseSingleChar(c1, 0x1000);
		uint p2 = ParseSingleChar(c2, 0x100);
		uint p3 = ParseSingleChar(c3, 0x10);
		uint p4 = ParseSingleChar(c4, 1);

		return p1 + p2 + p3 + p4;
	}

	private string ParseNumber()
	{
		ConsumeToken();

		// Need to start back one place because the first digit is also a token and would have been consumed
		var startIndex = index - 1;

		do
		{
			var c = json[index];

			if ((c >= '0' && c <= '9') || c == '.' || c == '-' || c == '+' || c == 'e' || c == 'E')
			{
				if (++index == json.Length) throw new Exception("Unexpected end of string whilst parsing number");
				continue;
			}

			break;
		} while (true);

		return new string(json, startIndex, index - startIndex);
	}

	private Token LookAhead()
	{
		if (lookAheadToken != Token.None) return lookAheadToken;

		return lookAheadToken = NextTokenCore();
	}

	private void ConsumeToken()
	{
		lookAheadToken = Token.None;
	}

	private Token NextToken()
	{
		var result = lookAheadToken != Token.None ? lookAheadToken : NextTokenCore();

		lookAheadToken = Token.None;

		return result;
	}

	private Token NextTokenCore()
	{
		char c;

		// Skip past whitespace
		do
		{
			c = json[index];

			if (c > ' ') break;
			if (c != ' ' && c != '\t' && c != '\n' && c != '\r') break;

		} while (++index < json.Length);

		if (index == json.Length)
		{
			throw new Exception("Reached end of string unexpectedly");
		}

		c = json[index];

		index++;

		//if (c >= '0' && c <= '9')
		//    return Token.Number;

		switch (c)
		{
			case '{':
				return Token.Curly_Open;

			case '}':
				return Token.Curly_Close;

			case '[':
				return Token.Squared_Open;

			case ']':
				return Token.Squared_Close;

			case ',':
				return Token.Comma;

			case '"':
				return Token.String;

			case '0': case '1': case '2': case '3': case '4':
			case '5': case '6': case '7': case '8': case '9':
			case '-': case '+': case '.':
				return Token.Number;

			case ':':
				return Token.Colon;

			case 'f':
				if (json.Length - index >= 4 &&
					json[index + 0] == 'a' &&
					json[index + 1] == 'l' &&
					json[index + 2] == 's' &&
					json[index + 3] == 'e')
				{
					index += 4;
					return Token.False;
				}
				break;

			case 't':
				if (json.Length - index >= 3 &&
					json[index + 0] == 'r' &&
					json[index + 1] == 'u' &&
					json[index + 2] == 'e')
				{
					index += 3;
					return Token.True;
				}
				break;

			case 'n':
				if (json.Length - index >= 3 &&
					json[index + 0] == 'u' &&
					json[index + 1] == 'l' &&
					json[index + 2] == 'l')
				{
					index += 3;
					return Token.Null;
				}
				break;

		}

		throw new Exception("Could not find token at index " + --index);
	}
}
#endregion

