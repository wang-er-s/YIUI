### Client 与 Realm 通讯

1. 从 Router 里面找到一个 Realm 的地址
   使用这个地址创建一个 Session 来发送登陆请求 C2R_Login
2. Realm 服务器收到消息 NetComponentOnReadInvoker_Realm，从消息分发里找到这个消息的 Handler，然后处理

### Realm 与 Gate 通讯

1. 然后 Realm 像 Gate 请求一个登陆 key R2G_GetLoginKey
2. 如果 Realm 和 Gate 在同一个进程里，则走类似 Main2Net 一样的内网通讯 ProcessInnerSender.Call
3. 如果不在一个进程，则通过 A2NetInner_Request 放到 Realm 消息发送线程里面处理，类似 Main2Net 的 A2NetClient_Message
4. A2NetClient_MessageHandler 通过 ProcessOuterSender.Call 来创建一个新的 Session 通过内网发送至 Gate，类似的通过 NetComponentOnReadInvoker_Gate 分发来来找到这个消息的 Handler
