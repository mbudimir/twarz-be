﻿namespace Twarz.API.Domains
{
    public class MessageRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string DeviceToken { get; set; }
    }
}
