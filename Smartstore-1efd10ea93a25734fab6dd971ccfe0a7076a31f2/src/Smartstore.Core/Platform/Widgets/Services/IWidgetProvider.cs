﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace Smartstore.Core.Widgets
{
    /// <summary>
    /// Allows request scoped registration of custom components, whose results get injected into widget zones.
    /// </summary>
    public interface IWidgetProvider
    {
        /// <summary>
        /// Reads all known widgetzones from the json file /App_Data/widgetzones.json
        /// </summary>
        Task<dynamic> GetAllKnownWidgetZonesAsync();

        /// <summary>
        /// Registers a custom widget for widget zones
        /// </summary>
        /// <param name="zones">The names of the widget zones to inject the HTML content to</param>
        /// <param name="widget">Widget to register</param>
        void RegisterWidget(string[] zones, WidgetInvoker widget);

        /// <summary>
        /// Registers a custom widget for multiple widget zones by pattern
        /// </summary>
        /// <param name="zones">The widget zone pattern to inject the HTML content to</param>
        /// <param name="widget">Widget to register</param>
        void RegisterWidget(Regex zonePattern, WidgetInvoker widget);

        /// <summary>
        /// Checks whether a given zone has content (contains at least one widget),
        /// </summary>
        /// <param name="zone">The zone name to check.</param>
        bool HasContent(string zone);

        /// <summary>
        /// Enumerates all injected widgets for a given zone.
        /// </summary>
        /// <param name="zone">Zone name to retrieve widgets for.</param>
        /// <returns>List of <see cref="WidgetInvoker"/> instances.</returns>
        IEnumerable<WidgetInvoker> GetWidgets(string zone);

        /// <summary>
        /// Checks whether given <paramref name="zone"/> contains a widget
        /// with the same <see cref="WidgetInvoker.Key"/> as <paramref name="widgetKey"/>.
        /// </summary>
        /// <param name="zone">The zone name to check for existing widget.</param>
        /// <param name="widgetKey">The widget key to check.</param>
        bool ContainsWidget(string zone, string widgetKey);
    }

    public static class IWidgetProviderExtensions
    {
        /// <summary>
        /// Registers a custom widget for a single widget zone.
        /// </summary>
        /// <param name="zone">The name of the widget zone to inject the HTML content to</param>
        /// <param name="widget">Widget to register</param>
        public static void RegisterWidget(this IWidgetProvider provider, string zone, WidgetInvoker widget)
        {
            Guard.NotEmpty(zone, nameof(zone));
            provider.RegisterWidget(new[] { zone }, widget);
        }

        /// <summary>
        /// Registers custom HTML content for a single widget zone
        /// </summary>
        /// <param name="zone">The name of the widget zones to inject the HTML content to</param>
        /// <param name="html">HTML to inject</param>
        /// <param name="order">Sort order within the specified widget zone</param>
        public static void RegisterHtml(this IWidgetProvider provider, string zone, IHtmlContent html, int order = 0)
        {
            Guard.NotEmpty(zone, nameof(zone));
            provider.RegisterWidget(new[] { zone }, new HtmlWidgetInvoker(html) { Order = order });
        }

        /// <summary>
        /// Registers custom HTML content for widget zones
        /// </summary>
        /// <param name="zones">The names of the widget zones to inject the HTML content to</param>
        /// <param name="html">HTML to inject</param>
        /// <param name="order">Sort order within the specified widget zones</param>
        public static void RegisterHtml(this IWidgetProvider provider, string[] zones, IHtmlContent html, int order = 0)
        {
            provider.RegisterWidget(zones, new HtmlWidgetInvoker(html) { Order = order });
        }

        /// <summary>
        /// Registers custom HTML content for multiple widget zones by pattern
        /// </summary>
        /// <param name="zonePattern">The widget zone pattern to inject the HTML content to</param>
        /// <param name="html">HTML to inject</param>
        /// <param name="order">Sort order within the specified widget zones</param>
        public static void RegisterHtml(this IWidgetProvider provider, Regex zonePattern, IHtmlContent html, int order = 0)
        {
            provider.RegisterWidget(zonePattern, new HtmlWidgetInvoker(html) { Order = order });
        }

        /// <summary>
        /// Registers a view component for a single widget zone
        /// </summary>
        /// <param name="zone">The name of the widget zones to inject the view component to</param>
        /// <param name="arguments">
        /// An <see cref="object"/> with properties representing arguments to be passed to the invoked view component
        /// method. Alternatively, an <see cref="IDictionary{String, Object}"/> instance
        /// containing the invocation arguments.
        /// </param>
        /// <param name="order">Sort order within the specified widget zone</param>
        public static void RegisterViewComponent<TComponent>(this IWidgetProvider provider, string zone, object arguments = null, int order = 0)
            where TComponent : ViewComponent
        {
            Guard.NotEmpty(zone, nameof(zone));
            provider.RegisterWidget(new[] { zone }, new ComponentWidgetInvoker(typeof(TComponent), arguments) { Order = order });
        }

        /// <summary>
        /// Registers a view component for a single widget zone
        /// </summary>
        /// <param name="zones">The names of the widget zones to inject the view component to</param>
        /// <param name="arguments">
        /// An <see cref="object"/> with properties representing arguments to be passed to the invoked view component
        /// method. Alternatively, an <see cref="IDictionary{String, Object}"/> instance
        /// containing the invocation arguments.
        /// </param>
        /// <param name="order">Sort order within the specified widget zone</param>
        public static void RegisterViewComponent<TComponent>(this IWidgetProvider provider, string[] zones, object arguments = null, int order = 0)
            where TComponent : ViewComponent
        {
            provider.RegisterWidget(zones, new ComponentWidgetInvoker(typeof(TComponent), arguments) { Order = order });
        }

        /// <summary>
        /// Registers a view component for a single widget zone
        /// </summary>
        /// <param name="zonePattern">The widget zone pattern to inject the view component to</param>
        /// <param name="arguments">
        /// An <see cref="object"/> with properties representing arguments to be passed to the invoked view component
        /// method. Alternatively, an <see cref="IDictionary{String, Object}"/> instance
        /// containing the invocation arguments.
        /// </param>
        /// <param name="order">Sort order within the specified widget zone</param>
        public static void RegisterViewComponent<TComponent>(this IWidgetProvider provider, Regex zonePattern, object arguments = null, int order = 0)
            where TComponent : ViewComponent
        {
            provider.RegisterWidget(zonePattern, new ComponentWidgetInvoker(typeof(TComponent), arguments) { Order = order });
        }
    }
}