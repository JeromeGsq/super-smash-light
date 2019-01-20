using UnityEngine;
using UnityWeld.Binding;

[Adapter(typeof(string), typeof(float), typeof(FloatToImageAmountAdapterOptions))]
public class FloatToImageAmount : IAdapter
{
	public object Convert(object valueIn, AdapterOptions options)
	{
		var scale = ((FloatToImageAmountAdapterOptions)options)?.Scale;
		if(scale != null)
		{
			return ((float)valueIn) * scale;
		}
		else
		{
			return (float)valueIn;
		}
	}
}


