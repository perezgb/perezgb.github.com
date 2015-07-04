---
layout: post
title: "ASP.NET 5 Typed Settings with the ConfigurationBinder"
description: ""
category: 
tags: [aspnet5, aspnet, configuration]
---
{% include JB/setup %}

One thing that may seem really small but I really love in the new ASP.NET 5 is the [Configuration API](https://github.com/aspnet/Configuration). The reason I like it so much is I've written something very similar in the past to make up for the lack of flexibility in System.Configuration. So now I get to retire my own implementation and I can just use the new API. Yay, less code I have to maintain on my own.

I'm not going to provide the general introduction to the API, that has been done well [Louis Dejardin](http://whereslou.com/2014/05/23/asp-net-vnext-moving-parts-iconfiguration/) and [Jsinh](http://blog.jsinh.in/asp-net-5-configuration-microsoft-framework-configurationmodel/).

What I would like to talk about is the lesser known ConfigurationBinder. What this handy class does is allow you to take the key-value structure of the IConfiguration interface and use it to assign a typed setting class. So here's what it looks like:

Let's say we define a json file with our settings:
{% highlight json %}
{
    "Server": "PLUTO",
    "Port": 8080
}
{% endhighlight %}

Now let's load that file using the config classes and output the values to the console.

{% highlight csharp %}
IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("MySettings.json")
               .Build();

Console.WriteLine(configuration.Get("server"));
Console.WriteLine(configuration.Get("port"));
{% endhighlight %}

What if we wanted to have a typed settings class. Well, that's straightforward too. First we define the class with the properties mapping to the settings file.

{% highlight csharp %}
public class TypedSettings
{
    public string Server { get; set; }
    public int Port { get; set; }
}
{% endhighlight %}

Now we get the ConfigurationBinder working. It's just a call to a static method specifying the type of the class to read the settings into and the IConfiguration instance:

{% highlight csharp %}
IConfiguration configuration = new ConfigurationBuilder()
               .AddJsonFile("MySettings.json")
               .Build();

TypedSettings settings = ConfigurationBinder.Bind<TypedSettings>(configuration);

Console.WriteLine(settings.Server);
Console.WriteLine(settings.Port);
{% endhighlight %}

The best way to get all the details is to clone the [repository](https://github.com/aspnet/Configuration) and have a look at the unit tests. But let's just go through some of the valid and invalid scenarios.

Properties without a public setter are not valid:
{% highlight csharp %}
public class ComplexOptions
{
    //Exception: private setters not allowed
    public string PrivateSetter { get; private set; }
    //Exception: protected setters not allowed
    public string ProtectedSetter { get; protected set; }
    //Exception internal setters not allowed
    public string InternalSetter { get; internal set; }
    //Static properties work
    public static string StaticProperty { get; set; }

    //Exception: readonly properties not allowed
    public string ReadOnly
    {
        get { return null; }
    }
}
{% endhighlight %}

Nested options are valid with the following exception:
{% highlight csharp %}
public class NestedOptions
{
    //Valid
    public NestedValid NestedValid { get; set; }
    //Invalid: the class has no public ctor, so it can't be instanciated
    public NestedInvalid NestedInvalid { get; set; }
    //Invalid: the framework wouldn't know which class to instantiate
    public ISomeInterface SomeInterface { get; set; }
}

public class NestedValid
{
    public int Integer { get; set; }
}

public class NestedInvalid
{
    private NestedInvalid() {}

    public int Integer { get; set; }
}

public interface ISomeInterface
{
    int Integer { get; set; }
}
{% endhighlight %}

My main goal with this post is just to shed some light on this very useful class that hasn't gotten a lot of attention yet.

Happy configuring!