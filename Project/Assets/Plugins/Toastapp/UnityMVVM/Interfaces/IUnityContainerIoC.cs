﻿using System;

public interface IUnityContainerIoC
{
	T Resolve<T>() where T : class, IUnityIoC;
	T Resolve<T>(Type type) where T : class, IUnityIoC;
}
