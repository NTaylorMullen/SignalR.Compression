# SignalR.Compression
SignalR.Compression is an [ASP.NET SignalR](https://github.com/SignalR/SignalR) addon built to minimize bandwidth consumption of existing ASP.NET SignalR applications.

## What can it be used for?
It can be used on any ASP.NET SignalR application to speed the process of sending data over the wire.  Applications which have a bandwidth bottleneck will especially see benefits from SignalR.Compression.

## Quick Start - Chat Application
Lets take an existing ASP.NET SignalR application for chatting:

**Server**
```csharp
...
public class ChatHub : Hub
{
    public void BroadcastMessage(string from, string content)
    {
        Clients.All.receiveMessage(new Message(from, content));
    }
}

public class Message
{
    public Message(string from, string content)
    {
        From = from;
        Content = content;
    }

    public string From { get; set; }
    public string Content { get; set; }
}
...
```

**Application Start**
```csharp
...
protected void Application_Start(object sender, EventArgs e)
{    
    RouteTable.Routes.MapHubs();
}
...
```

**Client**
```html
...
<div id="messages"></div>
<p><strong>From:</strong> <input type="text" id="messageFrom" value="John" /></p>
<p><strong>Message:</strong> <input type="text" id="messageContent" value="Hello World!" /></p>
<p><input type="button" value="Send" id="sendMessage" /></p>

<script type="text/javascript" src="Scripts/jquery-1.9.1.js"></script>
<script type="text/javascript" src="Scripts/jquery.signalr-1.0.1.js"></script>
<script type="text/javascript" src="signalr/hubs"></script>
<script type="text/javascript">
  var chatHub = $.connection.chatHub;
  
  chatHub.client.receiveMessage = function (message) {
      $("#messages").append("<p><strong>" + message.From + ":</strong> " + message.Content + "</p>");
  };
  
  $.connection.hub.start().done(function() {
    $("#sendMessage").click(function () {
        chatHub.server.broadcastMessage($("#messageFrom").val(), $("#messageContent").val());
    });
  });
</script>
...
```

***So what's happening?***  Clicking the "Send" button results in ASP.NET SignalR using JSON.NET to stringify the Message object and then send it over the wire to all clients.

The body of a message being sent from "John" with content "Hello World!" **without** SignalR.Compression looks like:
```JSON
{
  Name: "John",
  Content: "Hello World!"
}
```

**With** SignalR.Compression we can make the body of the message (over the wire) look like:
```json
["Hello World!", "John"]
```
Once the message gets to the other end, it's then decompressed into the easy-to-use state:
```JSON
{
  Name: "John",
  Content: "Hello World!"
}
```

## Adding SignalR.Compression to the Chat Application
### Server
1) Add a reference to Microsoft.AspNet.SignalR.Compression.Server and Microsoft.AspNet.SignalR.Compression.Server.SystemWeb  
2) In your Application Start add this line **BEFORE** your MapHubs:  
```csharp
...
GlobalHost.DependencyResolver.Compression().CompressPayloads(RouteTable.Routes);
RouteTable.Routes.MapHubs();
...
```
You will also need to add usings to your Application_Start file.  
3) Declare what is a payload by adding the Payload attribute.    
```csharp
...
[Payload]
public class Message
{
...
```

**Note:** After declaring your payload, it will always be compressed/decompressed when sent over the wire.

### Client
1) Add jquery.signalr.compression.js file **BEFORE** the "signalr/hubs" script. Example using the Chat Application:  
```html
...
<script type="text/javascript" src="Scripts/jquery.signalr-1.0.1.js"></script>
<script type="text/javascript" src="Scripts/jquery.signalr.compression.js"></script>
<script type="text/javascript" src="signalr/hubs"></script>
...
```

## Finished SignalR.Compression Chat Application

**Server**
```csharp
...
public class ChatHub : Hub
{
    public void BroadcastMessage(string from, string content)
    {
        Clients.All.receiveMessage(new Message(from, content));
    }
}

[Payload]
public class Message
{
    public Message(string from, string content)
    {
        From = from;
        Content = content;
    }

    public string From { get; set; }
    public string Content { get; set; }
}
...
```

**Application Start**
```csharp
...
protected void Application_Start(object sender, EventArgs e)
{    
    GlobalHost.DependencyResolver.Compression().CompressPayloads(RouteTable.Routes);
    RouteTable.Routes.MapHubs();
}
...
```

**Client**
```html
...
<div id="messages"></div>
<p><strong>From:</strong> <input type="text" id="messageFrom" value="John" /></p>
<p><strong>Message:</strong> <input type="text" id="messageContent" value="Hello World!" /></p>
<p><input type="button" value="Send" id="sendMessage" /></p>

<script type="text/javascript" src="Scripts/jquery-1.9.1.js"></script>
<script type="text/javascript" src="Scripts/jquery.signalr-1.0.1.js"></script>
<script type="text/javascript" src="Scripts/jquery.signalr.compression.js"></script>
<script type="text/javascript" src="signalr/hubs"></script>
<script type="text/javascript">
  var chatHub = $.connection.chatHub;
  
  chatHub.client.receiveMessage = function (message) {
      $("#messages").append("<p><strong>" + message.From + ":</strong> " + message.Content + "</p>");
  };
  
  $.connection.hub.start().done(function() {
    $("#sendMessage").click(function () {
        chatHub.server.broadcastMessage($("#messageFrom").val(), $("#messageContent").val());
    });
  });
</script>
...
```

*For additional samples check out the Samples within the SignalR.Compression source.*

## Advanced topics
### Payload Attribute Options
**RoundNumbersTo**: Forces all numeric members to be rounded to the specified number of digits. Defaults to -1 (will not round).

***More payload attribute options will be added in the future.***
