通过

```
CreateRouterSession(this NetComponent netComponent, IPEndPoint address, string account, string password)
```

来创建 session，一个 session 就是一个连接。
比如前期先创建 C 2 R 的 session，来登录账号获取 gate 的地址和 key
然后创建 C 2 G 的 session，来登录角色，后面的 Map 就一直用这个 session 来进行通信
