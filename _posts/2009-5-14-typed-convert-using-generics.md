---
layout: post
title: "Typed Convert using Generics"
description: ""
date: 2009-05-14 12:00:00 UTC
category: 
tags: [c#, generics]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Last night I answerd a interesting question on the Asp.Net Forums that I thought is worth a post. The objective was to create a method using Generics that could get a value from the Session only instead of returning a Object it would return the value in the type defined in the generic method. Just to make the demonstration easier I'll use a Dictionary instead of the Session but it works just the same:</p>
<pre title="code" class="brush: csharp">
class TypeTest
{
    private static Dictionary&lt;string, object&gt; PretendSession = new Dictionary&lt;string, object&gt;();

    public static void Test()
    {
        PretendSession.Add(&quot;int&quot;, 10M);
        Console.Out.WriteLine(Get&lt;int&gt;(&quot;int&quot;));
    }

    public static T Get&lt;T&gt;(string theKey)
    {
        object aSessionObject = PretendSession[theKey];

        if (aSessionObject == null)
        {
            return default(T);
        }

        return (T) aSessionObject;
    }
}
</pre>
<p>This code works well for most cases but you can get in trouble when you work with Value Types. Look at code that follows and to undestand the problems with boxing and unboxing value types:</p>
<pre title="code" class="brush: csharp">
 //doesn't work, need explicit cast 
int i = 10M; 

//now it's fine 
int i = (int)10M; 

//boxing and unboxing work fine 
object o = 10; 
int i = (int)o; 

//doesn't work 
object o = 10M; 
int i = (int)o;</pre>
<p>When you know exactaly what is the type of you are putting inside the Session it all goes fine. When you don't know what type you are working with you need the help of the Convert class to convert the type before casting it. To do this we test if the object is of type ValueType and if it is, before casting it we use the ChangeType method of the Convert class to make sure that the object really contains the type we will try to cast it to.</p>
<p>&nbsp;</p>
<pre title="code" class="brush: csharp">
class TypeTest
{
    static Dictionary&lt;string, object&gt; PretendSession = new Dictionary&lt;string, object&gt;();

    public static void Test()
    {
        PretendSession.Add(&quot;int&quot;, 10M);
        Console.Out.WriteLine(Get&lt;int&gt;(&quot;int&quot;));
    }

    public static T Get&lt;T&gt;(string theKey)
    {
        object aSessionObject = PretendSession[theKey];

        if (aSessionObject == null)
        {
            return default(T);
        }

        if (aSessionObject is ValueType)
        {
            return (T)Convert.ChangeType(aSessionObject, typeof(T));
        }

        return (T)aSessionObject;
    }
}
</pre>
<p>Now you should be safe to use your Get&lt;T&gt; method. Hope this can help other people that face this same issue.</p>
</div>