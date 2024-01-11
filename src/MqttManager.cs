namespace WPFordle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;

public class MqttManager
{

    public string mqttServerAddress = "s8f1e891.ala.us-east-1.emqxsl.com";
    public int? mqttServerPort = 8883;
    public string mqttClientID = "wordle-client"; // Guid.NewGuid().ToString();
    public string mqttUsername = "device1";
    public string mqttPassword = "getmqtt";
    public IMqttClient? mqttClient;
    private readonly MqttFactory mqttFactory = new();

    public MqttManager()
    {
        mqttClient = mqttFactory.CreateMqttClient();
    }

    public async Task ConnectClient()
    {
        try
        {
            MqttClientOptions? mqttClientOptions = new MqttClientOptionsBuilder()
                .WithClientId(mqttClientID)
                .WithTcpServer(mqttServerAddress, mqttServerPort)
                .WithCredentials(mqttUsername, mqttPassword)
                .WithTlsOptions(o =>
                {
                    // accepts all certificates
                    o.WithCertificateValidationHandler(_ => true);
                    o.WithSslProtocols(System.Security.Authentication.SslProtocols.Tls12);
                })
                // .WithWillQualityOfServiceLevel(MQTTnet.Protocol.MqttQualityOfServiceLevel.AtLeastOnce)
                // .WithCleanSession()
                .Build();

            //AmongUs_WPF.MainWindow.Current.SetText1("attempting mqtt connect");

            // TODO: Exception handling if connection fails - can I? are we using mqtt 3.1 or 5?
            // Connect with TLS
            using var timeout = new CancellationTokenSource(5000);
            MqttClientConnectResult connectResponse = await mqttClient.ConnectAsync(mqttClientOptions, timeout.Token);
            if (connectResponse.ResultCode == MqttClientConnectResultCode.Success)
            {
                //AmongUs_WPF.MainWindow.Current.SetText2($"Connected");
            }
            else
            {
                //AmongUs_WPF.MainWindow.Current.SetText2($"Connection failed: {connectResponse.ResultCode}");
            }
        }
        catch (Exception ex)
        {
            //AmongUs_WPF.MainWindow.Current.SetText3($"Exception: {ex}");

        }
    }


    public async Task PublishTestDev(string message)
    {

        await Publish("test/dev", message);
    }

    private async Task Publish(string channel, string message)
    {
        if (mqttClient?.IsConnected ?? false)
        {
            //AmongUs_WPF.MainWindow.Current.SetText1($"Publish {channel}: `{message}`");
            try
            {
                var applicationMessage = new MqttApplicationMessageBuilder()
                .WithTopic(channel)
                .WithPayload(message)
                .Build();

                using var timeout = new CancellationTokenSource(5000);
                MqttClientPublishResult publishResult = await mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                //AmongUs_WPF.MainWindow.Current.SetText3($"Exception: {ex}");
            }
        }
        else
        {
            //AmongUs_WPF.MainWindow.Current.SetText1($"Publish Failed - Not Connected");
        }
    }

    public async Task DisconnectClient()
    {
        if (mqttClient?.IsConnected ?? false)
        {
            await mqttClient.DisconnectAsync();
        }
    }

}
