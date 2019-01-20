using System;
using System.ComponentModel;

public interface IViewModel
{
	object Parameters
	{
		get; set;
	}

	void SetParameters<T>(T parameters);
}