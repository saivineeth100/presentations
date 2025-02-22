﻿// Copyright Information
// ==================================
// AutoLot - AutoLot.Api - SwaggerApplicationSettings.cs
// All samples copyright Philip Japikse
// http://www.skimedic.com 2022/08/09
// ==================================

namespace AutoLot.Api.Swagger.Models;
public class SwaggerApplicationSettings
{
    public string Title { get; set; }
    public List<SwaggerVersionDescription> Descriptions { get; set; } = new List<SwaggerVersionDescription>();
    public string ContactName { get; set; }
    public string ContactEmail {get; set; }
}