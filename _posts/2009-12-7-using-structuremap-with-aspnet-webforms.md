---
layout: post
title: "Using StructureMap with ASP.NET WebForms"
description: ""
date: 2009-12-07 12:00:00 UTC
category: 
tags: [speakout, structuremap, ioc, asp.net]
comments: true
---
{% include JB/setup %}

<div id="post">
<p>StrutctureMap is one of the most popular IoC containers in .NET but I had never tried it out. You can learn all about StructureMap from the documentation and the tutorials already available so I won't try to explain what it is and how it works. Instead I'll show you my experience on how to use it with ASP.NET WebForms.</p>
<p>To test StructureMap I decided to use it in <a href="http://speakoutblog.codeplex.com/">my blog</a> which is a simple WebForms application that is <a href="http://www.gbogea.com/2009/11/30/from-entity-framework-to-nhibernate">now using NHibernate</a>. The objective is use in the code only the interfaces for my Services and DAOs. Their implementation should be injected.</p>
<p>The first step is to create a class that will be responsible for initializing the IoC. I decided to create this class at the WebSite level in the App_Code folder because this class has dependencies on all the other assemblies. Since the WebSite has to reference all these assemblies I figured this would be the best place. Here is the code for the class:</p>
<pre class="brush: csharp" title="code">
public class IocConfigurator
{
    public static void Configure()
    {
        ObjectFactory.Initialize(x =&gt;
        {
            x.ForRequestedType&lt;IPostDao&gt;().TheDefault.Is.
                OfConcreteType&lt;PostDao&gt;();
            x.ForRequestedType&lt;ICategoryDao&gt;().TheDefault.Is.
                OfConcreteType&lt;CategoryDao&gt;();
            x.ForRequestedType&lt;ICommentDao&gt;().TheDefault.Is.
                OfConcreteType&lt;CommentDao&gt;();
            x.ForRequestedType&lt;IAuthorDao&gt;().TheDefault.Is.
                OfConcreteType&lt;AuthorDao&gt;();
            x.ForRequestedType&lt;ITagDao&gt;().TheDefault.Is.
                OfConcreteType&lt;TagDao&gt;();
            x.ForRequestedType&lt;IFrontEndService&gt;().TheDefault.Is.
                OfConcreteType&lt;FrontEndService&gt;();
            x.ForRequestedType&lt;IAdminService&gt;().TheDefault.Is.
                OfConcreteType&lt;AdminService&gt;();

            x.SetAllProperties(y =&gt;
                                   {
                                       y.OfType&lt;IFrontEndService&gt;();
                                       y.OfType&lt;IAdminService&gt;();
                                   });
        });
    }
}</pre>
<p>&nbsp;Now we have to class this IocConfiguration class in order to make all the initialization. The best place to do this is in the Global.asax in the Application_Start event.</p>
<pre title="code" class="brush: xhtml">
&lt;%@ Application Language=&quot;C#&quot; %&gt;
&lt;%@ Import Namespace=&quot;StructureMap&quot;%&gt;

&lt;script runat=&quot;server&quot;&gt;

    void Application_Start(object sender, EventArgs e) 
    {
        IocConfigurator.Configure();
    }
    
...    
&lt;/script&gt;</pre>
<h2>&nbsp;Top-Down resolution</h2>
<p>Ok, now let's deal with how to inject the services in the Pages. Let's continue using the IFrontEndService as our example. We could do something simple like:</p>
<pre class="brush: csharp" title="code">
IFrontEndService service = ObjectFactory.GetInstance&lt;IFrontEndService&gt;();</pre>
<p>But this is not a recommend approach as you can see in the <a href="http://structuremap.sourceforge.net/QuickStart.htm">StructureMap Quickstart</a>. You should minimize the GetInstance calls and let auto wiring do the job for you. Unfortunately WebForms doesn't make that easy. For that reason the team created the <a href="http://codebetter.com/blogs/jeremy.miller/archive/2009/01/16/quot-buildup-quot-existing-objects-with-structuremap.aspx">BuildUp feature</a> that let's you inject the dependencies into an object that was already built.</p>
<p>In order to do the BuildUp into a single place I posted the code into the BasePage constructor. This class derives from the Page class and all the pages in my application derive from it instead of from the Page as would be usual.</p>
<pre title="code" class="brush: csharp">
namespace SpeakOut.Lib.Web
{
    public class BasePage : Page
    {
        public BasePage()
        {
            ObjectFactory.BuildUp(this);
        }
    }
}</pre>
<p>Here is a page using the BasePage:</p>
<pre title="code" class="brush: csharp">
public partial class _Default : BasePage 
{
    public IFrontEndService Service { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RepeaterPosts.DataSource = Service.GetRecentPosts(10);
            RepeaterPosts.DataBind();
        }
    }
    protected void RepeaterPosts_ItemCreated(object sender, RepeaterItemEventArgs e)
    {
        Post post = (Post) e.Item.DataItem;
        PostDetailsControl control = (PostDetailsControl) e.Item.FindControl(&quot;PostDetailsControl1&quot;);
        control.Post = post;
    }
}</pre>
<p>The BuildUp method will look for properties of the type I have registered in the IoC Configuration and will inject the properties with the correct implementation. The registration of the type of properties that should be injected is a special part of the IoC setup. In our configuration class it is at the very end of the Configure method with this syntax:</p>
<pre class="brush: csharp" title="code">
x.SetAllProperties(y =&gt;
                       {
                           y.OfType&lt;IFrontEndService&gt;();
                           y.OfType&lt;IAdminService&gt;();
                       });</pre>
<p>&nbsp;With this StructureMap will take care of all the dependency injection for you. Including injecting the DAOs into the Service. In order for the DAOs to be injected all you need to do is to define a constructor that receives all the values you want to have injected. When StructureMap creates the Service it will look for the constructor with most parameters (the greediest). If it sees that it has all the parameters in the configuration it will automatically create the necessary objects and pass them to the constructor.</p>
<pre title="code" class="brush: csharp">
    public class FrontEndService : IFrontEndService
    {

        private readonly IPostDao _postDao;
        private readonly ICategoryDao _categoryDao;

        public FrontEndService(IPostDao postDao, ICategoryDao categoryDao)
        {
            _postDao = postDao;
            _categoryDao = categoryDao;
        }
...</pre>
<p>&nbsp;That makes the creation of a tree of objects really simple and your code doesn't need to know anything about the interface implementations.</p>
<p>You can get the whole implementation from the <a href="http://speakoutblog.codeplex.com/">SpeakOut Codeplex Project</a>.</p>
</div>