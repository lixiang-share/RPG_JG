using UnityEngine;
using System.Collections;
/// <summary>
/// 定义消息类型
/// </summary>
public class MsgProtocol {
    //请求协议后，服务端返回状态
    public const int Error = -1;
    public const int Success = 0;

    //协议类型
    public const int Login = 1;
    public const int Get_ServerList = 2;
    public const int Register = 3;
}
