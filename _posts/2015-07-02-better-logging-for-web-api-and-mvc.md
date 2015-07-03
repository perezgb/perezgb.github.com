---
layout: post
title: "Better logging for Web API and MVC"
description: ""
category: 
tags: [nlog, webapi, mvc, logging]
---
{% include JB/setup %}

So I'll try not to be too long with this one. Logging is good thing, I guess there's not argument there. Who am I kidding, we're developers, we can make an argument out of anything. Spaces vs tabs... Don't worry I'm not going there.

Anyways, when you're testing a Web application, be it an API or an MVC one, going through the logs is only simple when your the only one hitting the app sequentially. All the request and responses are neatly separated and parsing the log is a breeze. When you have a production app, that is totally not the case. Multiple requests come in at the same time and you're left scrambling trying to figure out to which request each message belongs. Let's see an example of this. Here's a sample controller from a Web API project with a single log message:

{% highlight csharp %}
public class ValuesController : ApiController
{
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    public async Task<HttpResponseMessage> Get()
    {
        Log.Debug("Hi there");
        return Request.CreateResponse(HttpStatusCode.OK, new[] { "one", "two" });
    }
}
{% endhighlight %}

If we hit that endpoint once this is what we get:

>2015-07-02 22:57:16.6195 DEBUG Hi there

Now, wouldn't it be nice if we could the address for the request, the verb used and how long it took to execute? And even better, what if we had a unique identifier included in every log message that would allow us to easly parse our log? So check this out:

>2015-07-02 22:58:43.9794|c265a13b-99bd-494e-b83a-4c4b1a891d4b|TRACE|Requesting:[GET]http://localhost:57577/api/values
>2015-07-02 22:58:44.0605|c265a13b-99bd-494e-b83a-4c4b1a891d4b|DEBUG|Hi there
>2015-07-02 22:58:44.0805|c265a13b-99bd-494e-b83a-4c4b1a891d4b|TRACE|Request completed. Duration:00:00:00.1019623 StatusCode:OK

That's much better than before. Now, every log message in the request has the same guid, so it's easy to parse your log and find the messages you want. We also have a line defining the beggining of the request and one defining the end of the request. All this can be achieved without changing any of your existing log message. Here's what we need to get this going:

1-Install the following packages.
 - NLog
 - NLog.Contrib

2-Add a DelegatingHandler that will tap in the request pipeline to do the logging:

{% highlight csharp %}
public class LoggerDelegatingHandler : DelegatingHandler
{
    private static readonly Logger Log = LogManager.GetLogger("RequestTracer");

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var stopwatch = new Stopwatch();

        Guid guid = Guid.NewGuid();
        NLog.Contrib.MappedDiagnosticsLogicalContext.Set("requestid", guid.ToString());
        stopwatch.Start();
        Log.Trace("Requesting:[{0}]{1}", request.Method.Method, request.RequestUri);

        var reponse = await base.SendAsync(request, cancellationToken);

        stopwatch.Stop();
        Log.Trace("Request completed. Duration:{0} StatusCode:{1}", (object) stopwatch.Elapsed, (object) reponse.StatusCode);
        return reponse;
    }
}
{% endhighlight %}

This delegating handler intercepts every request and appends a message before and after the action methods of the controllers. The magic here in the line that configures the requestid for NLog. This will append that id to the context of every log message.

3-Change your Application_Start to hookup the handler and initialize NLog:

{% highlight csharp %}
public class WebApiApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        //initialize the NLog config
        NLog.Config.ConfigurationItemFactory.Default.LayoutRenderers.RegisterDefinition("mdlc", typeof(MdlcLayoutRenderer));

        GlobalConfiguration.Configure(configuration =>
        {
        	//add the custom handler to the message handler pipeline
            configuration.MessageHandlers.Add(new LoggerDelegatingHandler());

            WebApiConfig.Register(configuration);
        });
    }
}
{% endhighlight %}

4-Modify the NLog.config file to display your newly defined renderer:

{% highlight xml %}
<target type="File" name="f" fileName="${basedir}/logs/${shortdate}.log"
        layout="${longdate}|${mdlc:item=requestid}|${uppercase:${level}}|${message}" />
{% endhighlight %}

I won't go into much detail about how this works because you can read it directly from the source [here](http://www.bfcamara.com/post/67752957760/nlog-how-to-include-custom-context-variables-in). That post will answer all your questions about how this really works in NLog.

You can get the full sample from my GitHub [samples repository](https://github.com/perezgb/samples/tree/master/WebApiLogging).

I think I ran a little longer that I expected but I hope it was worth it. Happy logging. 