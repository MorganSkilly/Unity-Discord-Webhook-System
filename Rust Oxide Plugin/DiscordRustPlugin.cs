using System.Collections.Generic;
using System.Net;
using System;
using System.IO;
using System.Text;
#if RUST
using Oxide.Core;
using Newtonsoft.Json;
#endif

namespace Oxide.Plugins
{

#if RUST
    [Info("Easy Discord Integration", "MorganSkilly", "1.0.0")]
    [Description("Allows easy communication from a Rust server to a Discord server")]
#endif

#if !RUST
    class DiscordRustPlugin
    {
        static void Main(string[] args)
        {
            EasyDiscordIntegration.Send("Easy Discord Integration Started!");
            EasyDiscordIntegration.Send("Easy Discord Integration Started!", "Username");
        }
    }
#endif

#if RUST
    public class DiscordRustPlugin : RustPlugin
    {
        private const string permUse = "easydiscord.use";
        private void Init()
        {
            EasyDiscordIntegration.Send("Server is starting...", "Rust Server", config.webhook);
            cmd.AddChatCommand(config.command, this, nameof(cmdSendDiscord));
            permission.RegisterPermission(permUse, this);
        }

        private static ConfigData config;

        //adjust these to match your webhook setup in the config file

        private class ConfigData
        {
            [JsonProperty(PropertyName = "Command")]
            public string command;

            [JsonProperty(PropertyName = "Webhook")]
            public string webhook;

            [JsonProperty(PropertyName = "WebhookConnections")]
            public string webhookconnections;
        }

        private ConfigData GetDefaultConfig()
        {
            return new ConfigData
            {
                command = "discord",
                webhook = "DEFAULT WEBHOOK GOES HERE",
                webhookconnections = "CONNECTIONS WEBHOOK GOES HERE",
            };
        }

        private void OnServerInitialized()
        {
            EasyDiscordIntegration.Send("Server started successfully and is ready to join!", "Rust Server", config.webhook);
        }

        private void OnServerSave()
        {
            EasyDiscordIntegration.Send("Server is saving...", "Rust Server", config.webhook);
        }

        private void OnServerShutdown()
        {
            EasyDiscordIntegration.Send("Server is shutting down...", "Rust Server", config.webhook);
        }

        private void OnPlayerConnected(BasePlayer player)
        {
            EasyDiscordIntegration.Send(player.displayName + " has connected to the server.", "Rust Server", config.webhookconnections);
        }

        private void OnPlayerDisconnected(BasePlayer player)
        {
            EasyDiscordIntegration.Send(player.displayName + " has disconnected from the server.", "Rust Server", config.webhookconnections);
        }
        
        private void cmdSendDiscord(BasePlayer player, string command, string[] args)
        {
            if (permission.UserHasPermission(player.UserIDString, permUse) == false)
            {
                Message(player, "Permission");
                return;
            }

            if (args == null || args?.Length < 1)
            {
                Message(player, "Usage");
                return;
            }

            EasyDiscordIntegration.Send(args[0], player.displayName);
        }

        protected override void LoadConfig()
        {
            base.LoadConfig();

            try
            {
                config = Config.ReadObject<ConfigData>();

                if (config == null)
                {
                    LoadDefaultConfig();
                }
            }
            catch
            {
                PrintError("Configuration file is corrupt! Unloading plugin...");
                Interface.Oxide.RootPluginManager.RemovePlugin(this);
                return;
            }

            SaveConfig();
        }

        protected override void LoadDefaultConfig()
        {
            config = GetDefaultConfig();
        }

        protected override void SaveConfig()
        {
            Config.WriteObject(config);
        }
                
        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                {"Usage", "Usage:\n/discord message"},
                {"Permission", "You don't have permission to use that!"},
                {"Error", "Looks as you did something wrong!"}
            }, this);
        }
        
        private void Message(BasePlayer player, string messageKey, params object[] args)
        {
            if (player == null)
            {
                return;
            }

            var message = GetMessage(messageKey, player.UserIDString, args);
            player.SendConsoleCommand("chat.add", (object) 0, (object) message);
        }

        private string GetMessage(string messageKey, string playerID, params object[] args)
        {
            return string.Format(lang.GetMessage(messageKey, this, playerID), args);
        }
    }
#endif

class EasyDiscordIntegration
    {
        //CHANGE THESE!
        private static string defaultWebhook = "DEFAULT WEBHOOK";
        private static string defaultUserAgent = "MorganSkilly Discord Webhook System";
        private static string defaultUserName = "Rust Server";
        private static string defaultAvatar = "https://steamuserimages-a.akamaihd.net/ugc/687094810512264399/04BA8A55B390D1ED0389E561E95775BCF33A9857/";


        #region simple message
        //Send a simple message

        public static string Send(string mssgBody)
        {
            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("username", defaultUserName);
            postParameters.Add("content", mssgBody);
            postParameters.Add("avatar_url", defaultAvatar);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: success");

            //return string with response
            return fullResponse;
        }

        public static string Send(string mssgBody, string userName)
        {
            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("username", userName);
            postParameters.Add("content", mssgBody);
            postParameters.Add("avatar_url", defaultAvatar);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: success");

            //return string with response
            return fullResponse;
        }

        public static string Send(string mssgBody, string userName, string webhook)
        {
            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("username", userName);
            postParameters.Add("content", mssgBody);
            postParameters.Add("avatar_url", defaultAvatar);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(webhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: success");

            //return string with response
            return fullResponse;
        }

        #endregion

        #region send file
        //Send a simple message with an embedded file

        public static string SendFile(
            string mssgBody,
            string filename,
            string fileformat,
            string filepath,
            string application)
        {
            // Read file data
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", filename);
            postParameters.Add("fileformat", fileformat);
            postParameters.Add("file", new FormUpload.FileParameter(data, filename, application/*"application/msexcel"*/));

            postParameters.Add("username", defaultUserName);
            postParameters.Add("content", mssgBody);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: file success");

            //return string with response
            return fullResponse;
        }

        public static string SendFile(
            string mssgBody,
            string filename,
            string fileformat,
            string filepath,
            string application,
            string userName)
        {
            // Read file data
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", filename);
            postParameters.Add("fileformat", fileformat);
            postParameters.Add("file", new FormUpload.FileParameter(data, filename, application/*"application/msexcel"*/));

            postParameters.Add("username", userName);
            postParameters.Add("content", mssgBody);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(defaultWebhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: file success");

            //return string with response
            return fullResponse;
        }

        public static string SendFile(
            string mssgBody,
            string filename,
            string fileformat,
            string filepath,
            string application,
            string userName,
            string webhook)
        {
            // Read file data
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            fs.Close();

            // Generate post objects
            Dictionary<string, object> postParameters = new Dictionary<string, object>();
            postParameters.Add("filename", filename);
            postParameters.Add("fileformat", fileformat);
            postParameters.Add("file", new FormUpload.FileParameter(data, filename, application/*"application/msexcel"*/));

            postParameters.Add("username", userName);
            postParameters.Add("content", mssgBody);

            // Create request and receive response
            HttpWebResponse webResponse = FormUpload.MultipartFormDataPost(webhook, defaultUserAgent, postParameters);

            // Process response
            StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
            string fullResponse = responseReader.ReadToEnd();
            webResponse.Close();
            Console.WriteLine("Discord: file success");

            //return string with response
            return fullResponse;
        }

        #endregion

        public static class FormUpload //formats data as a multi part form to allow for file sharing
        {
            private static readonly Encoding encoding = Encoding.UTF8;
            public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
            {
                string formDataBoundary = String.Format("----------{0:N}", Guid.NewGuid());

                string contentType = "multipart/form-data; boundary=" + formDataBoundary;

                byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

                return PostForm(postUrl, userAgent, contentType, formData);
            }

            private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
            {
                HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

                if (request == null)
                {
                    Console.WriteLine("request is not a http request");
                    throw new NullReferenceException("request is not a http request");
                }

                // Set up the request properties.
                request.Method = "POST";
                request.ContentType = contentType;
                request.UserAgent = userAgent;
                request.CookieContainer = new CookieContainer();
                request.ContentLength = formData.Length;

                // Send the form data to the request.
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }

                return request.GetResponse() as HttpWebResponse;
            }

            private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
            {
                Stream formDataStream = new System.IO.MemoryStream();
                bool needsCLRF = false;

                foreach (var param in postParameters)
                {
                    if (needsCLRF)
                        formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                    needsCLRF = true;

                    if (param.Value is FileParameter)
                    {
                        FileParameter fileToUpload = (FileParameter)param.Value;

                        string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"\r\nContent-Type: {3}\r\n\r\n",
                            boundary,
                            param.Key,
                            fileToUpload.FileName ?? param.Key,
                            fileToUpload.ContentType ?? "application/octet-stream");

                        formDataStream.Write(encoding.GetBytes(header), 0, encoding.GetByteCount(header));

                        formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
                    }
                    else
                    {
                        string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}",
                            boundary,
                            param.Key,
                            param.Value);
                        formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
                    }
                }

                // Add the end of the request.  Start with a newline
                string footer = "\r\n--" + boundary + "--\r\n";
                formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

                // Dump the Stream into a byte[]
                formDataStream.Position = 0;
                byte[] formData = new byte[formDataStream.Length];
                formDataStream.Read(formData, 0, formData.Length);
                formDataStream.Close();

                return formData;
            }

            public class FileParameter
            {
                public byte[] File { get; set; }
                public string FileName { get; set; }
                public string ContentType { get; set; }
                public FileParameter(byte[] file) : this(file, null) { }
                public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
                public FileParameter(byte[] file, string filename, string contenttype)
                {
                    File = file;
                    FileName = filename;
                    ContentType = contenttype;
                }
            }
        }
    }

}
