# mqttnet-unity-alpn

## NOTE:

The MQTTnet submodule in this repo is forked from the main repo. NETCOREAPP3_1_OR_GREATER define symbol has manually been set to true and CipherSuitesPolicy has been commented out. This is to demonstrate that ALPN support should be working in netstandard2.1 as seen here: https://learn.microsoft.com/en-us/dotnet/api/system.net.security.sslapplicationprotocol?view=netstandard-2.1

Currently only websockets works in both dotnet and websocket4net.

You can view the git history of the MQTTnet submodule to see the changed files.

## Dependencies

Tested on Ubuntu 22.04 and Windows 10. 

Windows 10 requires WSL Ubuntu 22.04 for cross compiling to ARM64 processors.

1. Install git: https://git-scm.com/downloads
    - NOTE: Reproduced with version: ``git version 2.40.1``
1. Install the rust toolset: https://www.rust-lang.org/tools/install
    - NOTE: Reproduced with version: ``rustup 1.26.0 (5af9b9484 2023-04-05), cargo 1.69.0 (6e9a83356 2023-04-12), rustc 1.69.0 (84c898d65 2023-04-16)``
1. Install cargo lambda: https://github.com/awslabs/aws-lambda-rust-runtime
    - NOTE: Reproduced with version: ``cargo-lambda 0.19.0 (e7a2b99 2023-04-07Z)``
1. Install aws cli v2: https://docs.aws.amazon.com/cli/latest/userguide/getting-started-install.html
    - NOTE: Reproduced with version: ``aws-cli/2.11.15 Python/3.11.3 Linux/6.2.6-76060206-generic exe/x86_64.pop.22 prompt/off``
1. Install dotnet 7.0: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
    - NOTE: Reproduced with version: ``7.0.203``
1. Install Unity 2021.3.24f1: https://unity.com/download
1. Clone:
    ```
    git clone https://github.com/TCROC/aws-iot-custom-auth.git --recurse-submodules
    ```

**NOTE:** When running the scripts, you can ignore the aws cli errors that are logged.  The scripts do things such as check if the lambda function is deployed by calling ``aws lambda get-function ``. If the command errors, the script assumes it doesn't exist in the cloud and attempts to create one.

## Create Lambda Authorizer

Run in a bash shell:

```bash
./create-lambda.sh
```

## Test Lambda Authorizer

1. Run in a bash shell:
    ```bash
    ./init.sh
    ``` 
1. Get the iot endpoint by running the following command:
    ```
    aws iot describe-endpoint --endpoint-type iot:Data-ATS --output text
    ```
1. Open the project in Unity
1. Open the SampleScene
1. Select the MqttClient in the Hierarchy pane
1. Edit the ``Endpoint`` property in the Inspector pane
1. Change between the ``Transport Implementation`` and ``Transport`` values to test different combinations
1. Hit "Play" to test a combination
1. Select the "console" tab to see the output

**Expected results:** The mqtt client sends keep alive packets for 24 hours as specified in the policy returned from the lambda function in any transport.

**Actual result:** The mqtt client forms a connection over TCP, but times out when attempting to authenticate.

<details>
<summary>dotnet websocket log</summary>

```log
Running mqtt example application!
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:40)


===================

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:42)

Args Used

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:43)

clientId:                  testid
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:44)

password:                  testpassword
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:45)

endpoint:                  a1jbgnrm1s76uh-ats.iot.us-east-1.amazonaws.com
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:46)

rootTopic:                 open
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:47)

authorizer:                iot-lambda-authorizer
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:48)

transportImplementation:   Dotnet
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:49)

transport:                 Websocket
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:50)


===================

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:51)

[2023-05-08T21:42:38.9922030Z] [MqttNet] [1] [MqttClient] [Verbose]: Trying to connect with server 'a1jbgnrm1s76uh-ats.iot.us-east-1.amazonaws.com/mqtt'
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<MQTTnet.Client.IMqttClientChannelOptions> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,MQTTnet.Client.IMqttClientChannelOptions) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:112)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:491)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<ConnectInternal>d__60> (MQTTnet.Client.MqttClient/<ConnectInternal>d__60&)
MQTTnet.Client.MqttClient:ConnectInternal (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:143)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<ConnectAsync>d__48> (MQTTnet.Client.MqttClient/<ConnectAsync>d__48&)
MQTTnet.Client.MqttClient:ConnectAsync (MQTTnet.Client.MqttClientOptions,System.Threading.CancellationToken)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:95)

[2023-05-08T21:42:39.4089570Z] [MqttNet] [28] [MqttClient] [Verbose]: Connection with server established
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:493)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Adapter.MqttChannelAdapter/<ConnectAsync>d__34:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:103)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Implementations.MqttWebSocketChannel/<ConnectAsync>d__15:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttWebSocketChannel.cs:75)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.4121860Z] [MqttNet] [29] [MqttClient] [Verbose]: Start receiving packets.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:895)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82> (MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82&)
MQTTnet.Client.MqttClient:TryReceivePacketsAsync (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<>c__DisplayClass60_0:<ConnectInternal>b__1 () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:499)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.4189650Z] [MqttNet] [28] [MqttChannelAdapter] [Verbose]: TX (114 bytes) >>> Connect: [ClientId=testid] [Username=username?x-amz-customauthorizer-name=iot-lambda-authorizer] [Password=****] [KeepAlivePeriod=35] [CleanSession=True]
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<int, MQTTnet.Packets.MqttPacket> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,int,MQTTnet.Packets.MqttPacket) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:122)
MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:206)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38> (MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38&)
MQTTnet.Adapter.MqttChannelAdapter:SendPacketAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient:SendAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:769)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:739)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:Start<MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>> (MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>&)
MQTTnet.Client.MqttClient:SendAndReceiveAsync<MQTTnet.Packets.MqttConnAckPacket> (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:438)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57> (MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57&)
MQTTnet.Client.MqttClient:AuthenticateAsync (MQTTnet.Client.MqttClientOptions,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Adapter.MqttChannelAdapter/<ConnectAsync>d__34:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:103)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Implementations.MqttWebSocketChannel/<ConnectAsync>d__15:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttWebSocketChannel.cs:75)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.7769720Z] [MqttNet] [29] [MqttChannelAdapter] [Verbose]: RX (29 bytes) <<< ConnAck: [ReturnCode=ConnectionAccepted] [ReasonCode=Success] [IsSessionPresent=False]
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<int, MQTTnet.Packets.MqttPacket> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,int,MQTTnet.Packets.MqttPacket) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:122)
MQTTnet.Adapter.MqttChannelAdapter/<ReceivePacketAsync>d__36:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:163)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Adapter.ReceivedMqttPacket>:SetResult (MQTTnet.Adapter.ReceivedMqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceiveAsync>d__42:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:423)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Formatter.ReadFixedHeaderResult>:SetResult (MQTTnet.Formatter.ReadFixedHeaderResult)
MQTTnet.Adapter.MqttChannelAdapter/<ReadFixedHeaderAsync>d__41:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:355)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
MQTTnet.Implementations.MqttWebSocketChannel/<ReadAsync>d__18:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttWebSocketChannel.cs:101)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.7804580Z] [MqttNet] [33] [MqttClient] [Verbose]: Authenticated MQTT connection with server established.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:453)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:761)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:39)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.7812040Z] [MqttNet] [33] [MqttClient] [Info]: Using keep alive value (35) sent from the server.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Info (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:82)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:152)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetResult (MQTTnet.Client.MqttClientConnectResult)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:503)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetResult (MQTTnet.Client.MqttClientConnectResult)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:456)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:761)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:39)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.7816310Z] [MqttNet] [33] [MqttClient] [Info]: Connected.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Info (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:82)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:163)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetResult (MQTTnet.Client.MqttClientConnectResult)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:503)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetResult (MQTTnet.Client.MqttClientConnectResult)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:456)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:761)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetResult (MQTTnet.Packets.MqttConnAckPacket)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:39)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:42:39.7826420Z] [MqttNet] [28] [MqttClient] [Verbose]: Start sending keep alive packets.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<TrySendKeepAliveMessagesAsync>d__83:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:964)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<TrySendKeepAliveMessagesAsync>d__83> (MQTTnet.Client.MqttClient/<TrySendKeepAliveMessagesAsync>d__83&)
MQTTnet.Client.MqttClient:TrySendKeepAliveMessagesAsync (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<>c__DisplayClass48_0:<ConnectAsync>b__0 () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:158)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

Client connection status: True. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: True. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: True. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

[2023-05-08T21:43:14.7944390Z] [MqttNet] [28] [MqttChannelAdapter] [Verbose]: TX (2 bytes) >>> PingReq
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<int, MQTTnet.Packets.MqttPacket> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,int,MQTTnet.Packets.MqttPacket) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:122)
MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:206)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38> (MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38&)
MQTTnet.Adapter.MqttChannelAdapter:SendPacketAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient:SendAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:769)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttPingRespPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:739)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttPingRespPacket>:Start<MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttPingRespPacket>> (MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttPingRespPacket>&)
MQTTnet.Client.MqttClient:SendAndReceiveAsync<MQTTnet.Packets.MqttPingRespPacket> (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<PingAsync>d__50:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:242)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<PingAsync>d__50> (MQTTnet.Client.MqttClient/<PingAsync>d__50&)
MQTTnet.Client.MqttClient:PingAsync (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<TrySendKeepAliveMessagesAsync>d__83:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:978)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:43:14.8575300Z] [MqttNet] [33] [MqttChannelAdapter] [Verbose]: RX (2 bytes) <<< PingResp
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<int, MQTTnet.Packets.MqttPacket> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,int,MQTTnet.Packets.MqttPacket) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:122)
MQTTnet.Adapter.MqttChannelAdapter/<ReceivePacketAsync>d__36:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:163)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Adapter.ReceivedMqttPacket>:SetResult (MQTTnet.Adapter.ReceivedMqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceiveAsync>d__42:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:423)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Formatter.ReadFixedHeaderResult>:SetResult (MQTTnet.Formatter.ReadFixedHeaderResult)
MQTTnet.Adapter.MqttChannelAdapter/<ReadFixedHeaderAsync>d__41:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:355)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
MQTTnet.Implementations.MqttWebSocketChannel/<ReadAsync>d__18:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttWebSocketChannel.cs:101)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

Client connection status: True. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)
```

</details>

<details>
<summary>dotnet tcp log</summary>

```log
Running mqtt example application!
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:40)


===================

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:42)

Args Used

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:43)

clientId:                  testid
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:44)

password:                  testpassword
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:45)

endpoint:                  a1jbgnrm1s76uh-ats.iot.us-east-1.amazonaws.com
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:46)

rootTopic:                 open
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:47)

authorizer:                iot-lambda-authorizer
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:48)

transportImplementation:   Dotnet
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:49)

transport:                 Tcp
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:50)


===================

UnityEngine.Debug:Log (object)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:51)

[2023-05-08T21:45:03.8869370Z] [MqttNet] [1] [MqttClient] [Verbose]: Trying to connect with server 'a1jbgnrm1s76uh-ats.iot.us-east-1.amazonaws.com:443'
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<MQTTnet.Client.IMqttClientChannelOptions> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,MQTTnet.Client.IMqttClientChannelOptions) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:112)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:491)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<ConnectInternal>d__60> (MQTTnet.Client.MqttClient/<ConnectInternal>d__60&)
MQTTnet.Client.MqttClient:ConnectInternal (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:143)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<ConnectAsync>d__48> (MQTTnet.Client.MqttClient/<ConnectAsync>d__48&)
MQTTnet.Client.MqttClient:ConnectAsync (MQTTnet.Client.MqttClientOptions,System.Threading.CancellationToken)
MqttClientBehaviour:Start () (at Assets/MqttClientBehaviour.cs:95)

[2023-05-08T21:45:04.3236290Z] [MqttNet] [33] [MqttClient] [Verbose]: Connection with server established
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:493)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Adapter.MqttChannelAdapter/<ConnectAsync>d__34:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:103)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Implementations.MqttTcpChannel/<ConnectAsync>d__17:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttTcpChannel.cs:153)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.MobileAuthenticatedStream/<ProcessAuthentication>d__48:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:45:04.3260330Z] [MqttNet] [28] [MqttClient] [Verbose]: Start receiving packets.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:895)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82> (MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82&)
MQTTnet.Client.MqttClient:TryReceivePacketsAsync (System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<>c__DisplayClass60_0:<ConnectInternal>b__1 () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:499)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:45:04.3339230Z] [MqttNet] [33] [MqttChannelAdapter] [Verbose]: TX (114 bytes) >>> Connect: [ClientId=testid] [Username=username?x-amz-customauthorizer-name=iot-lambda-authorizer] [Password=****] [KeepAlivePeriod=35] [CleanSession=True]
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<int, MQTTnet.Packets.MqttPacket> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,int,MQTTnet.Packets.MqttPacket) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:122)
MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:206)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38> (MQTTnet.Adapter.MqttChannelAdapter/<SendPacketAsync>d__38&)
MQTTnet.Adapter.MqttChannelAdapter:SendPacketAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient:SendAsync (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:769)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:739)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:Start<MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>> (MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>&)
MQTTnet.Client.MqttClient:SendAndReceiveAsync<MQTTnet.Packets.MqttConnAckPacket> (MQTTnet.Packets.MqttPacket,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:438)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:Start<MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57> (MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57&)
MQTTnet.Client.MqttClient:AuthenticateAsync (MQTTnet.Client.MqttClientOptions,System.Threading.CancellationToken)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Adapter.MqttChannelAdapter/<ConnectAsync>d__34:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:103)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Implementations.MqttTcpChannel/<ConnectAsync>d__17:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttTcpChannel.cs:153)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.MobileAuthenticatedStream/<ProcessAuthentication>d__48:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetResult (System.Nullable`1<int>)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

Client connection status: False. Checking again in 10 seconds...
UnityEngine.Debug:Log (object)
MqttClientBehaviour:Update () (at Assets/MqttClientBehaviour.cs:113)

[2023-05-08T21:46:43.8870460Z] [MqttNet] [33] [MqttClient] [Warning]: Timeout while waiting for response packet (MqttConnAckPacket).
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Warning<string> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:187)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:755)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:37)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:46:43.8878430Z] [MqttNet] [33] [MqttClient] [Error]: Error while connecting with server.
MQTTnet.Adapter.MqttConnectingFailedException: Error while authenticating. The operation has timed out. ---> MQTTnet.Exceptions.MqttCommunicationTimedOutException: The operation has timed out.
  at MQTTnet.PacketDispatcher.MqttPacketAwaitable`1[TPacket].WaitOneAsync (System.Threading.CancellationToken cancellationToken) [0x00059] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:36 
  at MQTTnet.Client.MqttClient.SendAndReceiveAsync[TResponsePacket] (MQTTnet.Packets.MqttPacket requestPacket, System.Threading.CancellationToken cancellationToken) [0x00232] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:758 
  at MQTTnet.Client.MqttClient.AuthenticateAsync (MQTTnet.Client.MqttClientOptions options, System.Threading.CancellationToken cancellationToken) [0x00054] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:438 
   --- End of inner exception stack trace ---
  at MQTTnet.Client.MqttClient.AuthenticateAsync (MQTTnet.Client.MqttClientOptions options, System.Threading.CancellationToken cancellationToken) [0x0011b] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:445 
  at MQTTnet.Client.MqttClient.ConnectInternal (System.Threading.CancellationToken cancellationToken) [0x001e6] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501 
  at MQTTnet.Client.MqttClient.ConnectAsync (MQTTnet.Client.MqttClientOptions options, System.Threading.CancellationToken cancellationToken) [0x00301] in /home/tcroc/dev/mqttnet-unity-alpn/Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:143 
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Error (MQTTnet.Diagnostics.MqttNetSourceLogger,System.Exception,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:42)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:177)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:455)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:758)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:37)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:46:43.8967740Z] [MqttNet] [33] [MqttClient] [Verbose]: Disconnecting [Timeout=00:01:40]
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose<System.TimeSpan> (MQTTnet.Diagnostics.MqttNetSourceLogger,string,System.TimeSpan) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:112)
MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:513)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61> (MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61&)
MQTTnet.Client.MqttClient:DisconnectCoreAsync (System.Threading.Tasks.Task,System.Exception,MQTTnet.Client.MqttClientConnectResult,bool)
MQTTnet.Client.MqttClient:DisconnectInternalAsync (System.Threading.Tasks.Task,System.Exception,MQTTnet.Client.MqttClientConnectResult) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:570)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:179)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:455)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:758)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:37)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:46:43.8981370Z] [MqttNet] [33] [MqttClient] [Verbose]: Disconnected from adapter.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:521)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:Start<MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61> (MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61&)
MQTTnet.Client.MqttClient:DisconnectCoreAsync (System.Threading.Tasks.Task,System.Exception,MQTTnet.Client.MqttClientConnectResult,bool)
MQTTnet.Client.MqttClient:DisconnectInternalAsync (System.Threading.Tasks.Task,System.Exception,MQTTnet.Client.MqttClientConnectResult) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:570)
MQTTnet.Client.MqttClient/<ConnectAsync>d__48:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:179)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<ConnectInternal>d__60:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:501)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Client.MqttClientConnectResult>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<AuthenticateAsync>d__57:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:455)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.Client.MqttClient/<SendAndReceiveAsync>d__74`1<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:758)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttConnAckPacket>:SetException (System.Exception)
MQTTnet.PacketDispatcher.MqttPacketAwaitable`1/<WaitOneAsync>d__6<MQTTnet.Packets.MqttConnAckPacket>:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/PacketDispatcher/MqttPacketAwaitable.cs:37)
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:46:43.9147250Z] [MqttNet] [34] [MqttClient] [Verbose]: Stopped receiving packets.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Verbose (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:147)
MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:956)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttPacket>:SetResult (MQTTnet.Packets.MqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceivePacketAsync>d__36:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:182)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Adapter.ReceivedMqttPacket>:SetResult (MQTTnet.Adapter.ReceivedMqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceiveAsync>d__42:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:423)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Formatter.ReadFixedHeaderResult>:SetResult (MQTTnet.Formatter.ReadFixedHeaderResult)
MQTTnet.Adapter.MqttChannelAdapter/<ReadFixedHeaderAsync>d__41:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:355)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
MQTTnet.Implementations.MqttTcpChannel/<ReadAsync>d__20:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttTcpChannel.cs:227)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetException (System.Exception)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetException (System.Exception)
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetException (System.Exception)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetException (System.Exception)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()

[2023-05-08T21:46:43.9167740Z] [MqttNet] [34] [MqttClient] [Info]: Disconnected.
UnityEngine.Debug:Log (object)
MqttClientBehaviour/<>c:<Start>b__9_0 (object,MQTTnet.Diagnostics.MqttNetLogMessagePublishedEventArgs) (at Assets/MqttClientBehaviour.cs:92)
MQTTnet.Diagnostics.MqttNetEventLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetEventLogger.cs:61)
MQTTnet.Diagnostics.MqttNetSourceLogger:Publish (MQTTnet.Diagnostics.MqttNetLogLevel,string,object[],System.Exception) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLogger.cs:24)
MQTTnet.Diagnostics.MqttNetSourceLoggerExtensions:Info (MQTTnet.Diagnostics.MqttNetSourceLogger,string) (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Diagnostics/Logger/MqttNetSourceLoggerExtensions.cs:82)
MQTTnet.Client.MqttClient/<DisconnectCoreAsync>d__61:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:548)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Internal.TaskExtensions/<WaitAsync>d__1:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Internal/TaskExtensions.cs:59)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetResult ()
MQTTnet.Client.MqttClient/<TryReceivePacketsAsync>d__82:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Client/MqttClient.cs:958)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Packets.MqttPacket>:SetResult (MQTTnet.Packets.MqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceivePacketAsync>d__36:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:182)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Adapter.ReceivedMqttPacket>:SetResult (MQTTnet.Adapter.ReceivedMqttPacket)
MQTTnet.Adapter.MqttChannelAdapter/<ReceiveAsync>d__42:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:423)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<MQTTnet.Formatter.ReadFixedHeaderResult>:SetResult (MQTTnet.Formatter.ReadFixedHeaderResult)
MQTTnet.Adapter.MqttChannelAdapter/<ReadFixedHeaderAsync>d__41:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Adapter/MqttChannelAdapter.cs:355)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetResult (int)
MQTTnet.Implementations.MqttTcpChannel/<ReadAsync>d__20:MoveNext () (at Assets/Softlinks/MQTTnet/Source/MQTTnet/MQTTnet/Implementations/MqttTcpChannel.cs:227)
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetException (System.Exception)
Mono.Net.Security.MobileAuthenticatedStream/<StartOperation>d__57:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<Mono.Net.Security.AsyncProtocolResult>:SetResult (Mono.Net.Security.AsyncProtocolResult)
Mono.Net.Security.AsyncProtocolRequest/<StartOperation>d__23:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder:SetException (System.Exception)
Mono.Net.Security.AsyncProtocolRequest/<ProcessOperation>d__24:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<System.Nullable`1<int>>:SetException (System.Exception)
Mono.Net.Security.AsyncProtocolRequest/<InnerRead>d__25:MoveNext ()
System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1<int>:SetException (System.Exception)
Mono.Net.Security.MobileAuthenticatedStream/<InnerRead>d__66:MoveNext ()
System.Threading._ThreadPoolWaitCallback:PerformWaitCallback ()
```

</details>

**Example test event for aws console:**

NOTE: The password is ``testpassword`` base64 encoded

```json
{
    "token": "aToken",
    "signatureVerified": false,
    "protocols": [
        "tls",
        "http",
        "mqtt"
    ],
    "protocolData": {
        "tls": {
            "serverName": "serverName"
        },
        "http": {
            "headers": {
                "#{name}": "#{value}"
            },
            "queryString": "?#{name}=#{value}"
        },
        "mqtt": {
            "username": "test",
            "password": "dGVzdHBhc3N3b3Jk",
            "clientId": "testid"
        }
    },
    "connectionMetadata": {
        "id": "UUID"
    }
}
```

**Example result:**

```json
{
  "isAuthenticated": true,
  "principalId": "testid",
  "disconnectAfterInSeconds": 86400,
  "refreshAfterInSeconds": 86400,
  "policyDocuments": [
    {
      "Version": "2012-10-17",
      "Statement": [
        {
          "Effect": "Allow",
          "Action": [
            "iot:Connect"
          ],
          "Condition": {
            "ArnEquals": {
              "iot:LastWillTopic": [
                "arn:aws:iot:us-east-1:144868213084:topic/open/s/${iot:ClientId}"
              ]
            }
          }
        },
        {
          "Effect": "Allow",
          "Action": [
            "iot:Receive"
          ],
          "Resource": [
            "arn:aws:iot:us-east-1:144868213084:topic/open/*"
          ],
          "Condition": {}
        },
        {
          "Effect": "Allow",
          "Action": [
            "iot:Publish"
          ],
          "Resource": [
            "arn:aws:iot:us-east-1:144868213084:topic/open/d/*/${iot:ClientId}",
            "arn:aws:iot:us-east-1:144868213084:topic/open/p/*/${iot:ClientId}",
            "arn:aws:iot:us-east-1:144868213084:topic/open/s/${iot:ClientId}"
          ],
          "Condition": {}
        },
        {
          "Effect": "Allow",
          "Action": [
            "iot:Subscribe"
          ],
          "Resource": [
            "arn:aws:iot:us-east-1:144868213084:topicfilter/open/d/${iot:ClientId}/*",
            "arn:aws:iot:us-east-1:144868213084:topicfilter/open/p/*/*",
            "arn:aws:iot:us-east-1:144868213084:topicfilter/open/s/*",
            "arn:aws:iot:us-east-1:144868213084:topicfilter/open/f/*"
          ],
          "Condition": {}
        }
      ]
    }
  ]
}
```

## Cleanup

The lambda functions, authorizers, and certificates in aws will be deleted.

Run in a bash shell:

```bash
./clean-aws.sh
```