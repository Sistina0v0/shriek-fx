﻿using Microsoft.Extensions.DependencyInjection;
using Shriek.ServiceProxy.Abstractions;
using System;
using System.Linq;

namespace Shriek.ServiceProxy.Http
{
    public static class WebApiProxyExtensions
    {
        public static IShriekBuilder AddWebApiProxy(this IShriekBuilder builder, Action<WebApiProxyOptions> optionAction)
        {
            var option = new WebApiProxyOptions();
            optionAction(option);

            foreach (var o in option.WebApiProxies)
            {
                var types = o.GetType().Assembly.GetTypes().Where(x =>
                    x.IsInterface && x.GetMethods().SelectMany(m => m.GetCustomAttributes(typeof(ApiActionAttribute), true)).Any());
                foreach (var t in types)
                {
                    builder.Services.AddScoped(t, x => ServiceProvider.GetHttpService(t, o.BaseUrl));
                }
            }

            return builder;
        }
    }
}