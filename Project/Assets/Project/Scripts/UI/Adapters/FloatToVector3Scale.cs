using UnityEngine;
using UnityWeld.Binding;

[Adapter(typeof(string), typeof(Vector3), typeof(FloatToVector3ScaleOptions))]
public class FloatToVector3Scale : IAdapter
{
	public object Convert(object valueIn, AdapterOptions options)
	{
		var scale = ((FloatToVector3ScaleOptions)options)?.Scale;
		if(scale != null)
		{
			return (((float)valueIn) * scale + 0.5f) * Vector3.one ;
		}
		else
		{
			return ((float)valueIn + 0.5f) * Vector3.one ;
		}
	}
}