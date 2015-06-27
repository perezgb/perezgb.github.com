---
layout: post
title: "C# Custom Type Conversions"
description: ""
date: 2008-10-22 12:00:00 UTC
category: 
tags: [.net, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>C# allows for implicit and explicit type conversion between classes when there  is an inheritance situation. Something like this:</p>
<pre title="code" class="brush: csharp">
class BaseClass { }

    class DerivedFromBaseClass : BaseClass { }

    class Program
    {
        static void Main(string[] args)
        {
            BaseClass bc = new DerivedFromBaseClass(); //implicit conversion

            DerivedFromBaseClass dbc = (DerivedFromBaseClass)bc; //explicit conversion
        }
    }

</pre>
<p>Ok, this shouldn&rsquo;t be anything new to know since this is a feature present in  all object oriented languages I know. But now I want to talk about a stranger  kind of conversion and for this I&rsquo;ll have to mix apples and oranges.</p>
<p>Let&rsquo;s define to classes: Apple and Orange.</p>
<pre title="code" class="brush: csharp">
class Apple
    {
        public string Farm { get; set; }

        public Apple(string farm)
        {
            this.Farm = farm;
        }

        public override string ToString()
        {
            return string.Format(&quot;I'm an apple from the {0} farm&quot;, this.Farm);
        }

    }

    class Orange
    {
        public string Farm { get; set; }

        public Orange(string farm)
        {
            this.Farm = farm;
        }

        public override string ToString()
        {
            return string.Format(&quot;I'm an orange from the {0} farm&quot;, this.Farm);
        }
    }
</pre>
<p>As you can see these two classes are totally unrelated, they don&rsquo;t have any  inheritance relation with each other.</p>
<p>Let&rsquo;s convert Apples to Oranges using the cast-like sintax. This is possible  using the keywords &rsquo;&rsquo;&lsquo;operator&rsquo;&rsquo;&rsquo;, &rsquo;&rsquo;&lsquo;explicit&rsquo;&rsquo;&rsquo; in a static method that will  do the conversion. Here are the changes to the Apple class so that it can be  converted explicitly to an Orange.</p>
<pre title="code" class="brush: csharp">
class Apple
    {
        public string Farm { get; set; }

        public Apple(string farm)
        {
            this.Farm = farm;
        }

        public override string ToString()
        {
            return string.Format(&quot;I'm an apple from the {0} farm&quot;, this.Farm);
        }

        public static explicit operator Apple(Orange o)
        {
            Apple a = new Apple(o.Farm);
            return a;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Orange o = new Orange(&quot;GreatSouth&quot;);
            Console.WriteLine(o.ToString());

            Apple a = (Apple)o; //explicit conversion
            Console.WriteLine(a.ToString());
        }
    }
</pre>
<p>Ok, it works. You can also do an implicit conversion but in this case you must  be aware that it will also create (automatically) the explicit conversion. This  is why you can&rsquo;t define both these conversions in the same class. Following is  the changes necessary to the Orange class so that you can make implicit and  explicit conversions from Apples to Oranges:</p>
<pre title="code" class="brush: csharp">
class Orange
    {
        public string Farm { get; set; }

        public Orange(string farm)
        {
            this.Farm = farm;
        }

        public override string ToString()
        {
            return string.Format(&quot;I'm an orange from the {0} farm&quot;, this.Farm);
        }

        //this will allow implicit and explicit conversion
        public static implicit operator Orange(Apple a)
        {
            Orange o = new Orange(a.Farm);
            return o;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Apple a = new Apple(&quot;GreatSouth&quot;);
            Console.WriteLine(a.ToString());

            Orange o = a; //implicit
            Console.WriteLine(o.ToString());

            //or you could do it like this
            o = (Orange)a; //explicit
            Console.WriteLine(o.ToString());
        }
    }</pre>
<p>I hope you found this interesting. Once again a know that my examples are not  always the most thought out but I&rsquo;m certain that you got the idea and will be  able to put it work in a real life situation.</p>
</div>