---
layout: post
title: "Serializing Custom Enumeration With Json.NET"
description: ""
date: 2012-04-03 12:00:00 UTC
category: 
tags: [serialization, json.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>When I went to Codemash a few months ago I watched <a href="http://lostechies.com/jimmybogard/">Jimmy Bogard&rsquo;s</a> <a href="http://youtu.be/GubLNJL47K8">Crafting Wicked Domain Models</a> talk. One of the interesting things in his talk was his implementation of a Java like <em>enum</em>. It was something I had seen before with varying implementations but I had never felt really compelled to switch to. What changed my mind was <a href="http://msmvps.com/blogs/jon_skeet/">Jon Skeet&rsquo;s</a> talk on C# Greatest Mistakes where he showed several problems with the C# implementation of <em>enums</em>. After that I decided to start using Jimmy&rsquo;s implementation full time.</p>
<p>Last week I came across a serialization issue using <a href="https://github.com/JamesNK/Newtonsoft.Json">Json.Net</a> with it though. Look at this sample implementation:</p>
<pre title="code" class="brush: csharp">public class ColorEnum : Enumeration
{
    public static readonly ColorEnum Black = new ColorEnum(1,"Black");

    public static readonly ColorEnum White = new ColorEnum(2,"White");
    
    [JsonConstructor]
    private ColorEnum(int value, string displayName) : base(value, displayName)
    {
    }
}</pre>
<p>The code for the Enumeration class can be found here: <a href="https://github.com/jbogard/presentations/blob/master/WickedDomainModels/After/Model/Enumeration.cs">https://github.com/jbogard/presentations/blob/master/WickedDomainModels/After/Model/Enumeration.cs</a></p>
<p>First, notice that in order to be able to deserialize it properly I had to add the JsonConstructor attribute to the constructor. But that&rsquo;s not my issue yet.</p>
<p>My first issue is that the deserialized object will be a new instance of my ColorEnum item. That&rsquo;s not bad as the Enumeration class is prepared to handle it by overriding the Equals method like this:</p>
<pre title="code" class="brush: csharp">public override bool Equals(object obj)
{
    var otherValue = obj as Enumeration;

    if (otherValue == null)
    {
        return false;
    }

    var typeMatches = GetType().Equals(obj.GetType());
    var valueMatches = _value.Equals(otherValue.Value);

    return typeMatches &amp;&amp; valueMatches;
}
</pre>
<p>It would still fail however if we did a comparison using the &ldquo;==&rdquo; operator. But that can still be fixed by overriding the operator in the Enumeration class like so:</p>
<pre title="code" class="brush: csharp">public static bool operator ==(Enumeration left, Enumeration right)
{
    return Equals(left, right);
}

public static bool operator !=(Enumeration left, Enumeration right)
{
    return !Equals(left, right);
}</pre>
<p>So if we can get away with multiple instances why would I want to use a single instance? Because I can :-)</p>
<p>The neat thing by using a single instance I can make the serialization look more like with the regular enum. So here what it would be like when serialized:</p>
<blockquote>
<p>{"Color":{"Value":1,"DisplayName":"Black"}}</p>
</blockquote>
<p>And this is how I&rsquo;d like it to be:</p>
<blockquote>
<p>{"Color":1}</p>
</blockquote>
<p>So how can we achieve this? Json.Net custom converters.</p>
<p>Custom converters are not complicated to implement, the one I created to take care of serializing my enumeration is as follows:</p>
<pre title="code" class="brush: csharp">public class EnumerationConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var enumeration = (Enumeration)value;
        serializer.Serialize(writer, enumeration.Value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null)
        {
            return null;
        }

        int value = serializer.Deserialize&lt;int&gt;(reader);
        foreach (Enumeration enumeration in Enumeration.GetAll(objectType))
        {
            if (enumeration.Value == value)
            {
                return enumeration;
            }
        }

        throw new Exception("Value not found in enumeration. Type:{0} Value:{1}".Frmt(objectType, value));
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType.IsSubclassOf(typeof(Enumeration));
    }
}
</pre>
<p>During deserialization I use the GetAll method from the Enumeration class to retrieve all the enumerated items for the specific type and try to match it's values to the value being deserialized. With a few tests we can easily prove that we get our expected results:</p>
<pre title="code" class="brush: csharp">[TestFixture]
public class EnumerationConverterTests
{
    [Test]
    public void Should_serialize_Enumeration_to_simplified_json()
    {
        var brush = new Brush {Color = ColorEnum.Black};
        string json = JsonConvert.SerializeObject(brush, new EnumerationConverter());
        Assert.AreEqual(@"{""Color"":1}", json);
    }

    [Test]
    public void Should_serialize_null_Enumeration()
    {
        var brush = new Brush();
        string json = JsonConvert.SerializeObject(brush, new EnumerationConverter());
        Assert.AreEqual(@"{""Color"":null}", json);
    }

    [Test]
    public void Should_deserialize_Enumeration()
    {
        string json = @"{""Color"":1}";
        var deserializeObject = JsonConvert.DeserializeObject&lt;Brush&gt;(json, new EnumerationConverter());
        Assert.AreEqual(ColorEnum.Black, deserializeObject.Color);
    }

    [Test]
    public void Should_deserialize_null_Enumeration()
    {
        string json = @"{""Color"":null}";
        var deserializeObject = JsonConvert.DeserializeObject&lt;Brush&gt;(json, new EnumerationConverter());
        Assert.IsNull(deserializeObject.Color);
    }

    public class Brush
    {
        public ColorEnum Color { get; set; } 
    }

    public class ColorEnum : Enumeration
    {
        public static readonly ColorEnum Black = new ColorEnum(1,"Black");

        public static readonly ColorEnum White = new ColorEnum(2,"White");
    
        [JsonConstructor]
        private ColorEnum(int value, string displayName) : base(value, displayName)
        {
        }
    }
}
</pre>
<p>I hope this was useful information. See you next time.</p>
</div>