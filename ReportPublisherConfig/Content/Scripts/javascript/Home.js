var SSA = SSA || { };
(function (home, undefined) {
    amplify.request.decoders.customJsend = function( data, status, ampXHR, success, error ) {
		if(data){
			if(data.d) data= data.d;
			if ( data.status === "success" ) {
				success( data.data );
			} else if ( data.status === "fail" ) {
				error( data, ampXHR);
			} else if ( data.status === "error" ) {
				error( data, ampXHR);
			}
		}
		if(status.indexOf("error") > -1){
			error({ message: status, status: status }, ampXHR);
		}
	};
    
    SSA.HomeModel = (function () {
        ko.baseModel.extend(HomeModel);
        HomeModel.page = "/home";
        HomeModel.webMethods = {
            save: { url: HomeModel.page + "/save", type:"POST", contentType: "application/x-www-form-urlencoded", dataType: "json" },
            publishReports: { url: HomeModel.page + "/publish-reports", type:"POST", contentType: "application/x-www-form-urlencoded", dataType: "json" }
		};
        
        HomeModel.ajaxDefaults = {
            decoder: "customJsend"
        };
        
        function HomeModel() {
            var model = this;
            
            function showLoadingDialog() {
                var loadingDialog = $("#loadingDialog");
                if(loadingDialog.length > 0) loadingDialog.remove();
                $("<div id='loadingDialog' style='padding: 5px;text-align:center;'><img align='center' src='Images/loader.gif' /></div>").dialog({
                    modal: true,
                    dialogClass: "noTitleStuff",
                    height: 50,
                    width: 128
                });
            }

            function showConfirm(msg, yes, no) {
                var confirmDialog = $("#confirm");
                if(confirmDialog.length > 0) confirmDialog.remove();
                $("<div id='confirm' style='padding: 5px;text-align:center;'>"+ msg +"</div>").dialog({
                    modal: true,
                    dialogClass: "noTitleStuff",
                    buttons: {
                        Yes: function () {
                            if($.isFunction(yes)) yes();
                            $(this).dialog("close");
                        },
                        No: function () {
                            if($.isFunction(no)) no();
                            $(this).dialog("close");
                        }
                    }
                });
            }

            model.beforeAjax = function() {
                showLoadingDialog();
            };
            
            model.onAjaxComplete = function() {
                $("#loadingDialog").dialog("close");
            };

            var publishReports = function(folder, obj) {
                model.publishDialog.autoOpen(true);
                model.publishDialogMethod = function() {
                    var canPublish = $(obj).attr("canPublish");
                    if (canPublish == "true") {
                        model.validateAndRequest("publishReports", { folderName: folder, site: model.get("selectedSite") }, function() {
                            model.publishDialog.autoOpen(false);
                        });
                    }
                };
            };

            var addReport = {
                publishReport: {
                    label: "<span id='publishReports'>Publish Reports</span>",
                    icon: false,
                    _class: "litPad",
                    separator_after: true,
                    action: function (obj) {
                        var that = this;
                        
                        if (model.get("needsSaved")) {
                            showConfirm("You have unsaved changes, publishing reports will save your current changes the publish. Are you sure you want to do this?", function() {
                                model.save(function () {
                                    publishReports(that.get_text(), obj);                              
                                });
                            });
                        }
                        else {
                            publishReports(that.get_text(), obj);
                        }
                    }
                },
                addItem: {
                    action: function(obj) {
                        model.folderToAddReportTo(this);
                        model.showAddReportDialog();
                    }
                },
                createFolder: {
                    label: "Create Folder",
                    icon: false,
                    _class: "litPad",
                    action: function () {
                        this.create(null, "inside", { attr: { rel: "Folder" }, data: "New Folder" });
                    }
                }
            };
            
            function needSaved() {
                model.set({needsSaved: true});
            }

            model.jsTree = {
                tree: ko.observable(),
                options: {
                    renameNode: needSaved,
                    deleteNode: needSaved,
                    moveNode: needSaved,
                    setType: needSaved,
                    createNode: needSaved,
                    on_contextmenu: function(e) {
                        var canPublish = $(this).jstree("get_selected").attr("canPublish") || $(this).jstree("get_selected").parent().parent().attr("id") == "configuration";
                        $("#publishReports").hide();
                        if(canPublish) {
                            $("#publishReports").show();
                        }
                    },
                    contextmenu: {
                        types: {
                            Folder: addReport,
                            DoesNotInheritPermissionsFolder: addReport
                        }
                    }
                }
            };
            
            model.needsSaved = ko.observable(false);
            model.reportToAdd = ko.observable();
            model.reportToAddIsCommon = ko.observable();
            model.folderToAddReportTo = ko.observable();
            model.sites = ko.observableArray();
            model.selectedSite = ko.observable();
            model.publishDialogMethod = $.noop;
            
            window.onbeforeunload = confirmExit;
            function confirmExit(){
                if(model.get("needsSaved"))
                    return "You have unsaved changes.  If you leave this page without clicking the Save button, your changes will be lost.  Are you sure you want to exit this page without saving?";
            }

            model.reportToAdd.subscribe(function(newValue) {
                if(newValue.indexOf("CommonReports") > -1) model.set({ reportToAddIsCommon: true });            
            });

            model.showAddReportDialog = function() {
                model.addReportDialog.autoOpen(true);
            };
            
            model.publishDialog = new ko.bindingHandlers.jqDialog.model({
                title: "Publish Reports",
                width: 400,
                buttons: {
                    Ok: function () {
                        model.publishDialogMethod();
                    }
                }
            });

            model.addReportDialog = new ko.bindingHandlers.jqDialog.model({
                title: "Add a Report/Stylesheet",
                width: 400,
                buttons: {
                    Ok: function () {
                        model.addReport();
                    }
                }
            });

            model.addReport = function() {
                var type = "Report";
                var reportPath = model.get("reportToAdd");
                var name = reportPath.substring(reportPath.lastIndexOf("\\") + 1);
                name = name.substring(0, name.lastIndexOf("."));
                var extension = reportPath.substring(reportPath.lastIndexOf(".") + 1);
                if(extension === "xsl") type = "StyleSheet";

                if(model.get("reportToAddIsCommon")) type = "Common" + type;
                
                model.get("folderToAddReportTo").create(null, "inside", { attr: { rel: type }, data: name });
                model.addReportDialog.autoOpen(false);
            };
            
            model.save = function(callback) {
                showLoadingDialog();
                setTimeout(function() {
                    var modelToServer = model.jsTree.tree().jstree("get_json", -1)[0];
                    model.validateAndRequest("save", { data: JSON.stringify(modelToServer) }, function() {
                        model.set({ needsSaved: false });
                        if($.isFunction(callback)) callback();
                    });
                }, 1);
            };

            model.collapseAll = function() {
                model.jsTree.tree().jstree("close_all", false);
            };
            
            HomeModel.__super__.constructor.call(this);
        }
        return HomeModel;
    })();

    home.init = function (obj) {
        var vm = new SSA.HomeModel();
        ko.applyBindings(vm);
        $("#container").height($(window).height() - 175);
        vm.set({sites: obj});
    };
} (SSA.Home = SSA.Home || {}));