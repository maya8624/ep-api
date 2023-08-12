﻿global using AutoMapper;
global using Azure.Identity;
global using ep.API.Extensions;
global using ep.API.Service.Hubs;
global using ep.Contract.RequestModels;
global using ep.Contract.ViewModels;
global using ep.Data.Persistent;
global using ep.Data.Wrappers;
global using ep.Domain.Models;
global using ep.Service.Email;
global using ep.Service.Interfaces;
global using ep.Service.Services;
global using FluentValidation.AspNetCore;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Filters;
global using System;
global using System.Collections.Generic;
global using System.Threading.Tasks;
global using System.Text;