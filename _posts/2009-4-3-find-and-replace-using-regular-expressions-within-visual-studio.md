---
layout: post
title: "Find and Replace using Regular Expressions within Visual Studio"
description: ""
date: 2009-04-03 12:00:00 UTC
category: 
tags: [.net, visualstudio, regex]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>Regular expressions can be a great addition to the Find and Replace  functionality within Visual Studio, specially when you want do do some  transformation on the text you are looking up. Lately I&rsquo;m working on a simple  conversion class to transform some <span class="caps">HTML</span> into <span class="caps">RTF</span>. I got a list of special characters in <span class="caps">HTML</span> that I had to convert to the <span class="caps">RTF</span>  equivalent and for that I needed to create a dictionary. The values where listed  in a text document like this:</p>
<pre class="brush: csharp" title="code">
\'b0 \tab &amp;deg;\par
\'b1 \tab &amp;plusmn;\par
\'b2 \tab &amp;sup2;\par
\'b3 \tab &amp;sup3;\par
\'b4 \tab &amp;acute;\par
\'b5 \tab &amp;micro;\par
\'b6 \tab &amp;para;\par
\'b7 \tab &amp;middot;\par
\'b8 \tab &amp;cedil;\par
\'b9 \tab &amp;sup1;\par
\'ba \tab &amp;ordm;\par
\'bb \tab &amp;raquo;\par
\'bc \tab &amp;frac14;\par
\'bd \tab &amp;frac12;\par
\'be \tab &amp;frac34;\par
</pre>
<p>And I needed to create a dictionary with the key being the <span class="caps">HTML</span> code (third column) and the <span class="caps">RTF</span>  code (first column) the value. Like this:</p>
<pre class="brush: csharp" title="code">
Dictionary&lt;string, string&gt; charsMapping = 
    new Dictionary&lt;string, string&gt;();
charsMapping.Add(&quot;&amp;euro;&quot;, @&quot;\'80&quot;);
</pre>
<p>Here is the expression to find the matches within Visual Studio: __  {\\&rsquo;:a:a}{.<ins>}{&amp;:a</ins>;}{\\par}</p>
<p><i>Each pair of { } is a group to be matched. The difference is that the usual  \w (letters or numbers) is :a. In my expression I have four groups the fist is  the match for the <span class="caps">RTF</span> character and the third is the  match for the <span class="caps">HTML</span> code. Knowing that the Replace  expression is as follows: __charsMapping.Add(&rdquo;\3&rdquo;, @&rdquo;\1&rdquo;);</i></p>
<p>Here is how it would look like in VS:</p>
<p><img src="http://wwww.gbogea.com/upload/regexFindReplace.jpg" alt="" /></p>
<p>Doing this is much easier then creating the dictionary by hand of even  writing a console app to do the replace. Sure I could have used some other text  tool to do the same but it wouldn&rsquo;t be as much fun as figuring out how to do it  in VS.</p>
<p>Here is the final output:</p>
<pre class="brush: csharp" title="code">
Dictionary&lt;string, string&gt; charsMapping = 
    new Dictionary&lt;string, string&gt;();
charsMapping.Add(&quot;&amp;deg;&quot;, @&quot;\'b0&quot;);
charsMapping.Add(&quot;&amp;plusmn;&quot;, @&quot;\'b1&quot;);
charsMapping.Add(&quot;&amp;sup2;&quot;, @&quot;\'b2&quot;);
charsMapping.Add(&quot;&amp;sup3;&quot;, @&quot;\'b3&quot;);
charsMapping.Add(&quot;&amp;acute;&quot;, @&quot;\'b4&quot;);
charsMapping.Add(&quot;&amp;micro;&quot;, @&quot;\'b5&quot;);
charsMapping.Add(&quot;&amp;para;&quot;, @&quot;\'b6&quot;);
charsMapping.Add(&quot;&amp;middot;&quot;, @&quot;\'b7&quot;);
charsMapping.Add(&quot;&amp;cedil;&quot;, @&quot;\'b8&quot;);
charsMapping.Add(&quot;&amp;sup1;&quot;, @&quot;\'b9&quot;);
charsMapping.Add(&quot;&amp;ordm;&quot;, @&quot;\'ba&quot;);
charsMapping.Add(&quot;&amp;raquo;&quot;, @&quot;\'bb&quot;);
charsMapping.Add(&quot;&amp;frac14;&quot;, @&quot;\'bc&quot;);
charsMapping.Add(&quot;&amp;frac12;&quot;, @&quot;\'bd&quot;);
charsMapping.Add(&quot;&amp;frac34;&quot;, @&quot;\'be&quot;);
charsMapping.Add(&quot;&amp;iquest;&quot;, @&quot;\'bf&quot;);
</pre>
<p>Of course the number of characters was much bigger but it doesn&rsquo;t matter here.  If you need more info on regular expressions you might want to check out the <a href="http://www.regular-expressions.info/">Regular-Expressions.info</a> site,  they have a lot of good info on the subject. There are some specifics to using  regular expressions on VS so you need to make sure to visit <a href="http://msdn.microsoft.com/en-us/library/2k3te2cs(VS.80).aspx"><span class="caps">MSDN</span> Reference</a>.</p>
</div>