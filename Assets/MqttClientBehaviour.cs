using UnityEngine;
using System.Web;
using MQTTnet;
using MQTTnet.Extensions.WebSocket4Net;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using MQTTnet.Diagnostics;
using MQTTnet.Client;
using System;
using System.Collections.Generic;
using System.Net.Security;

public enum TransportImplementation
{
    Dotnet,
    Websocket4net
}

public enum Transport
{
    Tcp,
    Websocket
}

public class MqttClientBehaviour : MonoBehaviour
{
    public string clientId = "testid";
    public string password = "testpassword";
    public string endpoint = "iot-endpoint-here";
    public string rootTopic = "open";
    public string authorizer = "aws-iot-auhtorizer"; // iot authorizer
    public TransportImplementation transportImplementation = TransportImplementation.Websocket4net;
    public Transport transport = Transport.Websocket;

    IMqttClient client;

    float lastConnectionCheckTimeCheck;

    // Update is called once per frame
    void Start()
    {
        UnityEngine.Debug.Log("Running mqtt example application!");

        UnityEngine.Debug.Log($"{Environment.NewLine}==================={Environment.NewLine}");
        UnityEngine.Debug.Log($"Args Used{Environment.NewLine}");
        UnityEngine.Debug.Log($"{nameof(clientId)}:                  {clientId}");
        UnityEngine.Debug.Log($"{nameof(password)}:                  {password}");
        UnityEngine.Debug.Log($"{nameof(endpoint)}:                  {endpoint}");
        UnityEngine.Debug.Log($"{nameof(rootTopic)}:                 {rootTopic}");
        UnityEngine.Debug.Log($"{nameof(authorizer)}:                {authorizer}");
        UnityEngine.Debug.Log($"{nameof(transportImplementation)}:   {transportImplementation}");
        UnityEngine.Debug.Log($"{nameof(transport)}:                 {transport}");
        UnityEngine.Debug.Log($"{Environment.NewLine}==================={Environment.NewLine}");

        var factory = new MqttFactory();

        if (transportImplementation == TransportImplementation.Websocket4net)
        {
            factory = factory.UseWebSocket4Net();
        }

        var optionsBuilder = factory.CreateClientOptionsBuilder();

        switch(transport)
        {
            case Transport.Tcp:
                optionsBuilder = optionsBuilder.WithTcpServer($"{endpoint}", 443);
                break;
            case Transport.Websocket:
                optionsBuilder = optionsBuilder.WithWebSocketServer($"{endpoint}/mqtt");
                break;
        }

        var options = optionsBuilder
            .WithCredentials($"username?x-amz-customauthorizer-name={HttpUtility.UrlEncode(authorizer)}", password)
            .WithTls(
                new MqttClientOptionsBuilderTlsParameters
                {
                    UseTls = true,
                    ApplicationProtocols = new List<SslApplicationProtocol> { new("mqtt") },
                }
            )
            .WithClientId(clientId)
            .WithWillTopic($"{rootTopic}/s/{clientId}")
            .WithWillRetain(false)
            .WithWillPayload(new byte[] { 0 })
            .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .WithProtocolVersion(MqttProtocolVersion.V500)
            .WithKeepAlivePeriod(TimeSpan.FromSeconds(35))
            .WithoutPacketFragmentation()
            .Build();

        var logger = new MqttNetEventLogger("MqttNet");
        logger.LogMessagePublished += (obj, logArgs) => UnityEngine.Debug.Log(logArgs.LogMessage.ToString());

        client = factory.CreateMqttClient(logger);
        client.ConnectAsync(options);
    }

    void OnDisable()
    {
        if (client.IsConnected)
        {
            if (!client.DisconnectAsync().Wait(TimeSpan.FromSeconds(5)))
            {
                Debug.LogError("Timed out trying to disconnect!");
            }
        }
    }

    void Update()
    {
        if (Time.time - lastConnectionCheckTimeCheck > 10)
        {
            Debug.Log($"Client connection status: {client.IsConnected}. Checking again in 10 seconds...");
            lastConnectionCheckTimeCheck = Time.time;
        }
    }
}
