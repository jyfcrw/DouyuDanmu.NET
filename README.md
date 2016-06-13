# DouyuDanmu.NET
依照《斗鱼弹幕服务器第三方接入协议》(v1.4.1), 接入斗鱼弹幕服务的客户端C#类库实现, 基于.NET 4.6

~~~~
  using DouyuDanmu;
  
  ...
  
  // 实例化弹幕客户端对象，传入房间号码(string)和lambda表达式
  var client = new DouyuClient("10000", (msg) => {
      switch ((string)msg["type"])
      {
          case "chatmsg":
              Console.WriteLine(msg["txt"]); // 文字弹幕内容
              break;

          default:
              Console.WriteLine(String.Format("<{0}>", msg["type"]));
              break;
      }
  });
  
  // 开始接收弹幕
  client.Connect();
~~~~
