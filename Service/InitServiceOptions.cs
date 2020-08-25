using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Service
{
    public class InitServiceOptions
    {
        public int Port { get; set; }
        public Protocol Protocol { get; set; }
        public Dictionary<string, string> Headers { get; set; }
        public string Prefix { get; set; }
        
        public InitServiceOptions(int port = 3000, Protocol protocol = Protocol.Http, Dictionary<string, string> headers = null, string prefix = "") {
            Port = port;
            Protocol = protocol;
            Headers = headers;
            Prefix = prefix;
        }
    }
}
