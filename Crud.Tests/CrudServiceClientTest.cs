using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NUnit.Framework;
using WireMock.Server;

namespace Crud.Tests
{
    public class Tests
    {
        private WireMockServer _server;
        private CrudServiceClient _sut;
        private HttpRequestHeaders _httpRequestHeaders = null;
        private const int SUCCESS_STATUS_CODE = 200;
        private const string SUCCESS_RESPONSE_BODY = @"{ ""msg"": ""Hello world!"" }";
        
        [SetUp]
        public void StartMockServer()
        {
            _server = WireMockServer.Start();
        }

        class Character
        {
            [JsonPropertyName("id")]
            public int Id { get; set; }    
            [JsonPropertyName("firstname")]
            public string Firstname { get; set; } 
            [JsonPropertyName("Lastname")]
            public string Lastname { get; set; } 
            [JsonPropertyName("status")]
            public string Status { get; set; }    
        }

        [Test]
        public async Task Test1()
        {
            _sut = new CrudServiceClient(new Dictionary<string, string>(), "http://localhost:3001/foo");
            var result = await _sut.RetrieveAll<Character>();

            Console.WriteLine(result);
        }
        
        [TearDown]
        public void ShutdownServer()
        {
            _server.Stop();
        }
    }
}