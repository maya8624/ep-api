﻿global using AutoMapper;
global using ep.API.Service.Hubs;
global using ep.Logic.Interfaces;
global using ep.Contract.RequestModels;
global using ep.Data.Persistent;
global using ep.Domain.Models;
global using ep.Contract.Constraints;
global using ep.Contract.Extensions;
global using ep.Contract.ViewModels;
global using ep.Data.Wrappers;
global using ep.Framework;
global using ep.Framework.Exceptions;
global using ep.Service.Cryptograph;
global using Microsoft.AspNetCore.Http;
global using Microsoft.AspNetCore.SignalR;
global using Microsoft.Extensions.Configuration;
global using Microsoft.IdentityModel.Tokens;
global using Newtonsoft.Json;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Text;
