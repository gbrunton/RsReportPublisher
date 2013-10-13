(function () {
    ko.bindingHandlers.jqButton = {
        init: function (element, value) {
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).button("destroy");
            });
        },
        update: function (element, value, allBindings, viewModel) {
            var $el = $(element);
            var jqOptions = value();
            var opts = {};

            var jqOpts = {
                iconPrefix: "ui-icon-",
                label: null,
                leftIcon: null,
                rightIcon: null,
                click: $.noop,
                disabled: false
            };

            var unwrappedOptions = {};
            for (var i in jqOptions) {
                unwrappedOptions[i] = ko.utils.unwrapObservable(jqOptions[i]);
            }

            $.extend(jqOpts, unwrappedOptions);

            opts = {
                label: jqOpts.label,
                icons: {
                    primary: (jqOpts.leftIcon) ? jqOpts.iconPrefix + jqOpts.leftIcon : jqOpts.leftIcon,
                    secondary: (jqOpts.rightIcon) ? jqOpts.iconPrefix + jqOpts.rightIcon : jqOpts.rightIcon
                },
                disabled: jqOpts.disabled
            };

            if (typeof $el.data("button") !== "object") {
                $el.button(opts);
                $el.bind("click.jqButton", function (e) {
                    if (!($(this).attr("disabled") || $(this).button("option", "disabled"))) {
                        jqOpts.click(viewModel, e);
                    }
                });
            }
            else {
                $el.button("option", opts);
            }
        }
    };

    ko.bindingHandlers.jqDialog = {
        model: function (obj) {
            if (!obj) obj = {};
            var defaultButtons = {
                OK: function () {
                    $(this).dialog("close");
                }
            };
            return {
                autoOpen: ko.observable(obj.autoOpen || false),
                modal: ko.observable(obj.modal || true),
                title: ko.observable(obj.title || "Information"),
                position: ko.observable(obj.position || "center"),
                close: obj.close || $.noop,
                width: obj.width,
                height: obj.height,
                buttons: ko.observable(obj.buttons || defaultButtons)
            };
        },
        init: function (element, value) {
            ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                $(element).dialog("destroy");
            });
        },
        update: function (element, value, allBindings) {
            var $el = $(element);
            var jqOptions = value();
            var opts = {
                autoOpen: false,
                modal: true,
                title: "Information",
                position: "center",
                close: $.noop,
                buttons: {
                    OK: function () {
                        $(this).dialog("close");
                    }
                }
            };

            var unwrappedOptions = {};
            for (var i in jqOptions) {
                unwrappedOptions[i] = ko.utils.unwrapObservable(jqOptions[i]);
            }

            $.extend(opts, unwrappedOptions);
            var oldClose = opts.close;

            opts.close = function () {
                oldClose();

                if (ko.isWriteableObservable(value().autoOpen))
                    value().autoOpen(false);
                else {
                    if (allBindings && allBindings['_ko_property_writers'] && allBindings['_ko_property_writers']['jqDialog']['autoOpen'])
                        allBindings['_ko_property_writers']['jqDialog']['autoOpen'](false);
                }
            };

            setTimeout(function () {
                if (typeof $el.data("dialog") !== "object") {
                    $el.dialog(opts);
                }

                if (opts.autoOpen) {
                    if (!$el.dialog("isOpen")) $el.show().dialog("open");
                }
                else {
                    if ($el.dialog("isOpen")) {
                        $el.dialog("close");
                    }
                }

                $el.removeClass("dialog");
                $el.dialog("option", opts);
            }, 0);
        }
    };

    ko.bindingHandlers.jstree = {
        update: function (element, valueAccessor) {
            var $el = $(element);
            var value = valueAccessor();
            var jqOptions = value.options;

            var opts = {
                types: {
                    types: {
                        Folder: {
                            icon: { image: 'Images/folder.png' }
                        },
                        DataSource: {
                            icon: { image: 'Images/database.png' }
                        },
                        CommonReport: {
                            icon: { image: 'Images/commonReport.png' }
                        },
                        StyleSheet: {
                            icon: { image: 'Images/xsl.png' }
                        },
                        CommonStyleSheet: {
                            icon: { image: 'Images/commonXsl.png' }
                        },
                        Parameter: {
                            icon: { image: 'Images/param.png' }
                        },
                        Property: {
                            icon: { image: 'Images/property.png' }
                        },
                        Report: {
                            icon: { image: 'Images/report.png' }
                        },
                        DoesNotInheritPermissionsFolder: {
                            icon: { image: 'Images/lock.png' }
                        }
                    }
                },
                json_data: {
                    data: [],
                    progressive_render: true
                },
                themes: {
                    theme: "default",
                    icons: true
                },
                dnd: {
                    check_timeout: 1000
                },
                unique: {
                    error_callback: function (n, p, f) {
                        $("<div>A node with the name '" + n + "' already exists in this folder. You cannot have two nodes named the same.</div>").dialog({
                            title: "Unique Error",
                            modal: true
                        });
                    }
                },
                hotkeys: {
                    "ctrl+x": function () {
                        this.cut();
                    },
                    "ctrl+c": function () {
                        this.copy();
                    },
                    "ctrl+v": function () {
                        this.paste();
                    },
                    "ctrl+d": function () {
                        this.remove();
                    }
                },
                plugins: [
					"core",
					"themes",
					"ui",
					"json_data",
                    "html_data",
					"hotkeys",
                    "contextmenu",
                    "crrm",
                    "dnd",
                    "cookies",
                    "unique",
                    "types",
                    "search"
				],
                contextmenu: {
                    select_node: true,
                    show_at_node: false,
                    types: {
                        DoesNotInheritPermissionsFolder: {
                            addItem: {
                                label: "Add Report/Stylesheet",
                                icon: false,
                                _class: "litPad",
                                action: function () {

                                }
                            },
                            addDataSource: {
                                label: "Add Data Source",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.create(null, "inside", { attr: { rel: "DataSource" }, data: "DataSource" });
                                }
                            },
                            inheritPermissions: {
                                label: "Inherit Permissions",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("Folder");
                                }
                            }
                        },
                        Folder: {
                            addItem: {
                                label: "Add Report/Stylesheet",
                                icon: false,
                                _class: "litPad",
                                action: function () {

                                }
                            },
                            addDataSource: {
                                label: "Add Data Source",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.create(null, "inside", { attr: { rel: "DataSource" }, data: "DataSource" });
                                }
                            },
                            doNotInheritPermissions: {
                                label: "Un-Inherit Permissions",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("DoesNotInheritPermissionsFolder");
                                }
                            }
                        },
                        ParameterCollection: {
                            addParameter: {
                                label: "Add Parameter",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.create(null, "inside", { attr: { rel: "Parameter" }, data: "NewParam = null" });
                                }
                            }
                        },
                        PropertyCollection: {
                            addProperty: {
                                label: "Add Property",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.create(null, "inside", { attr: { rel: "Property" }, data: "NewProperty = null" });
                                }
                            }
                        },
                        StyleSheet: {
                            makeCommon: {
                                label: "Make Common StyleSheet",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("CommonStyleSheet");
                                }
                            }
                        },
                        CommonStyleSheet: {
                            makeUnCommon: {
                                label: "Make UnCommon StyleSheet",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("StyleSheet");
                                }
                            }
                        },
                        CommonReport: {
                            addParameter: {
                                label: "Add Parameter",
                                icon: false,
                                _class: "litPad",
                                action: function (obj) {
                                    var collection = null;
                                    var findCollection = obj.find("[rel='ParameterCollection']:first");
                                    if (findCollection.length == 0) {
                                        collection = this.create(null, "inside", { attr: { rel: "ParameterCollection" }, data: "Parameters" });
                                    }
                                    else {
                                        collection = findCollection;
                                    }

                                    this.create(collection, "inside", { attr: { rel: "Parameter" }, data: "NewParameter = null" });
                                }
                            },
                            addProperty: {
                                label: "Add Property",
                                icon: false,
                                _class: "litPad",
                                action: function (obj) {
                                    var collection = null;
                                    var findCollection = obj.find("[rel='PropertyCollection']:first");
                                    if (findCollection.length == 0) {
                                        collection = this.create(null, "inside", { attr: { rel: "PropertyCollection" }, data: "Properties" });
                                    }
                                    else {
                                        collection = findCollection;
                                    }

                                    this.create(collection, "inside", { attr: { rel: "Property" }, data: "NewProperty = null" });
                                }
                            },
                            makeUnCommon: {
                                label: "Make UnCommon Report",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("Report");
                                }
                            }
                        },
                        Report: {
                            addParameter: {
                                label: "Add Parameter",
                                icon: false,
                                _class: "litPad",
                                action: function (obj) {
                                    var collection = null;
                                    var findCollection = obj.find("[rel='ParameterCollection']:first");
                                    if (findCollection.length == 0) {
                                        collection = this.create(null, "inside", { attr: { rel: "ParameterCollection" }, data: "Parameters" });
                                    }
                                    else {
                                        collection = findCollection;
                                    }

                                    this.create(collection, "inside", { attr: { rel: "Parameter" }, data: "NewParameter = null" });
                                }
                            },
                            addProperty: {
                                label: "Add Property",
                                icon: false,
                                _class: "litPad",
                                action: function (obj) {
                                    var collection = null;
                                    var findCollection = obj.find("[rel='PropertyCollection']:first");
                                    if (findCollection.length == 0) {
                                        collection = this.create(null, "inside", { attr: { rel: "PropertyCollection" }, data: "Properties" });
                                    }
                                    else {
                                        collection = findCollection;
                                    }

                                    this.create(collection, "inside", { attr: { rel: "Property" }, data: "NewProperty = null" });
                                }
                            },
                            makeCommon: {
                                label: "Make Common Report",
                                icon: false,
                                _class: "litPad",
                                action: function () {
                                    this.set_type("CommonReport");
                                }
                            }
                        }
                    },
                    defaults: {
                        rename: {
                            label: "Rename <span style='position:absolute;right:5px;font-size:.8em'>f2</span>",
                            icon: false,
                            _class: "litPad",
                            separator_after: true,
                            action: function () {
                                this.rename();
                            }
                        },
                        copy: {
                            label: "Copy <span style='position:absolute;right:5px;font-size:.8em'>ctrl + c</span>",
                            icon: false,
                            _class: "litPad",
                            separator_before: true,
                            action: function () {
                                this.copy();
                            }
                        },
                        cut: {
                            label: "Cut <span style='position:absolute;right:5px;font-size:.8em'>ctrl + x</span>",
                            icon: false,
                            _class: "litPad",
                            separator_after: true,
                            action: function () {
                                this.cut();
                            }
                        },
                        paste: {
                            label: "Paste <span style='position:absolute;right:5px;font-size:.8em'>ctrl + v</span>",
                            icon: false,
                            _class: "litPad",
                            action: function () {
                                this.paste();
                            }
                        },
                        remove: {
                            label: "Remove <span style='position:absolute;right:5px;font-size:.8em'>ctrl + d</span>",
                            icon: false,
                            _class: "litPad",
                            action: function () {
                                this.remove();
                            }
                        }
                    }
                },
                selectNode: $.noop,
                hoverNode: function (e, data) { },
                renameNode: $.noop,
                deleteNode: $.noop,
                moveNode: $.noop,
                setType: $.noop,
                createNode: $.noop,
                treeLoaded: $.noop,
                on_contextmenu: $.noop
            };

            opts.contextmenu.items = function (obj) {
                var type = obj.attr("rel");
                return $.extend({}, opts.contextmenu.types[type], opts.contextmenu.defaults);
            };

            $.extend(true, opts, jqOptions);

            value.tree($el
                    .unbind("loaded.jstree")
                    .unbind("select_node.jstree")
                    .unbind("hover_node.jstree")
                    .unbind("dehover_node.jstree")
                    .unbind("rename_node.jstree")
                    .unbind("delete_node.jstree")
                    .unbind("move_node.jstree")
                    .unbind("set_type.jstree")
                    .unbind("create_node.jstree")
                    .unbind("contextmenu.jstree")

                    .bind("loaded.jstree", opts.treeLoaded)
                    .bind("select_node.jstree", opts.selectNode)
                    .bind("hover_node.jstree", opts.hoverNode)
                    .bind("rename_node.jstree", opts.renameNode)
                    .bind("delete_node.jstree", opts.deleteNode)
                    .bind("move_node.jstree", opts.moveNode)
                    .bind("set_type.jstree", opts.setType)
                    .bind("create_node.jstree", opts.createNode)
                    .bind("contextmenu.jstree", opts.on_contextmenu)

                    .jstree(opts)
                    .css("background-color", "transparent"));
        }
    };
})()