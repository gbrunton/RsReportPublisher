(function(window,undefined){ 
    var expressionCache  = {};
	ko.utils.evalWithinScope = function (expression /*, scope1, scope2, scope3... */) {
        // Build the source for a function that evaluates "expression"
        // For each scope variable, add an extra level of "with" nesting
        // Example result: with(sc[1]) { with(sc[0]) { return (expression) } }
        var scopes = Array.prototype.slice.call(arguments, 1);
        var functionBody = "return (" + expression + ")";
        for (var i = 0; i < scopes.length; i++) {
            if (scopes[i] && typeof scopes[i] == "object") {
                functionBody = "with(sc[" + i + "]) { " + functionBody + " } ";
            }
        }
        if (expressionCache[functionBody]) {
            return expressionCache[functionBody](scopes);
        } else {
            expressionCache[functionBody] = new Function("sc", functionBody);
            return expressionCache[functionBody](scopes);
        }
	};

	if(typeof ko == "undefined")
    {
	    throw "You must reference Knockout.js in order for the ko.nativeExternalTemplateEngine to work.";
    }
    else
    {
        ko.nativeExternalTemplateEngine = function() {
            var self = this;
            
            this.templateUrl = "",
            this.templateContainerRules = null;
            this.templateSuffix = ".html",
            this.ajax = $.ajax,
            this.templatePrefix = "",
            this.useDefaultErrorTemplate = true,
            this.defaultErrorTemplateHtml = "<div style='font-style: italic;'>The template '{TEMPLATEID}' could not be loaded.  HTTP Status code: {STATUSCODE}.</div>",
            this.ajaxOptions = {},
            this.getTemplateNode = function(templateId) {
                var node = document.getElementById(templateId);
                if (node == null) {
                    var templatePath = this.getTemplatePath(templateId, this.templatePrefix, this.templateSuffix, this.templateUrl);

                    var options = {
                        "url": templatePath + "?randomToPreventCaching=" + (Math.random() * 3),
                        "dataType": "text",
                        "type": "GET",
                        "timeout": 0,
                        "success": function(data) {
                            var css = $(data).filter("style");
                            $("head").append(css);
                            var templates = $(data).filter('script');
                            $("body").append(templates);
                        },
                        "error": function(exception) {
                            if (this.useDefaultErrorTemplate) {
                                node = document.createElement("script");
                                node.type = "text/html";
                                node.id = templateId;
                                node.text = this.defaultErrorTemplateHtml.replace('{STATUSCODE}', exception.status).replace('{TEMPLATEID}', templateId);
                                document.body.appendChild(node);
                                throw new Error("The template '{TEMPLATEID}' could not be loaded.  HTTP Status code: {STATUSCODE}.".replace('{STATUSCODE}', exception.status).replace('{TEMPLATEID}', templateId));
                            }
                        }.bind(this)
                    };

                    $.extend(true, options, this.ajaxOptions);

                    options["async"] = false;

                    this.ajax(options);

                    node = document.getElementById(templateId);
                    if (node == null) throw new Error("Cannot find template with ID=" + templateId);
                }
                return node;
            },
            this.getTemplatePath = function(templateId, templatePrefix, templateSuffix, templateUrl) {
                var templateDescriptor = templateId;
                if (self.templateContainerRules) {
                    $.each(self.templateContainerRules, function(containerFileName, regEx) {
                        if (regEx.test(templateId)) {
                            templateDescriptor = containerFileName;
                            return false;
                        }
                    });
                }
                var templateFile = templatePrefix + templateDescriptor + templateSuffix;
                var templateSrc = templateUrl === undefined || templateUrl === "" ? templateFile : templateUrl + "/" + templateFile;
                return templateSrc;
            },

            this.setOptions = function(options) {
                $.extend(this, options);
            }
        };

        ko.nativeExternalTemplateEngine.prototype = new ko.nativeTemplateEngine();
        ko.nativeExternalTemplateEngine.instance = new ko.nativeExternalTemplateEngine();
        ko.nativeExternalTemplateEngine.prototype.makeTemplateSource = function(template) {
            if (typeof template == "string") {
                var elem = this.getTemplateNode(template);
                return new ko.templateSources.domElement(elem);
            } else if ((template.nodeType == 1) || (template.nodeType == 8)) {
                return new ko.templateSources.anonymousTemplate(template);
            } else throw new Error("Unrecognised template type: " + template);
        };

        ko.nativeExternalTemplateEngine.instance = new ko.nativeExternalTemplateEngine();

        ko.setTemplateEngine(ko.nativeExternalTemplateEngine.instance);
        ko.exportSymbol('ko.nativeExternalTemplateEngine', ko.nativeExternalTemplateEngine);
    }
})(window);                  
