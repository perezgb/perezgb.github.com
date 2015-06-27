---
layout: post
title: "MVC Storefront: Migrating to the Entity Framework"
description: ""
date: 2009-04-22 12:00:00 UTC
category: 
tags: [entityframework, ado.net, linq]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>The title of the post is actually misleading. A better title (although too long) would be: Migrating the Repository concept used in the <span class="caps">MVC</span> Storefront to the Entity Framework.</p>
<p>Lately I&rsquo;ve been playing more with the Entity Framework. The other day I was trying to replicate the way Rob Conery used the Repository Pattern in the <span class="caps">MVC</span> Storefront sample application and I was surprised to learn that the Entity Framework has some differences from <span class="caps">LINQ</span> to <span class="caps">SQL</span> that I did not expect.</p>
<p>My first test was very simple. I wanted to get an IQueryable of Posts with it&rsquo;s respective Comments attached. The StoreFront did this in a really clever and simple manner. The following code was supposed to work:</p>
<pre title="code" class="brush: csharp">
class BlogRepository
{
    SOEntities ctx = new SOEntities();

    public IQueryable&lt;Comment&gt; GetComments()
    {
        return from comment in ctx.Comments.Include(&quot;Post&quot;)
               select new Comment()
               {
                   CommentId = comment.CommentId,
                   Author = comment.Author,
                   Body = comment.Body,
                   PostId = comment.Post.PostId
               };
    }

    public IQueryable&lt;Post&gt; GetPosts()
    {
        var posts = from post in ctx.Posts
                    let comments = GetComments()
                    select new Post()
                           {
                               PostId = post.PostId,
                               Title = post.Title,
                               Body = post.Body,
                               Comments = comments.Where(x =&gt; x.PostId == post.PostId)
                           };
        return posts;
    }
}

class Post
{
    public Guid PostId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public IList&lt;Comment&gt; Comments { get; set; }
}

class Comment
{
    public Guid CommentId { get; set; }

    public Guid PostId { get; set; }

    public string Author { get; set; }

    public string Body { get; set; }

}

class Program 
{
static void Main(string[] args)
{
    BlogRepository rep = new BlogRepository();
    var posts = rep.GetPosts();
    posts = posts.Where(x =&gt; x.Title.Contains(&quot;MVC&quot;));
    List&lt;Post&gt; postList = posts.ToList();
}
}</pre>
<p>I get an error saying that Linq to Entities does not recognize the method GetComments().</p>
<p>It&rsquo;s not a bug, it&rsquo;s is just the way the EF works. The method GetComments can&rsquo;t be translated to anything in the provider. But why does it work with Linq to Sql you ask? Because it converts the Linq expression in a different way. Sorry but the post would be too long if I got into this subject.</p>
<p>The way recommended by the EF team is you use the AsEnumerable() extension method, forcing the initial query to be executed and then you would be working with objects. Here is how I needed to change my code for this to work:</p>
<pre title="code" class="brush: csharp">
public IQueryable&lt;Post&gt; GetPosts()
{
    var posts = from post in ctx.Posts.AsEnumerable()
                let comments = GetComments()
                select new Post()
                       {
                           PostId = post.PostId,
                           Title = post.Title,
                           Body = post.Body,
                           Comments = comments.Where(x =&gt; x.PostId == post.PostId)
                       };
    return posts.AsQueryable();
}
</pre>
<p>This does work, but when ToList() method is called in the posts object, instead of filtering the posts in the sql code, all the posts are brought from the database and then they are filtered as objects. This is a work around but it&rsquo;s not ideal.</p>
<p>I have another work around, it also isn&rsquo;t the ideal but it does work in the sense that it will apply the filter in the database.</p>
<pre title="code" class="brush: csharp">
public IQueryable&lt;Post&gt; GetPosts()
{
    var comments = GetComments();
    var posts = from post in ctx.Posts
                select new Post()
                       {
                           PostId = post.PostId,
                           Title = post.Title,
                           Body = post.Body,
                           CommentsQry = comments.Where(x =&gt; x.PostId == post.PostId)
                       };
    return posts;
}

class Post
{
    public Guid PostId { get; set; }

    public string Title { get; set; }

    public string Body { get; set; }

    public IList&lt;Comment&gt; Comments { get; set; }

    public IQueryable&lt;Comment&gt; CommentsQry
    {
        set { Comments = new LazyList&lt;Comment&gt;(value); }
    }
}
</pre>
<p>The trick is that I introduced a IQueryable property in the Post class. This property receives an IQueryable and under the hood converts it to a LazyList and sets the Comments property. Doing this overcomes the limitation that the EF has in not letting you use constructors that take parameters in the EF code. It only allows parameterless constructors on the select projection.</p>
<p>If you run this code you will see that the records are filtered in the database. The reason I still don&rsquo;t consider this solution ideal is that I had to make a change to my model class and created a property that is not related to the business but rather to a limitation imposed by the framework. I still don&rsquo;t see a way around this. If anyone has any suggestions please do leave them as comments.</p>
<p>A very interesting post was written by <a href="http://mosesofegypt.net/post/2008/08/31/LINQ-to-Entities-Workarounds-on-what-is-not-supported.aspx">Muhammad Mosa</a> a while ago. It&rsquo;s worth visiting his <a href="http://mosesofegypt.net/post/2008/08/31/LINQ-to-Entities-Workarounds-on-what-is-not-supported.aspx">blog</a> to checkout his idea.</p>
</div>