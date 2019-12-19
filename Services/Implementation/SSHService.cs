using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Transactions;
using EsxiRestfulApi.Services.Interface;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.Configuration;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace EsxiRestfulApi.Services.Implementation
{
    public class SSHService : ISSHService
    {
        private readonly IConfiguration _config;

        public SSHService(IConfiguration config)
        {
            _config = config;
        }
        
        public string ExecuteCommand(string command)
        {
            string host = _config.GetValue<string>("ESXI:host");
            string password = _config.GetValue<string>("ESXI:password");
            
            var pk = new PrivateKeyFile("/home/mousey/.ssh/esxi_pem_rsa");
            var keyFiles = new[] {pk};

            var methods = new List<AuthenticationMethod>();
            methods.Add(new PrivateKeyAuthenticationMethod("root", keyFiles));
            
            var connectionInfo = new ConnectionInfo(host, 22, "root", methods.ToArray());

            using (var client = new SshClient(connectionInfo))
            {
                client.Connect();
                var cmd = client.CreateCommand(command);
                var result = cmd.Execute();

                client.Disconnect();
                return cmd.Result;
            }
        }

        private void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = _config.GetValue<String>("ESXI:password");
                }
            }
        }
    }
}