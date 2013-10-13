using System;
using System.Collections.Generic;
using System.Linq;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Runtime;

namespace ReportPublisherConfig.Configuration.Conventions
{
    public class AjaxConvention : IConfigurationAction
    {
        public void Configure(BehaviorGraph graph)
        {
            graph
                .Actions()
                .Where(action => action.HasAttribute<AjaxActionAttribute>())
                .Each(action => action.WrapWith(typeof(AjaxActionWrapper<>).MakeGenericType(action.OutputType())));

            // TODO: Jeremy Miller says I should also do "Oh, and I forgot, make sure that you remove any other output nodes.  You may need to make 
            // the node for the new behavior be an OutputNode to stop other policies from trying to add stuff to it" but I don't know how.
            // See here: https://groups.google.com/forum/#!topic/fubumvc-devel/9Tl7cT20o0o
        } 
    }

    public class AjaxActionAttribute : Attribute
    {
    }

    public class AjaxActionWrapper<T> : IActionBehavior where T : class
    {
        private readonly IActionBehavior inner;
        private readonly IJsonWriter writer;
        private readonly IFubuRequest request;

        public AjaxActionWrapper(IActionBehavior inner, IJsonWriter writer, IFubuRequest request)
        {
            this.inner = inner;
            this.writer = writer;
            this.request = request;
        }

        public void Invoke()
        {
            var jSendDto = new JSendDto();
            try
            {
                this.inner.Invoke();
                jSendDto.data = this.request.Get<T>();
            }
            catch (Exception ex)
            {
                jSendDto.Error(ex.GetBaseException().ToString());
            }
            writer.Write(jSendDto, MimeType.Json.ToString());
        }

        public void InvokePartial()
        {
            throw new NotImplementedException();
        }
    }

    public class JSendDto
    {
        private const string fail = "fail";
        private readonly Dictionary<string, string> failures;

        public JSendDto()
        {
            this.status = "success";
            this.message = string.Empty;
            this.failures = new Dictionary<string, string>();
        }

        public string status { get; private set; }
        public object data { get; set; }
        public string message { get; set; }

        public void Error(string errorMessage)
        {
            this.status = "error";
            this.message = errorMessage;
        }

        public bool HasFailed()
        {
            return this.status == fail;
        }

        public void AddFailure(string paymentType, string format)
        {
            this.status = fail;
            this.failures.Add(paymentType, format);
            this.data = this.failures;
        }
    }
}