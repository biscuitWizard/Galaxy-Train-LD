using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;

public static class JsonSerializer {
	public static string Serialize(List<int> data) {
		var joined = string.Join(",", data.Select (token => token.ToString()).ToArray());
		
		return string.Format("[{0}]", joined);
	}
	
	public static List<int> Deserialize(string data) {
		var result = data.Trim(new [] { '[', ']' })
			.Split(new string[] { "," }, StringSplitOptions.None)
				.Select(token => Int32.Parse(token))
				.ToList();
		
		return result;
	}
}
