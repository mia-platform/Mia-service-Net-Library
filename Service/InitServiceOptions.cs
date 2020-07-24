using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Service
{
    public class InitServiceOptions
    {
        public int Port { get; set; }
        public Protocol Protocol { get; set; }
        public HttpRequestHeaders Headers { get; set; }
        public String Prefix { get; set; }
        
        public InitServiceOptions(int port = 3000, Protocol protocol = Protocol.HTTP, HttpRequestHeaders headers = null, String prefix = "") {
            Port = port;
            Protocol = protocol;
            Headers = headers;
            Prefix = prefix;
        }
    }
}