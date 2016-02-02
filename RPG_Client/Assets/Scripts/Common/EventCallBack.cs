using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// socket层异步回调接口
/// </summary>
/// <param name="iar"></param>
public delegate void NetEventCallBack(IAsyncResult iar);


/// <summary>
/// 默认无参的回调接口
/// </summary>
public delegate void DefAction();