// Prototype for an observable<->URL binding plugin.
(function () {
	function ensureString(value) { 
        return ((value === null) || (value === undefined)) ? "null" : value.toString();
    }
	var currentParams = {}, updateTimer, $ = window.jQuery;

    ko.linkObservableToUrl = function (observable, hashPropertyName, defaultValue) {
        observable.subscribe(function (value) {
			var valueToWrite = ensureString(value);
			if (currentParams[hashPropertyName] !== valueToWrite) {
                currentParams[hashPropertyName] = valueToWrite;
                queueAction(function () {
                    $.bbq.pushState(currentParams);
                });
            }
        });
        
        $(window).bind("hashchange", function(e){
            var querystring = $.deparam.fragment();
            var newValue = hashPropertyName in querystring ? querystring[hashPropertyName] : null;
            
            if(currentParams[hashPropertyName] == querystring[hashPropertyName]) return;
            
            currentParams[hashPropertyName] = newValue;
            observable(hashPropertyName in querystring ? querystring[hashPropertyName] : defaultValue);
        });
        
        $(window).trigger("hashchange");
    }

    function queueAction(action) {
        if (updateTimer) clearTimeout(updateTimer);
        updateTimer = setTimeout(action, 0);
    }
})();