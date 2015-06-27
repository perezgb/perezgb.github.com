---
layout: post
title: "Understanding how yield keyword works"
description: ""
date: 2008-10-16 12:00:00 UTC
category: 
tags: [.net, c#]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>If you work with C# you must know that in order to traverse a collection using  the &lsquo;foreach&rsquo; construct the collection must implement the <a href="http://msdn.microsoft.com/en-us/library/system.collections.ienumerable.aspx">IEnumerable  interface</a>. There&rsquo;s a lot of documentation about that on the web so I won&rsquo;t  go into it. What I&rsquo;d like to show you is an alternative to the IEnumerable  interface using the &lsquo;yield&rsquo; keyword. Take a look at this code:</p>
<pre title="code" class="brush: csharp">
public class Player
    {
        public string Name { get; set; }

        public Player(string name)
        {
            this.Name = name;
        }
    }

    public class Team
    {
        Player[] players = new Player[3];

        public Team()
        {
            players[0] = new Player(&quot;Gabriel&quot;);
            players[1] = new Player(&quot;Elisa&quot;);
            players[2] = new Player(&quot;Rafaela&quot;);
        }

        public IEnumerator GetEnumerator()
        {
            foreach (Player player in players)
            {
                yield return player;
            }
        }
    }
</pre>
<p>You can now test the code in a foreach like this:</p>
<pre title="code" class="brush: csharp">
 public static void TestYield()
        {
            Team team = new Team();
            foreach (Player player in team)
            {
                Console.WriteLine(player.Name);
            }
        }

</pre>
<p>This is really interesting, right? But how does it work? Shouldn&rsquo;t the iteration  through the players array start from scratch every time you call the method?  Nope! This is where the magic of the yield comes in. Every time the yield is  executed it will &ldquo;pause the state&rdquo; of the iteration and the next time the method  is called it will resume the iteration from the next item. I could even rewrite  my code in the following way and it would have the same effect:</p>
<pre title="code" class="brush: csharp">
public IEnumerator GetEnumerator()
        {
            yield return players[0];
            yield return players[1];
            yield return players[2];
        }

</pre>
<p>This code is only for demonstration purposes, I know that iterating the array  like this wouldn&rsquo;t be very smart :-) I just wanted to show you that the  execution will pause.</p>
<p>This woks because in compile time the method using the yield will be  transformed into a class which implements the IEnumerator interace and it&rsquo;s  stated will be maintained like any other object, that is why you get the  impression the method is paused. It&rsquo;s cool to understand how these things work  under the hood, isn&rsquo;t it?</p>
</div>