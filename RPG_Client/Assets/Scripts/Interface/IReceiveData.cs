using UnityEngine;
using System.Collections;
/// <summary>
/// 接受消息接口，所有client接受到的消息最终都会被扔到消息队列中，轮训调用该接口处理消息
/// </summary>
interface IReceiveData {

    void ReceiveData(MsgEntity msg);
}
