public class Twin<T1, T2>
{
	public T1 GameObject
	{
		get; private set;
	}
	public T2 Type
	{
		get; private set;
	}
	internal Twin(T1 gameObject, T2 view)
	{
		GameObject = gameObject;
		Type = view;
	}
}

public static class Twin
{
	public static Twin<T1, T2> New<T1, T2>(T1 gameObject, T2 viewModel)
	{
		var twin = new Twin<T1, T2>(gameObject, viewModel);
		return twin;
	}
}