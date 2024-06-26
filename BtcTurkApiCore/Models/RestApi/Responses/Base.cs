﻿using System.Text.Json.Serialization;

namespace BtcTurkApiCore.Models.Api.Responses;

public class Base
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    [JsonPropertyName("message")]
    public string Message { get; set; }
    
    [JsonPropertyName("code")]
    public int Code { get; set; }
}